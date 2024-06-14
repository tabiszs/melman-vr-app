// using System.Collections;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using UnityEngine.Networking;
// using UnityEngine.UI;

// public class InstructionMenuManager : MonoBehaviour
// {
//     public GameObject hmdGO;
//     public GameObject leftHandGO;
//     public GameObject rightHandGO;
//     public GameObject panel;   

//     public InputActionProperty leftHandTriggerAction;
//     public InputActionProperty rightHandTriggerAction;

//     private RobotConnection robotConnection;
//     private float activationTime = 0;

//     private Image panelBackgroundColor;
//     private Color inactiveColor = new(0.0f, 0.0f, 0.0f, 0.7f);
//     private Color activeColor = new(0.0f, 1.0f, 0.0f, 0.7f);

//     void Awake()
//     {
//         panelBackgroundColor = panel.GetComponent<Image>();
//         robotConnection = GetComponent<RobotConnection>();
//     }

//     // Start is called before the first frame update
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {
//         var leftTrigger = leftHandTriggerAction.action.ReadValue<float>();
//         var rightTrigger = rightHandTriggerAction.action.ReadValue<float>();
//         float time = Time.time;
//         float deltaTime = time - activationTime; // > 2 seconds

//         if (leftTrigger == 1.0f && rightTrigger == 1.0f && deltaTime > 2.0f)
//         {
//             if (ValidatePositions())
//             {
//                 robotConnection.sendData = !robotConnection.sendData;
//                 activationTime = time;
//                 if (robotConnection.sendData)
//                 {
//                     panelBackgroundColor.color = activeColor;
//                 }
//                 else
//                 {
//                     panelBackgroundColor.color = inactiveColor;
//                 }
//                 //robotConnection.SendStart(inputData);
//                 StartCoroutine(VideoRequest(robotConnection.sendData));
//             }
//         }

//         if(robotConnection.sendData)
//         {
//             var position = hmdGO.transform.position;
//             var rotation = hmdGO.transform.rotation;
//             var hmd = new DeviceDto(position, rotation);

//             position = leftHandGO.transform.position;
//             rotation = leftHandGO.transform.rotation;
//             var left = new DeviceDto(position, rotation);

//             position = rightHandGO.transform.position;
//             rotation = rightHandGO.transform.rotation;
//             var right = new DeviceDto(position, rotation);
//             var input = new FrameDto(hmd, left, right, Time.time);
//             StartCoroutine(backendConnection.PostSessionFrame(input));

//             var headPosition = hmdGO.transform.position;
//             var leftHandPosition = leftHandGO.transform.position;
//             var rightHandPosition = rightHandGO.transform.position;
//             var inputData = new FrameDto(
//                 new DeviceDto(headPosition, hmdGO.transform.rotation),                                       
//                 new DeviceDto(leftHandPosition, leftHandGO.transform.rotation),
//                 new DeviceDto(rightHandPosition, rightHandGO.transform.rotation)
//             );
//             robotConnection.SendFrame(inputData);
//         }
//     }

//     // 1. head is in the middle (with a tolerance of 0.1f)
//     // 2. left hand and right hand are on the same y position (with a tolerance of 0.1f)
//     private bool ValidatePositions()
//     {
//         var (headInMiddle, _) = ValidateHeadInTheMiddle(hmdGO, leftHandGO, rightHandGO);
//         var (handsOnSameY, _) = ValidateHandsSameY(leftHandGO, rightHandGO);

//         return headInMiddle && handsOnSameY;
//     }

//     public (bool, float) ValidateHeadInTheMiddle(
//         GameObject hmdGO, GameObject leftHandGO, GameObject rightHandGO )
//     {
//         const float forwardDistanceBetweenHandsAndHead = 0.3f;
//         const float headHeight = 0.2f;
//         const float tolerance = 0.1f;

//         var headForward = hmdGO.transform.forward;
//         var headPosition = hmdGO.transform.position;
//         var leftHandPosition = leftHandGO.transform.position;
//         var rightHandPosition = rightHandGO.transform.position;

//         var forwardVector = new Vector3(headForward.x, 0, headForward.z).normalized;
//         var middlePosition = (leftHandPosition + rightHandPosition) / 2;
//         var neckPosition = headPosition - forwardVector * forwardDistanceBetweenHandsAndHead;
//         neckPosition.y -= headHeight;

//         var neckDistance = Vector3.Distance(neckPosition, middlePosition);
//         bool headInMiddle = neckDistance < tolerance;

//         return (headInMiddle, neckDistance);
//     }

//     public (bool, float) ValidateHandsSameY(
//         GameObject leftHandGO, GameObject rightHandGO)
//     {
//         const float tolerance = 0.1f;

//         var leftHandPosition = leftHandGO.transform.position;
//         var rightHandPosition = rightHandGO.transform.position;

//         var handsDistance = Mathf.Abs(leftHandPosition.y - rightHandPosition.y);
//         bool handsOnSameY = handsDistance < tolerance;

//         return (handsOnSameY, handsDistance);
//     }

//     IEnumerator VideoRequest(bool activate)
//     {
//         UnityWebRequest www = UnityWebRequest.Get($"{robotConnection.robotUrl}?activate={activate}");
//         yield return www.SendWebRequest();

//         if (www.result != UnityWebRequest.Result.Success)
//         {
//             Debug.Log(www.error);
//         }
//         else
//         {
//             Debug.Log("Request for video was proceed");
//         }
//     }
// }
