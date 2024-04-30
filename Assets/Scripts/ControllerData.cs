using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Device
{
    public Vector3 Position;
    public Vector3 Rotation;
    public Device() { }
    public Device(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation.eulerAngles;
    }
}

[Serializable]
public class InputData
{
    public Device HMD;
    public Device LeftHand;
    public Device RightHand;
    public float time;
    public InputData(Device hmd, Device left, Device right)
    {
        HMD = hmd;
        LeftHand = left;
        RightHand = right;
        time = Time.time;
    }
}
