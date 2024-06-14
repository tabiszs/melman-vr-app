# melson-vr-controller
VR app for user interface with robot


## Tutorials
- <[How to Make a VR Game in Unity - PART 1](https://www.youtube.com/watch?v=HhtTtvBF5bI&list=PLpEoiloH-4eP-OKItF8XNJ8y8e1asOJud&index=2&ab_channel=ValemTutorials)>
- <[How to Play VIDEO in Unity - Easy Tutorial (2023)](https://www.youtube.com/watch?v=-XzVq7qIuys&ab_channel=SoloGameDev)>

## Camera
Melman record world by one camera with FoV XYZ
VRController will be displayed this video full screen.
When person turn his head it will be seen on screen. Immersive??

!!! UnAvailable now: https://forum.unity.com/threads/streaming-live-video-from-desktop-over-web-into-unity-vnc-obs-ip-camera-mjpeg.1313616/
> Use RTC instead:
- https://webrtc.github.io/samples/
- https://webrtc.org/getting-started/peer-connections

> Inspirated by:
- https://github.com/gpvigano/VidStreamComp 


### Stream video
1. Clone repository `git clone https://github.com/jacksonliam/mjpg-streamer`
2. Follow the building and instalation instuction
3. Set Robot IP address in Unity Video Component `VRScene > Video > VideoPlayer > VideoPlayer > URL` to `http://192.168.X.Y:8080/?action=stream`
4. Start stream on robot `./mjpg_streamer -i "input_uvc.so" -o "output_http.so -p 8080"`

### Unity constraints
On Linux you can use only specific video format. For more see: https://docs.unity3d.com/Manual/VideoSources-FileCompatibility.html. 


## Positions
Ones VRController has connection with robot, it will be send data from head and hands to change position of robot

## Communication
`Project Settings > Player > Configuration > Allow downloads over HTTP = always allowed (unsave)`

## Screens
1. Wait for connection
2. Sync with robot. Dialog with instruction how to stay in T pose and which button turn to sync coordinate system with robot.
3. Play button
4. Video

## Communication schema
1. VR app send post request to broker with own IP address
2. Robot send post request to broker with own IP address
3. After in every 1 second VR app send get request to get robot IP address
4. After in every 1 second robot send get request to get VR IP address
5. After enable trigger from T-pose VR app send activat=true request for video. 
6. VR app start recieving streaming
7. VR app in every frame proccess VR position HMD, left and right hand.

## Assets
- hands: https://drive.google.com/file/d/10b39IekUdpBHlcTslZ-BlNRyH5uqPUe1/view