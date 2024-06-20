# Melman VR

This repository cover:
1. Melman VR App - for user interface with robot
2. Melman VR Backend - running on robot
3. MJPEG-Streamer - running on robot

## Melman VR App

Melman VR App was developed in Unity 2022.3.25f.

[Initial mode in VR app](https://github.com/tabiszs/melson-vr-controller/assets/92331225/253ba4fc-8e75-4eda-bd7c-de929059c179)

![Streaming mode in VR app](./image.png)

## Melman VR Backend
[Details how to run backend](./Backend/README.md)

[Virtual Melman T-pose](https://github.com/tabiszs/melson-vr-controller/assets/92331225/7680d4f9-b6d5-45c5-b501-b8b442c65219)

## MJPEG-Streamer
Melman record world by one camera with FoV XYZ
VR app will be displayed this video full screen.

> Inspiration of IP Camera in VR
- https://github.com/gpvigano/VidStreamComp 

### Stream video
1. Clone repository `git clone https://github.com/jacksonliam/mjpg-streamer`
2. Follow the building and instalation instuction
3. Set Robot IP address in Unity Video Component `VRScene > VideoStreamManager > IPCameraStream > URL` to `http://192.168.X.Y:8080/?action=stream`. It uses `Codes Motion JPEG Video (MJPG)`
4. Start stream on robot `./mjpg_streamer -i "input_uvc.so" -o "output_http.so -p 8080"`

### Unity constraints
On Linux you can use only specific video format. For more see: https://docs.unity3d.com/Manual/VideoSources-FileCompatibility.html. 

## Positions
Ones VRController has connection with robot, it will be send data from head and hands to change position of robot

## Communication
`Project Settings > Player > Configuration > Allow downloads over HTTP = always allowed (unsave)`

## Assets Source
- hands: https://drive.google.com/file/d/10b39IekUdpBHlcTslZ-BlNRyH5uqPUe1/view

## Useful Tutorials
- <[How to Make a VR Game in Unity - PART 1](https://www.youtube.com/watch?v=HhtTtvBF5bI&list=PLpEoiloH-4eP-OKItF8XNJ8y8e1asOJud&index=2&ab_channel=ValemTutorials)>
