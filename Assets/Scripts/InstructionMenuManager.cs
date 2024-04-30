using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InstructionMenuManager : MonoBehaviour
{
    public GameObject hmdGO;
    public GameObject leftHandGO;
    public GameObject rightHandGO;
    public GameObject panel;   

    public InputActionProperty leftHandTriggerAction;
    public InputActionProperty rightHandTriggerAction;

    private RobotConnection robotConnection;
    private float activationTime = 0;

    private Image panelBackgroundColor;
    private Color inactiveColor = new(0.0f, 0.0f, 0.0f, 0.7f);
    private Color activeColor = new(0.0f, 1.0f, 0.0f, 0.7f);

    void Awake()
    {
        panelBackgroundColor = panel.GetComponent<Image>();
        robotConnection = GetComponent<RobotConnection>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var leftTrigger = leftHandTriggerAction.action.ReadValue<float>();
        var rightTrigger = rightHandTriggerAction.action.ReadValue<float>();
        float time = Time.time;
        float deltaTime = time - activationTime; // > 2 seconds

        if (leftTrigger == 1.0f && rightTrigger == 1.0f /*&& robotConnection.robotConnection*/ && deltaTime > 2.0f)
        {
            if (ValidatePositions())
            {
                robotConnection.sendData = !robotConnection.sendData;
                activationTime = time;
                if (robotConnection.sendData)
                {
                    panelBackgroundColor.color = activeColor;
                }
                else
                {
                    panelBackgroundColor.color = inactiveColor;
                }
                StartCoroutine(VideoRequest(robotConnection.sendData));
            }
        }

        if(robotConnection.sendData)
        {
            var headPosition = hmdGO.transform.position;
            var leftHandPosition = leftHandGO.transform.position;
            var rightHandPosition = rightHandGO.transform.position;
            var inputData = new InputData(
                new Device(headPosition, hmdGO.transform.rotation),                                       
                new Device(leftHandPosition, leftHandGO.transform.rotation),
                new Device(rightHandPosition, rightHandGO.transform.rotation)
            );
            StartCoroutine(SendData(inputData));
        }
    }

    // 1. head is in the middle (with a tolerance of 0.1f)
    // 2. left hand and right hand are on the same y position (with a tolerance of 0.1f)
    private bool ValidatePositions()
    {
        var (headInMiddle, _) = ValidateHeadInTheMiddle(hmdGO, leftHandGO, rightHandGO);
        var (handsOnSameY, _) = ValidateHandsSameY(leftHandGO, rightHandGO);

        return headInMiddle && handsOnSameY;
    }

    public (bool, float) ValidateHeadInTheMiddle(
        GameObject hmdGO, GameObject leftHandGO, GameObject rightHandGO )
    {
        const float forwardDistanceBetweenHandsAndHead = 0.3f;
        const float headHeight = 0.2f;
        const float tolerance = 0.1f;

        var headForward = hmdGO.transform.forward;
        var headPosition = hmdGO.transform.position;
        var leftHandPosition = leftHandGO.transform.position;
        var rightHandPosition = rightHandGO.transform.position;

        var forwardVector = new Vector3(headForward.x, 0, headForward.z).normalized;
        var middlePosition = (leftHandPosition + rightHandPosition) / 2;
        var neckPosition = headPosition - forwardVector * forwardDistanceBetweenHandsAndHead;
        neckPosition.y -= headHeight;

        var neckDistance = Vector3.Distance(neckPosition, middlePosition);
        bool headInMiddle = neckDistance < tolerance;

        return (headInMiddle, neckDistance);
    }

    public (bool, float) ValidateHandsSameY(
        GameObject leftHandGO, GameObject rightHandGO)
    {
        const float tolerance = 0.1f;

        var leftHandPosition = leftHandGO.transform.position;
        var rightHandPosition = rightHandGO.transform.position;

        var handsDistance = Mathf.Abs(leftHandPosition.y - rightHandPosition.y);
        bool handsOnSameY = handsDistance < tolerance;

        return (handsOnSameY, handsDistance);
    }

    // Send data to robot
    // forward vector
    // left hand position and rotation
    // right hand position and rotation
    // head position and rotation
    // THEN
    // robot reduce
    // 1. scale between head and hands
    // 2. scale between head and floor
    // end send video streaming
    IEnumerator SendData(InputData inputData)
    {
        string jsonToSend = JsonUtility.ToJson(inputData);
        UnityWebRequest www = UnityWebRequest.PostWwwForm(robotConnection.robotUrl, "");
        www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonToSend));
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

    IEnumerator VideoRequest(bool activate)
    {
        UnityWebRequest www = UnityWebRequest.Get($"{robotConnection.robotUrl}?activate={activate}");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Request for video was proceed");
        }
    }
}
