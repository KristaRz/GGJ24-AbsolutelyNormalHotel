using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private Rigidbody rigidBody;
    private bool isHeld = false;
    private Vector3 lastPosition;
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
        rigidBody.useGravity = true;
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    // Call this method when the shuriken is released
    public void Release()
    {
        isHeld = false;
        rigidBody.useGravity = false;
        rigidBody.velocity = velocity*2;


        rigidBody.angularVelocity = Vector3.zero;
        gameObject.transform.rotation = lastRotation;
    }

    private void Update()
    {
        if (isHeld)
        {
            // Calculate velocity and angular velocity
            velocity = (transform.position - lastPosition) / Time.deltaTime;
            //angularVelocity = (transform.rotation.eulerAngles - lastRotation.eulerAngles) / Time.deltaTime;

            // Update last position and rotation for the next frame
            lastPosition = transform.position;
            lastRotation = transform.rotation;
        }else
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
        // Approximate the hit point as the closest point on the bounds of the collider
        triggerPosition = other.ClosestPoint(transform.position);


        if (other.CompareTag("Chicken"))
        {
            OnHit.Invoke();
            // Destroy chicken
            other.gameObject.GetComponent<ChickenGame>().DestroySelf();
        }
        if (!other.CompareTag("Hand") &&    other.name != "Direct Interactor" )
        {

            rigidBody.useGravity = true;
        }

    }
}
