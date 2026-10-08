![image](https://github.com/churrufli/forgelauncher/assets/4362846/cb55056b-cef5-43d5-8d73-b9c538dc98ca)


**Introduction**

Forge Launcher help Forge users in Windows to keep it updated. No Minecraft related! https://card-forge.github.io/forge/

**Forge Launcher** is an .exe file that uses ICSharpCode.SharpZipLib.dll to unzip .tar.bz2 files. Place it in your main Forge folder near Forge.exe and run it. If Forge.exe does not exist, Launcher will do a fresh installation of Forge. If you already had Forge, when you start it for the first time it will not detect your version, as there is no programmatic way to get it for the first time. Select the update line you want (release or snapshot) and update your version of Forge. After that, every time you open Launcher, it will compare between your local version and the server version, and announce if there is a new version of Forge available.

**Tooltips**

Placing the mouse pointer on certain elements (in blue icons with a question mark too), information about element will appear. Read the tooltips in detail to understand how Forge Launcher works.

**Features**

**Install from Zero**
Forge Launcher will detect if Forge.exe exists in the same folder, if not, it will display the Install Forge option, along with the options Normal Install, and Install all in the same folder creating forge.profile.properties file. For more information, please read documentation about this in your forge.profile.properties.example.

**Update Options**

Use update button (arrows in circle icon) to check/install updates, selecting before type of update (release or snapshot). Use "enable prompt for a new version" will alert you when a new version is available in addition to the log).
Launch Mode (Normal or Advanced)
By clicking on the gear icon, you will find two options. Normal mode by default only call to Forge.exe to launch Forge. Advanced Mode create a bat.file (PlayForge.bat) with a custom launch for more RAM assignation. Click in blue icon with a question mark TO WRITE AN EXAMPLE.
