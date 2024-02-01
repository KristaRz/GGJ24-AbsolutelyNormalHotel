using UnityEngine;

public class RotateAndFollowHeadset : MonoBehaviour
{
    public Transform headset;  // The headset transform (e.g., the player's camera)
    public float rotationSpeed = 2.0f;  // Rotation speed
    public float followSpeed = 5.0f;    // Speed at which the object follows the headset
    private Vector3 initialOffset; // The initial offset from the headset to the object

    private void Start()
    {
        if (headset != null)
        {
            // Calculate the initial offset from the headset to the object
            initialOffset = transform.position - headset.position;
        }
        else
        {
            Debug.LogError("Headset transform is not assigned!");
        }
    }

    private void Update()
    {
        if (headset != null)
        {
            // Calculate the desired Y-axis rotation based on the headset's rotation
            float headsetYRotation = headset.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, headsetYRotation, transform.eulerAngles.z);

            // Smoothly interpolate the object's rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Calculate the new position based on the headset's position and the initial offset
            Vector3 targetPosition = headset.position + initialOffset;

            // Smoothly interpolate the object's position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }
    }
}
