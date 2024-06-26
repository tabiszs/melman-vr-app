using System;
using UnityEngine;

[Serializable]
public class FrameDto
{
    public Vector3Dto forward;
    public DeviceDto hmd;
    public DeviceDto leftHand;
    public DeviceDto rightHand;
    public float time;

    public FrameDto() { }

    public FrameDto(Vector3Dto forward, DeviceDto hmd, DeviceDto leftHand, DeviceDto rightHand, float time)
    {
        this.forward = forward;
        this.hmd = hmd;
        this.leftHand = leftHand;
        this.rightHand = rightHand;
        this.time = time;
    }
}

[Serializable]
public class DeviceDto
{
    public Vector3Dto position;
    public Vector3Dto rotation;

    public DeviceDto() { }

    public DeviceDto(Vector3 position, Quaternion rotation)
    {
        this.position = new Vector3Dto()
        {
            x = position.x,
            y = position.y,
            z = position.z
        };
        this.rotation = new Vector3Dto()
        {
            x = rotation.eulerAngles.x,
            y = rotation.eulerAngles.y,
            z = rotation.eulerAngles.z
        };
    }
}

[Serializable]
public class Vector3Dto
{
    public float x;
    public float y;
    public float z;

    public Vector3Dto() { }

    public Vector3Dto(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

[Serializable]
public class QuaternionDto
{
    public float w;
    public float x;
    public float y;
    public float z;
}