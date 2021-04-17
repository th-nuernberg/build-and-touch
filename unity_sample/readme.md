# Example VR Application
![Build and Touch in Action](../img/sample-application.png)

This folder contains an simple example that combines the DIY data gloves and the media optical tracking into a unity demo.

## Running the Project

Notice: While the hand tracking works in Unity on Linux, OSX and Windows, we provide for now a bluetooth implementation for Windows only. 

1. Follow the instructions to setup the optical tracking
2. Open the folder `build-and-touch` with Unity version `2020.4.x` or later.
3. Import the required dependencies (see next section).
4. (Optional) Enable the data gloves (Windows)
    1. Connect the datagloves with Windows 10
    2. Open "Bluetooth & other devices" in the settings
    3. Click on "More Bluetooth options" on the right side
    4. Select the tab "COM Ports"
    5. Copy the incoming com port for the right hand controller 
    6. Select the "Manager" GameObject in the unity hierarchy
    7. Paste the value into the `Right Hand Com Port` field in the inspector (For serial ports greater than 9 `\\.\` needs to be added in front of the serial port. See https://support.microsoft.com/en-us/topic/howto-specify-serial-ports-larger-than-com9-db9078a5-b7b6-bf00-240f-f749ebfd913e)
    8. Repeat steps 5-7 for the left hand controller
5. Enter Unity play mode.
6. Start the optical tracking python program. The hands now should be shown in Unity.

## Required Dependencies
The project requires the following dependencies to successfully compile, these files need to be manually added to the unity assets folder (for now):

- Google.Protobuf.dll - Select the current 3.x C# release https://github.com/protocolbuffers/protobuf/releases
- System.Buffers.dll - https://www.nuget.org/packages/System.Buffers/
- System.Memory.dll - https://www.nuget.org/packages/System.Memory/
- System.Runtime.CompilerServices.Unsafe.dll - https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe/