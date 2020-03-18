




# Ping Pong in Immersive Virtual Reality

## Introduction
This repository contains the source code to build a ping pong server and two clients.
Two users with two HMDs will be able to play ping-pong in VR.

## Architecture

A very specific aspect of this project is that it allows for TCP and UDP communication, 
and most of the physics and inverse kinematics is done on a central server, instead of in the clients.

## Compilation

The simplest way to get started is to open the project and:
0. Open the project and import the SteamVR plugin (we tested up to version 1.2.10)
1. Open the scene `ServerScene.unity` in folder `Assets\Scenes`
2. Build the project only with this scene. It can be a build for Linux (where you can use headless mode), or for windows.
3. In the main folder of the server build, add the file `ServerConfig.cfg` found in the folder `ping-pong-simpler\Server`, change the port if necessary
4. Open the scene `ClientSceneVRUpperBody.unity` in folder `Assets\Scenes`
5. Do a different build of the project only with this scene (in this case, for windows).
6. In the main folder of the client build, add the file ClientConfig.cfg, as found in the folder `ping-pong-simpler\Client`. Make sure you change the IP to the one of the server. Change also the port if necessary
7. Execute first the server, and then the client. If you are on a network managed by a University or a large institution, it is possible that the server and the client may need to be on different computers

## Credits

This project is a simplification of two original projects developed in Autumn 2017 and Winter 2018 by Enric Moreu and Alexandre Via 
as their final degree projects.


Alex Via's project developed a server/client architecture to manage the physics, the inverse kinematics and the communication between the two participants
Enric Moreu's project included a custom hardware component, where he developed a custom controller in the form of a ping pong paddle.
The outcome of this work was in a repository that was significantly bigger, it can be found [here](https://github.com/joanllobera/ping-pong/tree/9cc332536c2dd94f4f0cca1db427304f96764126): 
9cc332536c2dd94f4f0cca1db427304f96764126


The following images link to two videos of the original projects:

[![](http://img.youtube.com/vi/judXWQkDd5E/0.jpg)](http://www.youtube.com/watch?v=judXWQkDd5E "ping pong with IK")
[![](http://img.youtube.com/vi/QxPiP0HnYJk/0.jpg)](http://www.youtube.com/watch?v=QxPiP0HnYJk "ping pong HDK")

Further information:
For general information about the project, contact the manager of this repository.

The original developers can be found at:

Enric Moreu:	enric {dot} moreu {dot} filella {at} gmail {dot} com

Alex Via:  alexviacoll {at} gmail {dot} com
