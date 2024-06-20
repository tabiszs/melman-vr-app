from flask import Flask, request, jsonify
from pydantic import BaseModel
from typing import Optional
from threading import Thread
from dataclasses import dataclass
import numpy as np
import subprocess
import time
import math
import json

# Define constants
PI = math.pi
PI_OVER_2 = math.pi / 2
head_to_shoulder = 0.2
forwardDistanceBetweenHandsAndHead = 0.1
headHeight = 0.2
tolerance = 0.1
up_vector = np.array([0.0, 1.0, 0.0])

app = Flask(__name__)

# Dto definitions

class Vector3Dto(BaseModel):
    x: float
    y: float
    z: float

class DeviceDto(BaseModel):
    position: Vector3Dto
    rotation: Vector3Dto

class FrameDto(BaseModel):
    forward: Vector3Dto
    hmd: DeviceDto
    leftHand: DeviceDto
    rightHand: DeviceDto
    time: float

# Server variable
start_tracking = False
arm_length = 0.5
right_vector = np.array([0.0, 0.0, 0.0])
neck_position = np.array([0.0, 0.0, 0.0])
left_shoulder_position = np.array([0.0, 0.0, 0.0])
right_shoulder_position = np.array([0.0, 0.0, 0.0])
ra_y = 0
ra_x = 0
la_y = 0
la_x = 0
head_yaw = 0
head_pitch = 0

@app.route('/start-tracking', methods=['POST'])
def start_tracing():
    global start_tracking, arm_length, left_shoulder_position, right_shoulder_position
    try:
        data = request.json
        frame_dto = FrameDto(**data)
        
        # Set arm length
        get_shoulders_position(frame_dto)
        left_hand_position = np.array([frame_dto.leftHand.position.x, frame_dto.leftHand.position.y, frame_dto.leftHand.position.z])
        lal = np.abs(left_hand_position, left_shoulder_position)
        right_hand_position = np.array([frame_dto.rightHand.position.x, frame_dto.rightHand.position.y, frame_dto.rightHand.position.z])
        ral = np.abs(right_hand_position - right_shoulder_position)
        arm_length = np.mean(lal, ral)

        print(frame_dto)
        start_tracking = True;
        return jsonify({"message": "Started tracing"}), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 400

@app.route('/frames', methods=['POST'])
def accept_frame():
    global ra_y, ra_x, la_y, la_x
    try:
        data = request.json
        frame_dto = FrameDto(**data)        
        inverse_kinematics(frame_dto)
        dest_time = 10  # Destination time in seconds      

        # Start ROS2 command in a separate thread
        ros2_thread = Thread(target=run_ros2_command, args=(ra_y, ra_x, la_y, la_x, dest_time))
        ros2_thread.start()

        return jsonify({"message": "Frame received successfully"}), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 400
    
def get_neck_position(frame_dto):
    headPosition = np.array([frame_dto.hmd.position.x, frame_dto.hmd.position.y, frame_dto.hmd.position.z])
    forwardVector = np.array([frame_dto.forward.x, frame_dto.forward.y, frame_dto.forward.z])
    neck_position = headPosition - forwardVector * forwardDistanceBetweenHandsAndHead
    neck_position[1] -= headHeight
    return neck_position

def get_shoulders_position(frame_dto):
    global neck_position, right_vector, right_shoulder_position, left_shoulder_position
    neck_position = get_neck_position(frame_dto)
    #print("neck_position: " + str(neck_position))
    forward = np.array([frame_dto.forward.x, frame_dto.forward.y, frame_dto.forward.z])
    # TODO: sepecify proper coordinate system axis, 
    # TODO: revert/change ra_x, ra_y, la_x, la_y to adjust to coordinate system
    right_vector = +1 * np.cross(forward, up_vector) 
    #print("right_vector: "+str(right_vector))
    right_shoulder_position = neck_position + right_vector * head_to_shoulder
    #print("right_shoulder_position: " + str(right_shoulder_position))
    left_shoulder_position = neck_position - right_vector * head_to_shoulder  
    #print("left_shoulder_position: " + str(left_shoulder_position))

def normalization(direction_vector):
    magnitude = np.linalg.norm(direction_vector)
    normalized_vector = direction_vector / magnitude if magnitude != 0 else direction_vector
    return normalized_vector

