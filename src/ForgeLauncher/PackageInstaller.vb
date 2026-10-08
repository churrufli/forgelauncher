Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading
Imports ICSharpCode.SharpZipLib.BZip2
Imports ICSharpCode.SharpZipLib.Tar

''' <summary>
''' Installs Forge packages (<c>forge-installer-*.tar.bz2</c>) and keeps the Forge folder clean.
''' </summary>
''' <remarks>
''' An installation works in three steps, so that a failure never leaves the user without Forge:
''' <list type="number">
''' <item>The new package is extracted over the existing files.</item>
''' <item>Files listed for the previous package (in <c>installed-files.txt</c>) that the new package does not
''' contain are deleted, together with folders that become empty.</item>
''' <item>The list of files of the new package is saved for the next update.</item>
''' </list>
''' The result is the same as deleting the old version first, but without the risk.
''' Files that never came from a package (user data, the launcher, forge.profile.properties) are never touched.
''' </remarks>
Public NotInheritable Class PackageInstaller
    Private Const BufferSize As Integer = 1 << 16
    Private Const PartialSuffix As String = ".part"

    Private Sub New()
    End Sub

    ''' <summary>Temporary file name used while a package is downloading.</summary>
    ''' <param name="downloadsFolder">The <c>fldata\downloads</c> folder.</param>
    ''' <param name="packageUrl">Download URL of the package.</param>
    Public Shared Function GetDownloadPath(downloadsFolder As String, packageUrl As String) As String
        Return Path.Combine(downloadsFolder, PackageFileName(packageUrl) & PartialSuffix)
    End Function

    ''' <summary>File name of a package, taken from its download URL.</summary>
    Public Shared Function PackageFileName(packageUrl As String) As String
        Return Uri.UnescapeDataString(Path.GetFileName(New Uri(packageUrl).LocalPath))
    End Function

    ''' <summary>Deletes unfinished downloads left behind when the launcher was closed during a download.</summary>
    ''' <param name="downloadsFolder">The <c>fldata\downloads</c> folder.</param>
    Public Shared Sub DeleteLeftovers(downloadsFolder As String)
        If Not Directory.Exists(downloadsFolder) Then Return
        For Each leftover In ForgeInstallation.SafeGetFiles(downloadsFolder, "*" & PartialSuffix)
            TryDelete(leftover)
        Next
    End Sub

    ''' <summary>
    ''' Extracts a <c>.tar.bz2</c> package into a folder, overwriting existing files.
    ''' Entry names are read as UTF-8 (Forge packages contain names such as <c>Chandra’s Tome</c>), file times are
    ''' preserved, and entries that would land outside the destination folder (<c>../</c>) are rejected.
    ''' </summary>
    ''' <param name="archivePath">Package to extract.</param>
    ''' <param name="destination">Forge folder.</param>
    ''' <param name="progress">Receives the completed percentage, based on how much of the package was read.</param>
    ''' <param name="cancellationToken">Cancels the extraction between files.</param>
    ''' <returns>Paths of the extracted files, relative to the destination.</returns>
    Public Shared Function Extract(archivePath As String, destination As String, progress As IProgress(Of Integer), cancellationToken As CancellationToken) As HashSet(Of String)
        Dim root = WithTrailingSeparator(Path.GetFullPath(destination))
        Dim extracted As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

        Using archive As New FileStream(archivePath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize)
            Using bzip As New BZip2InputStream(archive)
                Using tar As New TarInputStream(bzip, Encoding.UTF8)
                    Dim lastPercent = -1
                    Dim entry = tar.GetNextEntry()
                    Do While entry IsNot Nothing
                        cancellationToken.ThrowIfCancellationRequested()
                        Dim relativePath = NormalizeEntryName(entry.Name)
                        If relativePath.Length > 0 Then
                            Dim target = Path.GetFullPath(Path.Combine(root, relativePath))
                            If Not target.StartsWith(root, StringComparison.OrdinalIgnoreCase) Then
                                Throw New InvalidDataException($"The package contains an unsafe path: {entry.Name}")
                            End If
                            If entry.IsDirectory Then
                                Directory.CreateDirectory(target)
                            ElseIf IsRegularFile(entry) Then
                                WriteEntry(tar, entry, target)
                                extracted.Add(relativePath)
                            End If
                        End If

                        ' The compressed stream position is the only reliable measure of progress, because the
                        ' number of files in the package is not known in advance.
                        Dim percent = CInt(archive.Position * 100L \ Math.Max(1L, archive.Length))
                        If percent <> lastPercent Then
                            lastPercent = percent
                            progress?.Report(percent)
                        End If
                        entry = tar.GetNextEntry()
                    Loop
                End Using
            End Using
        End Using

        Return extracted
    End Function

    ''' <summary>Reads the list of files installed by the previous package.</summary>
    ''' <param name="manifestFile">The <c>fldata\installed-files.txt</c> file.</param>
    ''' <returns>The list, or <c>Nothing</c> when the file does not exist (first update after a manual install).</returns>
    Public Shared Function ReadManifest(manifestFile As String) As HashSet(Of String)
        If Not File.Exists(manifestFile) Then Return Nothing
        Try
            Return New HashSet(Of String)(
                File.ReadAllLines(manifestFile, Encoding.UTF8).
                    Select(Function(line) line.Trim()).
                    Where(Function(line) line.Length > 0).
                    Select(Function(line) line.Replace("/"c, Path.DirectorySeparatorChar).Replace("\"c, Path.DirectorySeparatorChar)),
                StringComparer.OrdinalIgnoreCase)
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            Return Nothing
        End Try
    End Function

    ''' <summary>Saves the list of files installed by the current package, one relative path per line, sorted.</summary>
    ''' <param name="manifestFile">The <c>fldata\installed-files.txt</c> file.</param>
    ''' <param name="files">Relative paths returned by <see cref="Extract"/>.</param>
    Public Shared Sub WriteManifest(manifestFile As String, files As IEnumerable(Of String))
        Directory.CreateDirectory(Path.GetDirectoryName(manifestFile))
        File.WriteAllLines(manifestFile, files.OrderBy(Function(name) name, StringComparer.OrdinalIgnoreCase), New UTF8Encoding(False))
    End Sub

    ''' <summary>
    ''' Deletes files of the previous package that the new package does not contain, then deletes folders left empty.
    ''' Each path is validated again before deletion, so a damaged or edited list can never delete anything outside
    ''' the Forge folder or any of the launcher's protected files.
    ''' </summary>
    ''' <param name="paths">Launcher paths (defines the Forge folder and the protected files).</param>
    ''' <param name="previous">Files of the previous package.</param>
    ''' <param name="current">Files of the package that was just extracted.</param>
    ''' <returns>Relative paths of the deleted files.</returns>
    Public Shared Function RemoveOrphans(paths As LauncherPaths, previous As ISet(Of String), current As ISet(Of String)) As List(Of String)
        Dim root = WithTrailingSeparator(Path.GetFullPath(paths.ForgeFolder))
        Dim removed As New List(Of String)
        Dim touchedFolders As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

        For Each relativePath In previous
            If current.Contains(relativePath) OrElse paths.IsProtected(relativePath) Then Continue For
            Dim target As String
            Try
                target = Path.GetFullPath(Path.Combine(root, relativePath))
            Catch ex As Exception When TypeOf ex Is ArgumentException OrElse TypeOf ex Is NotSupportedException OrElse TypeOf ex Is PathTooLongException
                Continue For
            End Try
            If Not target.StartsWith(root, StringComparison.OrdinalIgnoreCase) Then Continue For
            If File.Exists(target) AndAlso TryDelete(target) Then
                removed.Add(relativePath)
                touchedFolders.Add(Path.GetDirectoryName(target))
            End If
        Next

        RemoveEmptyFolders(touchedFolders, root)
        Return removed
    End Function

    ''' <summary>
    ''' Fallback for the first update of an installation that has no file list yet: deletes only the old Forge jars
    ''' (recognizable by their <c>-jar-with-dependencies.jar</c> suffix) that the new package did not install.
    ''' Nothing else is deleted, because without a list the launcher cannot tell Forge files from user files.
    ''' </summary>
    ''' <param name="folder">Forge folder.</param>
    ''' <param name="extracted">Files of the package that was just extracted.</param>
    ''' <returns>Names of the deleted jars.</returns>
    Public Shared Function RemoveStaleJars(folder As String, extracted As ISet(Of String)) As List(Of String)
        Dim removed As New List(Of String)
        Dim packageHasJars = extracted.Any(Function(name) name.IndexOf(Path.DirectorySeparatorChar) < 0 AndAlso
                                                          name.EndsWith(ForgeInstallation.JarSuffix, StringComparison.OrdinalIgnoreCase))
        If Not packageHasJars Then Return removed

        For Each jarPath In ForgeInstallation.SafeGetFiles(folder, "*" & ForgeInstallation.JarSuffix)
            Dim name = Path.GetFileName(jarPath)
            If extracted.Contains(name) Then Continue For
            If TryDelete(jarPath) Then removed.Add(name)
        Next
        Return removed
    End Function

    ''' <summary>Deletes a file if it exists, clearing the read-only flag first.</summary>
    ''' <returns><c>False</c> when the file is locked or access is denied.</returns>
    Public Shared Function TryDelete(filePath As String) As Boolean
        Try
            If File.Exists(filePath) Then
                File.SetAttributes(filePath, FileAttributes.Normal)
                File.Delete(filePath)
            End If
            Return True
        Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Deletes the given folders and their parents while they are empty, deepest first, never going above the
    ''' Forge folder.
    ''' </summary>
    Private Shared Sub RemoveEmptyFolders(folders As IEnumerable(Of String), root As String)
        For Each startFolder In folders.OrderByDescending(Function(folder) folder.Length)
            Dim current = startFolder
            Do While current IsNot Nothing AndAlso
                     WithTrailingSeparator(current).StartsWith(root, StringComparison.OrdinalIgnoreCase) AndAlso
                     Not String.Equals(WithTrailingSeparator(current), root, StringComparison.OrdinalIgnoreCase)
                Try
                    If Not Directory.Exists(current) OrElse Directory.EnumerateFileSystemEntries(current).Any() Then Exit Do
                    Directory.Delete(current)
                Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
                    Exit Do
                End Try
                current = Path.GetDirectoryName(current)
            Loop
        Next
    End Sub

    ''' <summary>Converts a tar entry name into a relative path with the system separator, without "./" prefixes.</summary>
    Private Shared Function NormalizeEntryName(name As String) As String
        Dim normalized = name.Replace("\"c, "/"c)
        Do While normalized.StartsWith("./", StringComparison.Ordinal)
            normalized = normalized.Substring(2)
        Loop
        Return normalized.Trim("/"c).Replace("/"c, Path.DirectorySeparatorChar)
    End Function

    ''' <summary><c>True</c> for regular files; links and special entries are skipped.</summary>
    Private Shared Function IsRegularFile(entry As TarEntry) As Boolean
        Dim flag = entry.TarHeader.TypeFlag
        Return flag = TarHeader.LF_NORMAL OrElse flag = TarHeader.LF_OLDNORM OrElse flag = TarHeader.LF_CONTIG
    End Function

    ''' <summary>Writes one file from the package, replacing read-only files and keeping the original file time.</summary>
    Private Shared Sub WriteEntry(tar As TarInputStream, entry As TarEntry, target As String)
        Directory.CreateDirectory(Path.GetDirectoryName(target))
        If File.Exists(target) Then
            Dim attributes = File.GetAttributes(target)
            If attributes.HasFlag(FileAttributes.ReadOnly) Then File.SetAttributes(target, attributes And Not FileAttributes.ReadOnly)
        End If

        Using output As New FileStream(target, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize)
            tar.CopyEntryContents(output)
        End Using

        Try
            File.SetLastWriteTimeUtc(target, Date.SpecifyKind(entry.ModTime, DateTimeKind.Utc))
        Catch ex As ArgumentOutOfRangeException
        End Try
    End Sub

    ''' <summary>Adds a trailing directory separator, so "C:\Forge2" is not mistaken for a child of "C:\Forge".</summary>
    Private Shared Function WithTrailingSeparator(folder As String) As String
        Return If(folder.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal), folder, folder & Path.DirectorySeparatorChar)
    End Function
End Class
