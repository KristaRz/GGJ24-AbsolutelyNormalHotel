using System;
using UnityEngine;
using UnityEngine.Events;

public class Shuriken : MonoBehaviour
{
    private Rigidbody rigidBody;
    private bool isHeld = false;
    private Vector3 lastPosition;
    private Vector3 positionBeforeRelease; // Store position just before release
    private Quaternion lastRotation;
    private Vector3 velocity;
    private Vector3 angularVelocity;

    public UnityEvent OnHit;
    Vector3 triggerPosition;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Call this method when the shuriken starts being held
    public void Grab()
    {
        isHeld = true;
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    // Call this method when the shuriken is released
    public void Release()
    {
        isHeld = false;

        // Calculate the direction of movement at the moment of release
        Vector3 releaseDirection = (transform.position - positionBeforeRelease).normalized;
        releaseDirection.y = 0; // Setting Y to 0 to keep horizontal direction

        // Set the velocity to continue in the release direction
        rigidBody.velocity = releaseDirection * velocity.magnitude * 2; // Adjust the multiplier as needed

        // Add a downward force to simulate gravity
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, -9.81f, rigidBody.velocity.z);

        rigidBody.angularVelocity = Vector3.zero;
        gameObject.transform.rotation = lastRotation;
    }

    private void Update()
    {
        if (isHeld)
        {
            velocity = (transform.position - lastPosition) / Time.deltaTime;

            // Update last position and rotation for the next frame
            lastPosition = transform.position;
            lastRotation = transform.rotation;

            // Update positionBeforeRelease
            positionBeforeRelease = lastPosition;
        }
        else
        {
            angularVelocity = Vector3.zero;
        }
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
            // Simulate gravity effect when hitting non-hand objects
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, -9.81f, rigidBody.velocity.z);
        }
    }
}
