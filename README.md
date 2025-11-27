1. Download https://builds.bepinex.dev/projects/bepinex_be/752/BepInEx-Unity.Mono-win-x64-6.0.0-be.752%2Bdd0655f.zip
2. Copy AlwaysBeNaked.dll to BepInEx\plugins
3. Copy BepInEx Folder to the UiTC root folder, where you can see the UiTC.exe
4. Done, now you can overall be naked

for devs:

too update the plugin add the Assembly-Csharp.dll in visual studio , you can find the assembly in UiTC_Data\Managed\Assembly-CSharp.dll

bin\Release\net40\AlwaysBeNaked.dll copy to BepInEx\plugins

bin\Debug\net40\AlwaysBeNaked.dll copy to BepInEx\plugins

also important is JetBrains DotPeek to open the assembly Assembly-Csharp.dll 

https://www.jetbrains.com/decompiler/download/?section=web-installer

you can find all important Class Objects like Global_Objects_UW and Character_Clothes_UW and LevelManager_UW in the Root Namespace

looks pictures

![howtoinstall](https://github.com/user-attachments/assets/59afb2f1-9f3b-4f9f-80c7-4c51b352df7b)
![howtoinstall](https://github.com/user-attachments/assets/89186126-9a14-4c00-9099-d115d615a7e8)
