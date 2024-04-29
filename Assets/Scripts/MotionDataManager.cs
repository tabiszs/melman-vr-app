using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MotionDataManager : MonoBehaviour
{
    public GameObject hmdGO;
    public GameObject leftHandGO;
    public GameObject rightHandGO;

    // HMD
    public TextMeshProUGUI hmdPositionText;
    public TextMeshProUGUI hmdRotationText;

    // Left Hand
    public TextMeshProUGUI leftHandPositionText;
    public TextMeshProUGUI leftHandRotationText;

    // Right Hand
    public TextMeshProUGUI rightHandPositionText;
    public TextMeshProUGUI rightHandRotationText;

    private Vector3 position;
    private Quaternion rotation;
    private Vector3 velocity;
    private Vector3 angularVelocity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        position = hmdGO.transform.position;
        rotation = hmdGO.transform.rotation;
        hmdPositionText.text = "Position: " + position.ToString();
        hmdRotationText.text = "Rotation: " + rotation.eulerAngles.ToString();

        position = leftHandGO.transform.position;
        rotation = leftHandGO.transform.rotation;
        leftHandPositionText.text = "Position: " + position.ToString();
        leftHandRotationText.text = "Rotation: " + rotation.eulerAngles.ToString();

        position = rightHandGO.transform.position;
        rotation = rightHandGO.transform.rotation;
        rightHandPositionText.text = "Position: " + position.ToString();
        rightHandRotationText.text = "Rotation: " + rotation.eulerAngles.ToString();
    }
}
