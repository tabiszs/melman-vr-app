using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class RobotConnectionManager : MonoBehaviour
{
    private const float waitTime = 1.0f;
    private const string brokerUrl = "http://192.168.0.101:5010";
    private const string initSessionUrl = brokerUrl + "/session";
    
    private bool getRobotUrl = false;
    private bool brokerConnection = false;

    private RobotConnection RobotConnection;

    public TextMeshProUGUI connectionText;

    void Awake()
    {
        RobotConnection = GetComponent<RobotConnection>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitSession());
    }

    // Update is called once per frame
    void Update()
    {
        if(brokerConnection && getRobotUrl && RobotConnection.robotConnection)
        {
            connectionText.text = "Connected with robot";
        }
        else
        {
            connectionText.text = "Waiting for connection...";
        }
    }

    IEnumerator InitSession()
    {
        while(!brokerConnection)
        {
            UnityWebRequest post = UnityWebRequest.PostWwwForm(initSessionUrl, "");
            yield return post.SendWebRequest();

            if (post.result != UnityWebRequest.Result.Success)
            {
                yield return new WaitForSeconds(waitTime);
                Debug.Log("Waiting for connection...");
            }
            else
            {
                Debug.Log("IP saved in broker");
                brokerConnection = true;
            }
        }

        while (!getRobotUrl)
        {
            UnityWebRequest get = UnityWebRequest.Get(initSessionUrl);
            yield return get.SendWebRequest();

            if (get.result != UnityWebRequest.Result.Success)
            {
                yield return new WaitForSeconds(waitTime);
                Debug.Log("Waiting for robot IP...");
            }
            else
            {
                RobotConnection.robotUrl = get.downloadHandler.text;
                Debug.Log($"Get robot IP: {RobotConnection.robotUrl}");
                getRobotUrl = true;
            }
        }

        while (!RobotConnection.robotConnection)
        {
            UnityWebRequest get = UnityWebRequest.Get(initSessionUrl);
            yield return get.SendWebRequest();

            if (get.result != UnityWebRequest.Result.Success)
            {
                yield return new WaitForSeconds(waitTime);
                Debug.Log("Waiting for robot connection...");
            }
            else
            {
                Debug.Log($"Connection established");
                RobotConnection.robotConnection = true;
            }
        }
    }
}
