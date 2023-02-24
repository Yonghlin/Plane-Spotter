# Plane Spotter App

## Setting up the project in Unity
Navigate to a folder where you want the project to be on your system, and run:
`git clone -b opening-app https://gitlab.engr.ship.edu/Burke/plane-spotter-app`
Navigate to `Plane Spotter/Assets/Scenes/` and double-click `BlankAR.unity`
Go to `File > Build Settings`
Click `Android`, then on the bottom right, `Switch Platform`
NOTE: You may see a dialogue telling you to install Android development tools. Click that.
When Unity Hub finishes installing them, restart Unity and then go back to the `Build Settings` menu.
Click `Add Open Scenes` toward the top right.
Click `Build`.

## Deploying and Testing on Android
On your Android device, go to `Settings > About Phone` and tap `Build number` 7 times.
Now go to `Settings > System > Developer options` and enable `USB debugging`.
Plug your phone in to your computer. You should get a popup on your phone to authorize your computer. Click allow.
In Unity's `Build Settings` window, click `Build And Run`. The first time will take several minutes or more. Let it finish. If you don't see the app open automatically on your Android device, click `Build And Run` once more.
