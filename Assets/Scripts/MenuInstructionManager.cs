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
    private float minDeltaTime = 2.0f;

    private Image panelBackgroundColor;
    private Color inactiveColor = new(0.0f, 0.0f, 0.0f, 0.7f);
    private Color activeColor = new(0.0f, 1.0f, 0.0f, 0.7f);

    private Vector3 position;
    private Quaternion rotation;

    private float lastFrameTime = -1.0f;
    private float minFrameDeltaTime = 1.0f; 

    void Awake()
    {
        panelBackgroundColor = panel.GetComponent<Image>();
        robotConnection = GetComponent<RobotConnection>();
    }

    void Start()
    {

    }

    void Update()
    {
        var leftTrigger = leftHandTriggerAction.action.ReadValue<float>();
        var rightTrigger = rightHandTriggerAction.action.ReadValue<float>();
        float time = Time.time;
        float deltaTime = time - activationTime;

        if (robotConnection.sendData)
        {
            SendFrame();
        }

        if (leftTrigger == 1.0f && rightTrigger == 1.0f && deltaTime > minDeltaTime)
        {
            if (ValidatePositions())
            {
                robotConnection.sendData = !robotConnection.sendData;
                activationTime = time;
                if (robotConnection.sendData)
                {
                    panelBackgroundColor.color = activeColor;
                    SendStart();
                }
                else
                {
                    panelBackgroundColor.color = inactiveColor;
                }                
            }
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
        GameObject hmdGO, GameObject leftHandGO, GameObject rightHandGO)
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

    void SendStart()
    {
        position = hmdGO.transform.position;
        rotation = hmdGO.transform.rotation;
        var hmd = new DeviceDto(position, rotation);
        var preForward = new Vector3(hmdGO.transform.forward.x, 0, hmdGO.transform.forward.z).normalized;
        var forward = new Vector3Dto(preForward.x, preForward.y, preForward.z);

        position = leftHandGO.transform.position;
        rotation = leftHandGO.transform.rotation;
        var left = new DeviceDto(position, rotation);

        position = rightHandGO.transform.position;
        rotation = rightHandGO.transform.rotation;
        var right = new DeviceDto(position, rotation);
        var input = new FrameDto(forward,hmd, left, right, Time.time);
        robotConnection.SendStart(input);
    }

    void SendFrame()
    {
        if(Time.time - lastFrameTime >= minFrameDeltaTime)
        {
            lastFrameTime = Time.time;

            position = hmdGO.transform.position;
            rotation = hmdGO.transform.rotation;
            var hmd = new DeviceDto(position, rotation);
            var preForward = new Vector3(hmdGO.transform.forward.x, 0, hmdGO.transform.forward.z).normalized;
            var forward = new Vector3Dto(preForward.x, preForward.y, preForward.z);

            position = leftHandGO.transform.position;
            rotation = leftHandGO.transform.rotation;
            var left = new DeviceDto(position, rotation);

            position = rightHandGO.transform.position;
            rotation = rightHandGO.transform.rotation;
            var right = new DeviceDto(position, rotation);

            var input = new FrameDto(forward, hmd, left, right, Time.time);
            robotConnection.SendFrame(input);
        }        
    }
}
