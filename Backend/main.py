from flask import Flask, request, jsonify
from pydantic import BaseModel
from typing import Optional
import json

app = Flask(__name__)

start_tracking = False

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

@app.route('/start-tracking', methods=['POST'])
def start_tracing():
    try:
        data = request.json
        frame_dto = FrameDto(**data)
        # Process initial position
        # oblicz pozycje barkow i dlugosc rak

        print(frame_dto)
        start_tracking = True;
        return jsonify({"message": "Started tracing"}), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 400

@app.route('/frames', methods=['POST'])
def accept_frame():
    try:
        data = request.json
        frame_dto = FrameDto(**data)
        # Set head rotation and send as action
        # Compute angles in arms and send as action

        print(frame_dto)
        return jsonify({"message": "Frame received successfully"}), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 400

if __name__ == '__main__':
    app.run(debug=True)
    # Set robotin T-pose