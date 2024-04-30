using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MotionMenuManager : MonoBehaviour
{
    public GameObject instructionMenu;
    private InstructionMenuManager instructionMenuManager;

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

    // Triggers
    public TextMeshProUGUI handsSameY;
    public TextMeshProUGUI headInTheMiddle;

    private Vector3 position;
    private Quaternion rotation;

    void Awake()
    {
        instructionMenuManager = instructionMenu.GetComponent<InstructionMenuManager>();
    }

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

        var (headInMiddle, neckDistance) = instructionMenuManager.ValidateHeadInTheMiddle(hmdGO, leftHandGO, rightHandGO);
        headInTheMiddle.text = $"Head in the middle: {headInMiddle} - distance: {neckDistance}";

        var (handsOnSameY, handsDistance) = instructionMenuManager.ValidateHandsSameY(leftHandGO, rightHandGO);
        handsSameY.text = $"Hands same Y: {handsOnSameY} - distance: {handsDistance}";
    }
}
