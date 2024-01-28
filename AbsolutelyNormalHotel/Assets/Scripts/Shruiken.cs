using UnityEngine;
using UnityEngine.Events;
using System.Collections;

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

    public float scaleDownInSec = 1.0f; // Time before starting to scale down
    public float scaleDuration = 2.0f; // Duration of the scale down process


    public UnityEvent OnHit;
    public UnityEvent OnReset;
    private Vector3 triggerPosition;


    private Transform parent;
    // Variables for easing the descent
    private float maxDescentSpeed = 9.81f; // Maximum descent speed

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        parent = transform;
        rigidBody = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
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
        rigidBody.isKinematic = false;

        anim.SetBool("Spin", true);

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
    public void InstantiatePrefabInGameObjectPosition   (GameObject prefab)
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
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
        if (!other.CompareTag("Hand") && other.name != "Direct Interactor" && !other.CompareTag("Player"))
        {


            // Instead of setting a strong gravity effect, halt the shuriken more gently
            //rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);

            if(!other.gameObject.CompareTag("Chicken"))
            rigidBody.angularVelocity = Vector3.zero; // Optionally, stop angular velocity as well
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Platform" && !collision.gameObject.CompareTag("Shuriken") && !collision.gameObject.CompareTag("Chicken"))
        {
            // Deactivate the trigger collider
            Collider triggerCollider = GetComponent<Collider>();
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }

            anim.SetBool("Spin", false);
            isFreeFalling = false; // Also stop free falling
            StartCoroutine(ScaleDownAndDestroy());
        }
    }


    private IEnumerator ScaleDownAndDestroy()
    {
        // Wait for the specified delay before starting the scale down
        yield return new WaitForSeconds(scaleDownInSec);

        float elapsed = 0;
        Vector3 originalScale = transform.localScale; // Store the original scale

        if (transform.localScale.x > 0.1f && transform.localScale.y > 0.1f && transform.localScale.z > 0.1f)
        {
            elapsed += Time.deltaTime;
            float blend = Mathf.Clamp01(elapsed / scaleDuration);
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, blend); // Scale down
            yield return null;
        }
        StopCoroutine("ScaleDownAndDestroy");
        ResetObject();
    }
    public void ResetObject()
    {
        OnReset.Invoke();
        transform.localScale = new Vector3(1, 1, 1);
        rigidBody.isKinematic = true;
        transform.localScale = Vector3.one; // Reset scale to its initial value

        transform.parent = parent;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        gameObject.SetActive(true); // Reactivate the GameObject if it was deactivated

   
    }


}
