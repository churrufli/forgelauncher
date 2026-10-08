Imports System
Imports System.IO

''' <summary>
''' Central place that defines every file and folder Forge Launcher creates.
''' Everything the launcher writes lives in the <c>fldata</c> folder inside the Forge folder,
''' so the Forge folder itself only contains Forge's own files plus the launcher executable and its DLL.
''' </summary>
Public NotInheritable Class LauncherPaths
    ''' <summary>Name of the launcher's data folder, created next to Forge.exe.</summary>
    Public Const DataFolderName As String = "fldata"

    ''' <summary>File name of the compression library that must sit next to the launcher executable.</summary>
    Public Const ZipLibraryFileName As String = "ICSharpCode.SharpZipLib.dll"

    ''' <summary>Creates the path set for a Forge folder.</summary>
    ''' <param name="forgeFolder">Folder that contains (or will contain) Forge and the launcher.</param>
    ''' <param name="launcherExe">Full path of the running launcher executable.</param>
    Public Sub New(forgeFolder As String, launcherExe As String)
        Me.ForgeFolder = forgeFolder
        Me.LauncherExe = launcherExe
        DataFolder = Path.Combine(forgeFolder, DataFolderName)
    End Sub

    ''' <summary>The Forge folder (also the launcher's own folder).</summary>
    Public ReadOnly Property ForgeFolder As String

    ''' <summary>Full path of the running launcher executable.</summary>
    Public ReadOnly Property LauncherExe As String

    ''' <summary>The <c>fldata</c> folder that holds everything the launcher generates.</summary>
    Public ReadOnly Property DataFolder As String

    ''' <summary>Launcher settings (<c>fldata\settings.ini</c>).</summary>
    Public ReadOnly Property SettingsFile As String
        Get
            Return Path.Combine(DataFolder, "settings.ini")
        End Get
    End Property

    ''' <summary>Persistent session log that users can send when reporting a problem (<c>fldata\launcher.log</c>).</summary>
    Public ReadOnly Property LogFile As String
        Get
            Return Path.Combine(DataFolder, "launcher.log")
        End Get
    End Property

    ''' <summary>
    ''' List of every file installed by the last Forge package (<c>fldata\installed-files.txt</c>).
    ''' It is used to remove files that a newer or older package no longer contains.
    ''' </summary>
    Public ReadOnly Property ManifestFile As String
        Get
            Return Path.Combine(DataFolder, "installed-files.txt")
        End Get
    End Property

    ''' <summary>Temporary folder for Forge packages that are being downloaded (<c>fldata\downloads</c>).</summary>
    Public ReadOnly Property DownloadsFolder As String
        Get
            Return Path.Combine(DataFolder, "downloads")
        End Get
    End Property

    ''' <summary>Forge packages kept for rollback, each with an <c>.info</c> file (<c>fldata\packages</c>).</summary>
    Public ReadOnly Property PackagesFolder As String
        Get
            Return Path.Combine(DataFolder, "packages")
        End Get
    End Property

    ''' <summary>Working folder used while the launcher updates itself (<c>fldata\update</c>).</summary>
    Public ReadOnly Property UpdateFolder As String
        Get
            Return Path.Combine(DataFolder, "update")
        End Get
    End Property

    ''' <summary>Full path of the compression library next to the launcher executable.</summary>
    Public ReadOnly Property ZipLibrary As String
        Get
            Return Path.Combine(Path.GetDirectoryName(LauncherExe), ZipLibraryFileName)
        End Get
    End Property

    ''' <summary>
    ''' Settings file written by Forge Launcher 3.0.0 in the Forge folder root.
    ''' It is migrated to <see cref="SettingsFile"/> on first start.
    ''' </summary>
    Public ReadOnly Property LegacyRootSettingsFile As String
        Get
            Return Path.Combine(ForgeFolder, "forgelauncher.ini")
        End Get
    End Property

    ''' <summary>
    ''' Settings file written by the original Forge Launcher (2.x) in <c>fldata\version.txt</c>.
    ''' Its update channel and selected launch file are imported on first start.
    ''' </summary>
    Public ReadOnly Property LegacyVersionFile As String
        Get
            Return Path.Combine(DataFolder, "version.txt")
        End Get
    End Property

    ''' <summary>Creates a folder if it does not exist yet and returns its path.</summary>
    ''' <param name="folder">Folder to create.</param>
    Public Shared Function Ensure(folder As String) As String
        Directory.CreateDirectory(folder)
        Return folder
    End Function

    ''' <summary>
    ''' Returns <c>True</c> when a path relative to the Forge folder belongs to the launcher itself
    ''' (its data folder, executable or DLL) or to the user's Forge profile file.
    ''' Such paths are never deleted when cleaning up files from a Forge package.
    ''' </summary>
    ''' <param name="relativePath">Path relative to the Forge folder, using the system separator.</param>
    Public Function IsProtected(relativePath As String) As Boolean
        Dim firstSegment = relativePath.Split(Path.DirectorySeparatorChar)(0)
        Return String.Equals(firstSegment, DataFolderName, StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(relativePath, Path.GetFileName(LauncherExe), StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(relativePath, ZipLibraryFileName, StringComparison.OrdinalIgnoreCase) OrElse
               String.Equals(relativePath, ForgeInstallation.ProfileFileName, StringComparison.OrdinalIgnoreCase)
    End Function
End Class
