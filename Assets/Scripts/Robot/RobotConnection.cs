using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RobotConnection : MonoBehaviour
{
    public bool sendData = false;

    public static string url = "192.168.43.232:5000";

    private string startTracking = $"{url}/start-tracking";
    private string frames = $"{url}/frames";

    public void SendStart(FrameDto frame)
    {
        StartCoroutine(SendStartCoroutine(frame));
    }

    IEnumerator SendStartCoroutine(FrameDto inputData)
    {
        string jsonToSend = JsonUtility.ToJson(inputData);
        UnityWebRequest www = UnityWebRequest.PostWwwForm(startTracking, "");
        www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonToSend));
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Start Form upload complete!");
        }
    }

    public void SendFrame(FrameDto frame)
    {
        StartCoroutine(SendFrameCoroutine(frame));
    }

    IEnumerator SendFrameCoroutine(FrameDto inputData)
    {
        string jsonToSend = JsonUtility.ToJson(inputData);
        UnityWebRequest www = UnityWebRequest.PostWwwForm(frames, "");
        www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonToSend));
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        Debug.Log(jsonToSend);
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