def get_left_arm_angles(left_pos):
    global la_x, la_y
    left_hand_position = np.array([left_pos.x, left_pos.y, left_pos.z])
    direction_vector = left_hand_position - left_shoulder_position
    shoulder_to_hand_vector = normalization(direction_vector)
    # XZ planar part of vector
    xz_neg_right_vector = normalization(np.array([-right_vector[0], -right_vector[2]]))
    xz_shoulder_to_hand_vector = normalization(np.array([shoulder_to_hand_vector[0], shoulder_to_hand_vector[2]]))
    dot_product = np.dot(xz_neg_right_vector, xz_shoulder_to_hand_vector)
    # Calculate the magnitudes
    magnitude_xz_neg_right_vector = np.linalg.norm(xz_neg_right_vector)
    magnitude_xz_normalized_vector = np.linalg.norm(xz_shoulder_to_hand_vector)        
    # Calculate the angle in radians
    la_y = math.acos(dot_product / (magnitude_xz_neg_right_vector * magnitude_xz_normalized_vector))
    # Y Height part of vector
    y_part = left_hand_position[1] - neck_position[1]
    la_x = math.atan2(y_part, arm_length) - PI_OVER_2
    
def get_right_arm_angles(right_pos):
    global ra_x, ra_y
    right_hand_position = np.array([right_pos.x, right_pos.y, right_pos.z])
    direction_vector = right_hand_position - right_shoulder_position
    shoulder_to_hand_vector = normalization(direction_vector)
    # XZ planar part of vector
    xz_right_vector = normalization(np.array([right_vector[0], right_vector[2]]))
    xz_shoulder_to_hand_vector = normalization(np.array([shoulder_to_hand_vector[0], shoulder_to_hand_vector[2]]))
    dot_product = np.dot(xz_right_vector, xz_shoulder_to_hand_vector)
    # Calculate the magnitudes
    magnitude_xz_right_vector = np.linalg.norm(xz_right_vector)
    magnitude_xz_normalized_vector = np.linalg.norm(xz_shoulder_to_hand_vector)        
    # Calculate the angle in radians
    ra_y = math.acos(dot_product / (magnitude_xz_right_vector * magnitude_xz_normalized_vector))
    # Y Height part of vector
    y_part = right_hand_position[1] - neck_position[1]
    ra_x = math.atan2(y_part, arm_length) + PI_OVER_2

def get_head_angles(hmd_rotation):
    global head_yaw, head_pitch
    head_pitch = hmd_rotation.y
    head_yaw = hmd_rotation.z

def inverse_kinematics(frame_dto):
    get_shoulders_position(frame_dto)
    get_left_arm_angles(frame_dto.leftHand.position)
    get_right_arm_angles(frame_dto.rightHand.position)
    get_head_angles(frame_dto.hmd.rotation)

def run_ros2_tpose():
    run_ros2_command(0, PI_OVER_2, 0, -PI_OVER_2, 10)

def run_ros2_command(ra_y, ra_x, la_y, la_x, dest_time):
    command = [
        "ros2", "action", "send_goal", "/joint_trajectory_controller/follow_joint_trajectory",
        "control_msgs/action/FollowJointTrajectory", "-f", 
        f'''{{
            "trajectory": {{
                "joint_names": ["RTz", "RTx", "RTy", "RSy", "RFy", "RFx", "LTz", "LTx", "LTy", "LSy", "LFy", "LFx", "RAy", "RAx", "LAy", "LAx"],
                "points": [
                    {{
                        "positions": [0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, {ra_y}, {ra_x}, {la_y}, {la_x}], 
                        "time_from_start": {{"sec": {dest_time}}}
                    }}
                ]
            }}
        }}'''
    ]
    print(command)
    proc = subprocess.Popen(command)
    time.sleep(5*dest_time)  # Wait for the destination time # 5x for simulation
    proc.terminate()  # Terminate the subprocess

if __name__ == '__main__':
    # Start the server in a separate thread
    server_thread = Thread(target=lambda: app.run(debug=True, use_reloader=False))
    server_thread.start()
    
    # Give the server a moment to start up
    #time.sleep(5)    
    #run_ros2_tpose()
    
    # Join the server thread to keep the script running
    server_thread.join()