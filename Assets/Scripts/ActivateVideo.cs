// using UnityEngine;

// public class ActivateVideo : MonoBehaviour
// {
//     public GameObject instructionMenu;
//     private RobotConnection robotConnection;
//     private bool showVideo = false;

//     public GameObject video;
//     public GameObject plane;

//     void Awake()
//     {
//         robotConnection = instructionMenu.GetComponent<RobotConnection>();
//     }

//     // Start is called before the first frame update
//     void Start()
//     {
//         video.SetActive(true);
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (robotConnection.sendData != showVideo)
//         {
//             showVideo = !showVideo;
//             video.SetActive(!video.activeSelf);
//             plane.SetActive(!plane.activeSelf);
//         }
//     }
// }
