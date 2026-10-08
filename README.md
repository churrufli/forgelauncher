# Forge Launcher

Forge Launcher installs, updates and starts [Forge](https://card-forge.github.io/forge/) on Windows. Not related to Minecraft.

It is a small Windows program made of two files: `Forge Launcher.exe` and `ICSharpCode.SharpZipLib.dll`. Put both in your Forge folder (or in an empty folder for a new installation) and run the exe. Nothing else is needed: no installer and no extra DLLs.

## Requirements

- Windows 10 or 11, which already include .NET Framework 4.8 (Windows 10 from version 1903). Windows 7 SP1 and 8.1 also work once .NET Framework 4.8 is installed.
- Java 17 or later, which Forge itself needs.
- A folder you can write to. Avoid `C:\Program Files`, because updating would need administrator rights.

## The main window

The header shows the installed Forge version, when it was built, and its state:

| Dot | Meaning |
|---|---|
| Green | Up to date (or newer than the selected channel) |
| Amber | An update is available |
| Gray | Not checked yet, the check failed, or you are on a restored version |

Below the header:

- **Snapshot / Release** selects the update channel. Snapshots are daily development builds; releases are stable versions. Changing it checks that channel right away.
- **Launch list.** Choose which file "Launch Forge" starts: `forge.exe`, `forge-adventure.exe` or any `.cmd`/`.bat` in the folder. In custom Java mode it shows a summary of the Java command instead.
- **Settings (gear)** opens the Settings window.
- **Check for updates** looks for a newer build of the selected channel.
- **Launch Forge** starts Forge and closes the launcher. When Forge is not installed, the same button reads **Install Forge**, and a **Portable install** switch appears next to it.

The log shows everything the launcher does. The status bar at the bottom shows:

- download progress, with speed and time left;
- a **Cancel** button while downloading;
- a **Retry** button when an update check failed.

Download progress also appears on the launcher's taskbar button.

### Keyboard shortcuts

| Key | Action |
|---|---|
| Enter | Launch Forge (or Install Forge) |
| F5 | Check for updates |
| Ctrl+, | Open Settings |

### Menus

- **Forge**:
  - Open Forge folder
  - Open user data folder
  - Open forge.log
  - Restore a previous version
  - Reset Forge preferences: deletes `forge.preferences`, which fixes most cases of Forge freezing on the splash screen.
- **Settings** opens the Settings window.
- **Help**:
  - Report a problem
  - Forge website, Forge releases and Forge Discord
  - Forge Launcher on GitHub
  - Check for Forge Launcher updates
  - About

## Installing and updating Forge

1. The package (`forge-installer-<version>.tar.bz2`, about 300 MB) is downloaded to `fldata\downloads`. The download can be cancelled, and it is retried on request if it fails.
2. The package is extracted over the existing files.
3. Files that the previous version installed but the new one doesn't contain are deleted, together with folders left empty. The launcher knows which files those are from `fldata\installed-files.txt`, the list of files of the last installed package.
4. The new package's file list is saved.

The result is the same as deleting the old version first and installing the new one, but without the risk of losing Forge if something fails half way. Files that never came from a Forge package are never deleted. That includes:

- the launcher and its `fldata` folder;
- `forge.profile.properties`;
- the portable `user` and `cache` folders;
- anything you added yourself.

The first update of a Forge installation the launcher didn't install has no file list yet. In that case only old Forge jars, which are recognizable by name, are removed; from then on the cleanup is complete.

The launcher refuses to install while Forge is running, because Windows keeps its files locked.

**Portable install** (fresh installations only) creates `forge.profile.properties` so Forge keeps its settings, decks and card images inside the Forge folder:

```
userDir=./user/
cacheDir=./cache/
```

An existing `forge.profile.properties` is never overwritten.

## How update detection works

Every Forge package contains a `build.txt` file with the build time, for example `2026-10-06 18:28:55`. Forge's build process publishes exactly the same file on GitHub, so comparing the two tells precisely whether you have the latest build.

The launcher reads the installed build time from `build.txt` in the Forge folder. For older installations without that file, it reads the copy stored inside the newest `forge-gui-desktop-*-jar-with-dependencies.jar`. The installed version is detected even the first time the launcher runs.

| Channel | Where the launcher looks |
|---|---|
| Snapshot | `version.txt` and `build.txt` of the `daily-snapshots` release |
| Release | `/releases/latest` on GitHub redirects to the newest tag (for example `forge-2.0.15`); that release's `build.txt` |

Build times decide whenever both are known. Otherwise numeric versions are compared, and a release counts as newer than the snapshots that preceded it.

The "What's new" link in the update dialog opens either:

- the release notes, for releases;
- the list of Forge changes since your installed build, for snapshots.

## Previous versions (rollback)

In **Settings > Previous versions** choose how many previous Forge versions to keep: **None** (default), **1** or **2**.

- **How versions are kept.** When this is on, every package you install stays in `fldata\packages`. Next to it goes a small `.info` file with its channel, version and build time. The package of the installed version plus the chosen number of others are kept; older ones are deleted automatically.
- **Disk space.** Each version takes about 300 MB, so 2 previous versions use up to about 900 MB.
- **Why it matters for snapshots.** GitHub only publishes the latest snapshot, so a saved package is the only way back to an older one.

**Forge > Restore a previous version** (also in Settings) lists the saved versions with their channel and build time. The installed one is marked and can't be chosen. Restoring:

- uses the same installation process as an update, so files that only exist in the version you leave are removed;
- keeps your decks, preferences and card images;
- works offline, because it uses the saved package.

**Don't offer `<version>` again, only newer builds** is on by default. With it on, the launcher doesn't offer the version you just left at the next start:

- The header shows "Restored version · `<version>` skipped".
- As soon as a newer build is published, it is offered as usual, and installing it clears the skip.
- **Check for updates** always offers the latest build, mentioning that you skipped it.

## Settings

| Section | Options |
|---|---|
| Updates | Check for Forge updates on startup; check for Forge Launcher updates on startup; offer to start Forge when it is up to date |
| Previous versions | How many versions to keep; disk space in use; Restore |
| Launch | Normal mode, or a custom Java command with application (Forge or Forge Adventure), Java executable, maximum memory and JVM arguments |
| Maintenance | Open `fldata`; open `launcher.log`; uninstall Forge Launcher |

Nothing changes until you click **Save**.

### Offer to start Forge when it is up to date

When Forge is up to date at startup:

- **On** (default): a dialog offers to start Forge and close the launcher.
- **Off**: no dialog appears; the **Launch Forge** button has the focus, so pressing Enter starts Forge.

### Custom Java command

Instead of starting `forge.exe`, the launcher runs:

```
<java> -Xmx<memory>m <JVM arguments> -jar <newest Forge jar>
```

Because the newest jar is always used, the command keeps working after updates. The default JVM arguments are the module access flags Forge needs on Java 17 and later. This replaces the `PlayForge.bat` file of the original launcher.

## Files and folders

Everything the launcher creates lives in `fldata` inside the Forge folder:

```
Forge/
├── forge.exe, res/, *.jar ...           Forge (installed from its packages)
├── Forge Launcher.exe                   the launcher
├── ICSharpCode.SharpZipLib.dll          the launcher's compression library
├── forge.profile.properties             only for portable installs (read by Forge)
└── fldata/
    ├── settings.ini                     launcher settings
    ├── launcher.log                     log of every session
    ├── installed-files.txt              files of the installed Forge package
    ├── downloads/                       packages being downloaded (temporary)
    ├── packages/                        saved versions for rollback, each with an .info file
    └── update/                          used while the launcher updates itself (temporary)
```

`launcher.log` is trimmed automatically when it grows beyond 1 MB.

Settings are migrated automatically on first start:

- from Forge Launcher 3.0.0 (`forgelauncher.ini` in the Forge folder);
- from the original launcher (`fldata\version.txt`): the update channel and the selected launch file.

### settings.ini keys

| Key | Values | Default |
|---|---|---|
| `Channel` | `Snapshot`, `Release` | `Snapshot` |
| `CheckOnStartup` | `True`, `False` | `True` |
| `CheckLauncherUpdates` | `True`, `False` | `True` |
| `AskToLaunchWhenUpToDate` | `True`, `False` | `True` |
| `KeepPreviousVersions` | `0`, `1`, `2` | `0` |
| `LaunchMode` | `Normal`, `CustomJava` | `Normal` |
| `LaunchTarget` | File name in the Forge folder | `forge.exe` |
| `CustomApp` | `Desktop`, `Adventure` | `Desktop` |
| `JavaPath` | `javaw`, `java` or a full path | `javaw` |
| `MaxMemoryMB` | Megabytes | `4096` |
| `JvmArguments` | Arguments on one line | Forge's `--add-opens` list plus `-Dio.netty.tryReflectionSetAccessible=true -Dfile.encoding=UTF-8` |
| `SkippedBuild` | Build time of the build skipped after a rollback | empty |
| `SkippedVersion` | Version text of that build | empty |

## Reporting a problem

**Help > Report a problem** copies a report to the clipboard and offers to open the Forge Discord. The report contains:

- the versions of Forge Launcher, Windows, .NET Framework, Java and Forge;
- the main settings;
- the last 200 lines of `launcher.log`.

Paste it with a short description of what happened.

## Forge Launcher updates

When **Check for Forge Launcher updates on startup** is on (or with **Help > Check for Forge Launcher updates**), the launcher looks for a newer release on `https://github.com/churrufli/forgelauncher`. If there is one, it offers to update. The update:

1. downloads the release package;
2. moves the running exe and DLL to `fldata\update` (Windows allows renaming a running program, but not overwriting it);
3. puts the new files in place, starts the new version and exits;
4. if anything fails half way, puts the original files back, so the current version keeps working.

The new version waits for the old one to exit, then deletes the leftovers.

While the repository is private, GitHub answers "not found" to the launcher's anonymous requests. The startup check then finds nothing and stays silent. It starts working, without any code change, as soon as the repository is public.

### Publishing a new launcher version

1. Increase the version in `src/ForgeLauncher/AssemblyInfo.vb` (`AssemblyVersion`, `AssemblyFileVersion` and `AssemblyInformationalVersion`, for example `3.2.0`) and build the Release configuration.
2. Create a zip named `ForgeLauncher-3.2.0-win.zip` that contains `Forge Launcher.exe` and `ICSharpCode.SharpZipLib.dll`. They can be at the root of the zip or inside one folder.
3. On GitHub, create a release with the tag `v3.2.0` and attach the zip.

The tag (without the `v`) and the zip name must match the version. Launchers compare the tag with their own version and only offer newer ones.

## Uninstalling

**Settings > Maintenance > Uninstall Forge Launcher** removes `Forge Launcher.exe`, `ICSharpCode.SharpZipLib.dll` and the `fldata` folder, including saved Forge versions; the confirmation shows their size.

Forge, your decks, preferences and card images, and `forge.profile.properties` are not touched.

Because a running program can't delete itself, the launcher starts a small hidden script and exits. The script waits until the launcher has closed, deletes the files and then deletes itself.

## Appearance

The interface uses a dark theme with white text, Segoe UI, and the green of the launcher icon as accent color.

- **Windows 10 (20H1 and later) and Windows 11:** the title bars, scroll bars and drop-down lists are dark too.
- **Button icons:** they come from the icon font included in Windows, Segoe Fluent Icons on Windows 11 and Segoe MDL2 Assets on Windows 10, so no image files are needed. On Windows 7 and 8.1 buttons show text only.
- **Dialogs:** all questions and messages use the launcher's own dark dialog, because the standard Windows message box is always light.

## Building from source

The project targets .NET Framework 4.8 and uses the classic VB.NET project format, which the Windows Forms designer supports best.

Requirements:

- **Visual Studio 2022** with the ".NET desktop development" workload.
- **The .NET Framework 4.8 targeting pack.** In the Visual Studio Installer choose Modify, open Individual components and tick ".NET Framework 4.8 targeting pack". Alternatively, install the .NET Framework 4.8 Developer Pack from Microsoft.

  Check that `C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8` exists. Without it, neither the build nor the forms designer can find .NET Framework ("Could not resolve mscorlib for target framework .NETFramework,Version=v4.8").

Open `Forge Launcher.sln`, choose the Release configuration and build. The output folder `src/ForgeLauncher/bin/Release/` contains only `Forge Launcher.exe` and `ICSharpCode.SharpZipLib.dll`. The application icon is embedded in the exe.

The windows use custom controls (dark buttons, switches, selectors), so build the project once before opening a form in the designer. To see a form's code without the designer, right-click the file and choose View Code (F7).

From the command line, a "Developer Command Prompt for VS 2022" can build it with `msbuild "Forge Launcher.sln" /p:Configuration=Release`.

### Project layout

| File | Purpose |
|---|---|
| `AssemblyInfo.vb` | Version, title and description of the executable (change the version here when publishing) |
| `Program.vb` | Entry point: DLL check, waiting after a self-update, one launcher per folder (bringing the existing window to the front) |
| `MainForm.vb` | Main window: header, update checks, installation, rollback, self-update, report, launching |
| `SettingsForm.vb` | Settings window |
| `RestoreForm.vb` | "Restore a previous version" window |
| `LauncherPaths.vb` | Every file and folder the launcher creates |
| `LauncherSettings.vb` | `settings.ini` and migration from older launchers |
| `LauncherLog.vb` | `launcher.log` |
| `ForgeInstallation.vb` | Local installation: build time, version, jars, user folder, launch files |
| `ForgeServer.vb` | GitHub access: latest Forge builds, latest release tags, downloads |
| `UpdateChecker.vb` | Compares the installed build with the latest one |
| `PackageInstaller.vb` | Safe `.tar.bz2` extraction and cleanup of files from the previous package |
| `PackageStore.vb` | Saved versions for rollback |
| `SelfUpdater.vb` | Forge Launcher updates |
| `ForgeRunner.vb` | Starting Forge |
| `Uninstaller.vb` | Removing Forge Launcher |
| `SystemInfo.vb` | Problem report |
| `FriendlyErrors.vb` | User-friendly error messages and retry decisions |
| `Ui/Theme.vb` | Colors, fonts, icon glyphs, dark title bar and scroll bars |
| `Ui/DarkControls.vb` | Custom-drawn buttons, switches, selector, progress bar, status dot, panels and menus |
| `Ui/DarkDialog.vb` | Dark dialog used instead of message boxes |
| `Ui/TaskbarProgress.vb` | Progress on the taskbar button |

All code is documented with XML comments.

## Third-party components

`lib/ICSharpCode.SharpZipLib.dll` is [SharpZipLib](https://github.com/icsharpcode/SharpZipLib) 1.3.3, `net45` build, under the MIT license. It reads the `.tar.bz2` packages, which .NET Framework can't open on its own.

The 1.3.3 `net45` build is used because it has no dependencies. SharpZipLib 1.4 only ships a `netstandard2.0` build for .NET Framework, which would need several more DLLs.

## Troubleshooting

| Problem | Solution |
|---|---|
| "ICSharpCode.SharpZipLib.dll must be in the same folder" | Copy the DLL next to `Forge Launcher.exe`. |
| "Forge is running" | Close Forge and Forge Adventure before updating or restoring. |
| "Couldn't reach GitHub" | Check the internet connection, then click Retry. |
| "Not enough free disk space" | An update needs about 750 MB free during installation. |
| "Java couldn't be started" (custom mode) | Set the full path to `javaw.exe` in Settings > Launch. |
| An installation was interrupted | Click Check for updates and choose Reinstall. |
| A new Forge version misbehaves | Use Restore a previous version (requires Settings > Previous versions on 1 or 2 before updating). |
| Forge freezes on the splash screen | Use Forge > Reset Forge preferences. |
