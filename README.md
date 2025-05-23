Development Notes:

How to set up Android emulators (in as detailed steps as possible)

Set up Virtualization Technologies on your respective motherboard. The links depend on your type of motherboard but it is safe to assume that searching for something similar to "how to set up virtualization on ____ motherboard" will give a more accurate link to the steps for the firmware
Use the link https://learn.microsoft.com/en-us/xamarin/android/get-started/installation/android-emulator/hardware-acceleration?tabs=vswin&pivots=windows#hyper-v to check out what you need for your CPU more specifically. Intel requires a few pieces of software, but most importantly hyper-v will need to be enabled.
The link gives you the command to enable it, but if you are still having issues going to cmd and then entering the command "systeminfo" will tell you if you have hyper-v enabled. If it isn't, it will tell you what specifications you meet and which ones you don't. If you do meet the specifications and hyper-v was started correctly in step 2 using their commands, it will read "A hypervisor has been detected. Features required for Hyper-V will not be displayed" meaning you've set it up correctly.
How to use your Android device in the testing of the application:

Enable developer mode on your device (some devices vary, but for most of them you just go into settings, scroll down to about, find the build number, and then press it rapidly until it says you are a developer. You will also likely need to re-enter any password or pin tied to your phone.)
Back out of the About tab and below it should be a developer settings tab. Enter that and scroll down until you see USB debugging. Enable it.
If you re-open Visual Studio, you should be able to see your device (assuming it is plugged into your computer) on the build with the debugging button.
How to solve NuGet Error "Invalid Character on Line 1":

In Visual Studio 2019 or newer (this was tested in VS 2022), use the tab view and then select Terminal to open Visual Studio's built-in PowerShell.
Enter the command "dotnet nuget locals all --clear" and wait for its completion.
Finally, enter the command "dotnet restore --no-cache". This one should take longer to complete but afterward, any broken or non-working nuget packages should be fixed.
