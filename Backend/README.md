# Melman VR Backend

Part of Melman VR project to move robot using VR controllers. This part should be run on robot OS [Ubuntu 22.04](https://releases.ubuntu.com/jammy/) with [ROS2 Humble](https://docs.ros.org/en/humble/index.html) framework. Commands 1-14 include ROS2 Humble installation

## Enable Virtual Melman

To run virtual Melman robot download [knr_ros2_humanoid_virtual_twin](https://github.com/KNR-PW/knr_ros2_humanoid_virtual_twin) repository and execute following commands in terminal
``` bash
1  locale  # check for UTF-8
2  sudo apt update && sudo apt install locales
3  sudo locale-gen en_US en_US.UTF-8
4  sudo update-locale LC_ALL=en_US.UTF-8 LANG=en_US.UTF-8
5  export LANG=en_US.UTF-8
6  locale  # verify settings
7  sudo apt install software-properties-common
8  sudo add-apt-repository universe
9  sudo apt update && sudo apt install curl -y
10  sudo curl -sSL https://raw.githubusercontent.com/ros/rosdistro/master/ros.key -o /usr/share/keyrings/ros-archive-keyring.gpg
11  echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/ros-archive-keyring.gpg] http://packages.ros.org/ros2/ubuntu $(. /etc/os-release && echo $UBUNTU_CODENAME) main" | sudo tee /etc/apt/sources.list.d/ros2.list > /dev/null
12  sudo apt update
13  sudo apt upgrade
14  sudo apt install ros-humble-desktop
17  source /opt/ros/humble/setup.bash
19  sudo apt install git
20  mkdir Humanoid_workspace
21  cd Humanoid_workspace/
22  mv knr_ros2_humanoid_virtual_twin/ src/
23  git clone https://github.com/KNR-PW/knr_ros2_humanoid_virtual_twin.git
24  sudo apt install ros-humble-ros2-control ros-humble-ros2-controllers
25  sudo apt install python3-colcon-common-extensions
26  sudo apt install python3-rosdep
27  sudo apt install ros-humble-gazebo-ros-pkgs
28  sudo rosdep init
29  rosdep update
30  rosdep install -i --from-path src --rosdistro humble -y
32  source /opt/ros/humble/setup.bash
33  colcon build --packages-up-to ros2_humanoid_virtual_twin
34  source /opt/ros/humble/setup.bash
35  source install/local_setup.bash
36  ros2 launch ros2_humanoid_virtual_twin rviz_launch.py 
37  ros2 launch ros2_humanoid_virtual_twin gazebo_Melman_ros2control.launch.py
```

## How to Run the Server

1. Make sure you have Python installed on your machine.
2. Install the necessary packages with the following command:

    ```bash
    pip install flask pydantic
    ```

3. Save the script to a file, for example `main.py`.
4. Run the server with the command:

    ```bash
    python3 main.py
    ```

The server will be running on `http://X.Y.X.W:5000` and will accept POST requests at the `/frames` endpoint with a JSON body matching the `FrameDto` schema.

## Example JSON Payload

This payload is posted from VR headset.

Here's an example of what the JSON payload should look like when sending a POST request to the `/frames` endpoint:

```json
{
    "hmd": {
        "position": {"x": 1.0, "y": 2.0, "z": 3.0},
        "rotation": {"x": 0.0, "y": 0.0, "z": 0.0}
    },
    "leftHand": {
        "position": {"x": 4.0, "y": 5.0, "z": 6.0},
        "rotation": {"x": 3.14, "y": 0.0, "z": 0.0}
    },
    "rightHand": {
        "position": {"x": 7.0, "y": 8.0, "z": 9.0},
        "rotation": {"x": 0.0, "y": 0.0, "z": 0.0}
    },
    "forward": {"x": 0.0, "y": 0.0, "z": 1.0},
    "time": 123.456
}
```

You can use a tool like `curl` or Postman to send this payload to the server:

1. T-pose
```bash
curl -X POST http://127.0.0.1:5000/frames -H "Content-Type: application/json" -d '{"hmd":{"position":{"x":0.07,"y":1.29,"z":0.07},"rotation":{"x":0.0,"y":0.0,"z":0.0}},"leftHand":{"position":{"x":-0.73,"y":1.15,"z":-0.10},"rotation":{"x":0.0,"y":0.0,"z":0.0}},"rightHand":{"position":{"x":0.84,"y":1.08,"z":0.53},"rotation":{"x":0.0,"y":0.0,"z":0.0}},"forward": {"x": 0.0, "y": 0.0, "z": 1.0},"time":1.00}'
```

2. I-pose
```bash
curl -X POST http://127.0.0.1:5000/frames -H "Content-Type: application/json" -d '{"hmd":{"position":{"x":0.07,"y":1.29,"z":0.07},"rotation":{"x":0.0,"y":0.0,"z":0.0}},"leftHand":{"position":{"x":-0.43,"y":0.5,"z":-0.5},"rotation":{"x":0.0,"y":0.0,"z":0.0}},"rightHand":{"position":{"x":0.25,"y":0.5,"z":0.15},"rotation":{"x":0.0,"y":0.0,"z":0.0}},"forward": {"x": 0.0, "y": 0.0, "z": 1.0},"time":1.00}'
```

## TODO
1. check the coordinate system: I used Y upwards
2. determine which servo names correspond to head movements. 
3. assign angles and run actions extended by head movements
2. test in real life on the robot
3. possibly speed up the frequency of sending actions. Currently set to 0.2x/second