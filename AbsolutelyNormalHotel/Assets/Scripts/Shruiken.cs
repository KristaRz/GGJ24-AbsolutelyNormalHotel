using UnityEngine;
using UnityEngine.Events;

public class Shuriken : MonoBehaviour
{
    private Rigidbody rigidBody;
    private bool isHeld = false;
    private bool isFreeFalling = false; // Flag to indicate if the shuriken is in free fall
    private Vector3 lastPosition;
    private Vector3 positionBeforeRelease;
    private Quaternion lastRotation;
    private Vector3 velocity;
    private Vector3 angularVelocity;
    public Animator anim;



    public UnityEvent OnHit;
    private Vector3 triggerPosition;

    // Variables for easing the descent
    private float maxDescentSpeed = 9.81f; // Maximum descent speed

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Called when the shuriken starts being held
    public void Grab()
    {
        isHeld = true;
        isFreeFalling = false; // Stop free falling when grabbed
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    // Called when the shuriken is released
    public void Release()
    {
        anim.SetTrigger("Roll");

        isHeld = false;
        isFreeFalling = true; // Start free falling when released

        // Calculate the direction of movement at the moment of release
        Vector3 releaseDirection = (transform.position - positionBeforeRelease).normalized;
        releaseDirection.y = 0; // Setting Y to 0 to keep horizontal direction

        // Set the velocity to continue in the release direction
        rigidBody.velocity = releaseDirection * velocity.magnitude * 2; // Adjust the multiplier as needed

        rigidBody.angularVelocity = angularVelocity;
        gameObject.transform.rotation = lastRotation;



    }

    private void Update()
    {
        if (isHeld)
        {
            velocity = (transform.position - lastPosition) / Time.deltaTime;
            angularVelocity = rigidBody.angularVelocity;

            // Update last position and rotation for the next frame
            lastPosition = transform.position;
            lastRotation = transform.rotation;

            // Update positionBeforeRelease
            positionBeforeRelease = lastPosition;
        }
        else if (isFreeFalling)
        {


            EaseDescent(); // Apply ease descent while in free fall
                           // Rotate anim.gameObject
        }


    }
    private void EaseDescent()
    {
        float easeFactor = CalculateEaseFactor(rigidBody.velocity.y, maxDescentSpeed);
        Vector3 newVelocity = rigidBody.velocity;
        newVelocity.y *= easeFactor;
        rigidBody.velocity = newVelocity;

    }

    private float CalculateEaseFactor(float currentSpeed, float maxSpeed)
    {
        return 1 - Mathf.Clamp01(Mathf.Abs(currentSpeed) / maxSpeed);
    }

    public void InstantiatePrefab(GameObject prefab)
    {
        Instantiate(prefab, triggerPosition, Quaternion.identity);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerPosition = other.ClosestPoint(transform.position);
        if (other.CompareTag("Chicken"))
        {
            OnHit.Invoke();
            other.gameObject.GetComponent<ChickenGame>().DestroySelf();
        }
        if (!other.CompareTag("Hand") && other.name != "Direct Interactor")
        {


            // Instead of setting a strong gravity effect, halt the shuriken more gently
            //rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);


            rigidBody.angularVelocity = Vector3.zero; // Optionally, stop angular velocity as well
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        anim.SetTrigger("Idle");
        isFreeFalling = false; // Also stop free falling
    }

}
