
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;   // The target to follow
    public float distance;    // The distance to maintain from the target
    public float smoothTime = 0.3f;   // The time it takes to reach the target position

    private Vector3 startPosition;  // The initial position relative to the target
    private Vector3 velocity;   // The current velocity of the object

    private float lookTimer = 0f; // The remaining time to look in front of the player

    private void Start()
    {

    }
    //private void OnEnable()
    //{
    //    if (target != null)
    //    {
    //        // Calculate the target position based on the distance and the target's position
    //        Vector3 targetPosition = target.position + startPosition - Vector3.forward * distance;
    //        // Smoothly move towards the target position using an easing function
    //        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    //        //transform.position = targetPosition;
    //    }
    //    // Calculate the initial position based on the distance
    //    startPosition = transform.position - target.position + Vector3.forward * distance;

    //}
    private void Update()
    {
        if(target != null)
        {
            // Calculate the target position based on the distance and the target's position
            Vector3 targetPosition = target.position + startPosition - Vector3.forward * distance;
            // Smoothly move towards the target position using an easing function
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            //transform.position = targetPosition;
        }

        //if (target == null)
        //{
        //    // Calculate the target position based on the distance and the target's position
        //    Vector3 targetPosition = Camera.main.transform.position + startPosition - Vector3.forward * distance;
        //    // Smoothly move towards the target position using an easing function
        //    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        //    //transform.position = targetPosition;

        //}

        //// Make the camera look towards the front of the target
        //transform.LookAt(target.position + target.forward * 1.0f);

        //LookTargeting();
    }


    //void LookTargeting()
    //{
    //    // Check if the player is moving
    //    if (target.GetComponent<Rigidbody>().velocity.magnitude > 0f)
    //    {
    //        // Decrease the look timer
    //        lookTimer -= Time.deltaTime;

    //        // Calculate the target position based on the distance and the target's position
    //        Vector3 targetPosition = target.position + startPosition - Vector3.forward * distance;

    //        // Check if the look timer has expired
    //        if (lookTimer > 0f)
    //        {
    //            // Look in front of the player
    //            transform.LookAt(targetPosition + target.forward * 10f);
    //        }
    //        else
    //        {
    //            // Smoothly move towards the target position using an easing function
    //            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    //        }
    //    }
    //    else
    //    {
    //        // Reset the look timer if the player is not moving
    //        lookTimer = 1f;
    //    }
    //}
}