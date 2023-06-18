# Github Repository Template for Valheim modding

Create your own mod using this template as a starting point for your mod.

## Template usage

For using this template, either

* use the github template function
* clone this repo and use the included dotnet template
    * run `dotnet new --install .` --> this will install this template into your dotnet local installation
    * creating a new solution then becomes also simple, either
        * run `dotnet new mt -o "myNewProject" -A "YourAuthorName"`
        * create a new solution choosing this template from your IDE
    * see also [dotnet docs](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates)

## Requires

* To have [BepInEx](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/) installed (unzip) to your
  Valheim path like
  ```
  C:\Program Files (x86)\Steam\steamapps\common\Valheim\BepInEx
  ```
* To have assemblies publicized via [AssemblyPublicizer](https://github.com/CabbageCrow/AssemblyPublicizer)
    * download and unzip, put the `AssemblyPublicizer.exe` into your managed lib
      folder `...\Valheim\valheim_Data\Managed`
    * Drag and drop the 3 .dlls onto the .exe, it will create the `publicized_assemblies` sub folder and put the
      publicized .dlls there
    * The .csproj does include the publicised .dlls already
* If not using the dotnet templating feature, you will need to rename the solution and .csproj, same as namespace and
  classes and files to your own names

## Features

* Has [ServerSync](https://github.com/blaxxun-boop/ServerSync) built in
* Has debug build feature that puts the dll into BepInEx installed inside Valheim
* Has run the game feature for running your debug version locally
* Has release build feature to package everything ready to upload to ThunderStore as zip file in your local user home
  Downloads folder
    * Prepared contents inside ThunderStorePackage folder
    * replace the icon.png with any other 256x256 pixel png file
    * edit the manifest.json file
    * remove or replace the empty files inside config and plugins folders
    * it will put the README.md (this content) and CHANGELOG.md from root folder also into the zip

# Credits

* I did follow some hints by [AzumattDev](https://github.com/AzumattDev) from his
    * [YouTube session on how to create a template](https://www.youtube.com/watch?v=gSL31r2AgrI).
    * [YouTube session on how to use one of his templates](https://www.youtube.com/watch?v=ws7Lq8tRWlI)
* `blaxxun#9098` for being awesome and providing features like ServerSync to the public
* `margmas` for hints on publicising
* Having had a look at some people's setups for .csproj
