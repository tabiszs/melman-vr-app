// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.Networking;

// public class RobotConnection : MonoBehaviour
// {
//     public bool sendData = false;

//     public string url = "192.168.2.101:5000";

//     private string startTracking = $"{url}/start-tracking";
//     private string frames = $"{url}/frames";
   

//     public void SendFame(FrameDto frame)
//     {
//         StartCoroutine(SendFrameCoroutine(frame));
//     }

//     // Send data to robot
//     // forward vector
//     // left hand position and rotation
//     // right hand position and rotation
//     // head position and rotation
//     // THEN
//     // robot reduce
//     // 1. scale between head and hands
//     // 2. scale between head and floor
//     // end send video streaming
//     IEnumerator SendFrameCoroutine(FrameDto inputData)
//     {
//         string jsonToSend = JsonUtility.ToJson(inputData);
//         UnityWebRequest www = UnityWebRequest.PostWwwForm(frames, "");
//         www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonToSend));
//         www.SetRequestHeader("Content-Type", "application/json");
//         yield return www.SendWebRequest();

//         if (www.result != UnityWebRequest.Result.Success)
//         {
//             Debug.Log(www.error);
//         }
//         else
//         {
//             Debug.Log("Form upload complete!");
//         }
//     }

//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }
// }
