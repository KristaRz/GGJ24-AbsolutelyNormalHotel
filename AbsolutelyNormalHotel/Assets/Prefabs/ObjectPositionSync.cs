using UnityEngine;

public class ObjectPositionSync : MonoBehaviour
{
    public Transform target;   // The target to follow
    public float smoothTime = 0.3f;   // The time it takes to reach the target position
    public GameObject activationObject; // The object that controls activation

    private Vector3 startLocalPosition;  // The initial position relative to the target
    private Quaternion startLocalRotation; // The initial rotation relative to the target
    private Vector3 velocity;   // The current velocity of the object

    private Vector3 desiredPosition;
    private Quaternion desiredRotation;

    private void Start()
    {
        // Calculate the initial position relative to the target
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localRotation;

        // Calculate the target position and rotation based on the distance and the target's position
        desiredPosition = transform.position - target.position;
        desiredRotation = Quaternion.Inverse(target.rotation) * transform.rotation;
    }

    private void FixedUpdate()
    {
        if (activationObject.activeSelf)
        {
            // Smoothly move towards the target position using an easing function
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition + target.position, ref velocity, smoothTime);

            // Calculate the centered rotation by applying the desired rotation to the target's rotation
            Quaternion centeredRotation = target.rotation * desiredRotation;

            transform.SetPositionAndRotation(smoothedPosition, centeredRotation);
        }
        else
        {
            // Reset the object's position and rotation to the initial position and rotation relative to the target
            transform.localPosition = startLocalPosition;
            transform.localRotation = startLocalRotation;
        }
    }
}
