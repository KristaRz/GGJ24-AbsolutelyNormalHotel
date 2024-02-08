using UnityEngine;

public class RotateAndFollowHeadset : MonoBehaviour
{
    /*

    private Transform _headset;  // The headset transform (e.g., the player's camera)
    public float rotationSpeed = 2.0f;  // Rotation speed
    public float followSpeed = 5.0f;    // Speed at which the object follows the headset
    private Vector3 initialOffset; // The initial offset from the headset to the object

    private void Start()
    {
        transform.parent = null;
        _headset = Camera.main.transform;

        // Calculate the initial offset from the headset to the object
        initialOffset = transform.position - _headset.position;
    }

    private void Update()
    {
        if (_headset != null)
        {
            // Calculate the desired Y-axis rotation based on the headset's rotation
            float headsetYRotation = _headset.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, headsetYRotation, transform.eulerAngles.z);

            // Smoothly interpolate the object's rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Calculate the new position based on the headset's position and the initial offset
            Vector3 targetPosition = _headset.position + initialOffset;

            // Smoothly interpolate the object's position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }
    }
    */

    [SerializeField] private float deadzone = 0.1f;
    [SerializeField] private float lerpSpeed = 0.01f;
    [SerializeField] private bool overrideY = false;
    [SerializeField] private Transform overrideYReference;

    private Transform parentTransform;
    private Quaternion targetRotation;


    private void Awake()
    {
        parentTransform = transform.parent;
        transform.parent = null;
    }

    private void Update()
    {
        transform.position = parentTransform.position;
        if (overrideY)
            transform.position = new Vector3(parentTransform.position.x, overrideYReference.position.y, parentTransform.position.z);

        if (Vector3.Dot(parentTransform.forward, transform.forward) <= deadzone)
            targetRotation = parentTransform.rotation;

        Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed);
        newRotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);
        transform.rotation = newRotation;
    }
}
