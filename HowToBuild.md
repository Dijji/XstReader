Two things are required to build the project: the source code and the tool to build it with.

- The tool can be obtained by downloading and installing VisualStudio Community from [visualstudio.microsoft.com](https://visualstudio.microsoft.com/). Just do a default installation and you will be guided to install other components if required when compiling a project.

- Start Visual Studio and `Clone or checkout code` from [github.com/Dijji/XstReader](<https://github.com/Dijji/XstReader>). Alternatively, the source code can be obtained from [github.com/Dijji/XstReader](<https://github.com/Dijji/XstReader>) using the 'Clone or Download' button to download the zip file, and unpacking the whole XstReader file structure into a working area on your PC then start Visual Studio and `Open a local folder`.

Once installed, double click XstReader.sln in the root folder of the source code to open the project in Visual Studio.

At this point, you should be able to build the solution (press `Ctrl+Shift+B`), and run it from within Visual Studio (press `F5`). Use the Build, Configuration Manager to change the Configuration from Debug to Release and the  binary installation files will be created in the `bin\Release` folder.
