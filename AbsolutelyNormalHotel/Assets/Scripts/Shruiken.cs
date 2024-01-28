using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Shuriken : MonoBehaviour
{
    // Variables for easing the descent
    private float maxDescentSpeed = 9.81f; // Maximum descent speed

    private Rigidbody rigidBody;
    private bool isHeld = false;
    private bool isFreeFalling = false;
    private Vector3 lastPosition;
    private Vector3 positionBeforeRelease;
    private Quaternion lastRotation;
    private Vector3 velocity;
    private Vector3 angularVelocity;
    public Animator anim;

    public float scaleDownInSec = 1.0f;
    public float scaleDuration = 2.0f;
    public UnityEvent OnHit;
    public UnityEvent OnReset;
    private Vector3 triggerPosition;

    private Vector3 initialPosition; // Store the initial position
    private Quaternion initialRotation; // Store the initial rotation

    private GameObject parentObject; // Store the parent GameObject

    private void Awake()
    {
        parentObject = transform.parent.gameObject;

        rigidBody = GetComponent<Rigidbody>();
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    public void ResetObject()
    {
        // Restore the original parent
        transform.parent = parentObject.transform;

        // Restore the initial local position and rotation
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;

        OnReset.Invoke();
        transform.localScale = Vector3.one;
        rigidBody.isKinematic = true;
        gameObject.SetActive(true);
    }


    public void Grab()
    {
        isHeld = true;
        isFreeFalling = false;
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    public void Release()
    {
        rigidBody.isKinematic = false;
        anim.SetBool("Spin", true);
        isHeld = false;
        isFreeFalling = true;

        Vector3 releaseDirection = (transform.position - positionBeforeRelease).normalized;
        releaseDirection.y = 0;
        rigidBody.velocity = releaseDirection * velocity.magnitude * 2;
        rigidBody.angularVelocity = angularVelocity;
        gameObject.transform.rotation = lastRotation;
        transform.parent = null;
    }

    private void Update()
    {
        if (isHeld)
        {
            velocity = (transform.position - lastPosition) / Time.deltaTime;
            angularVelocity = rigidBody.angularVelocity;
            lastPosition = transform.position;
            lastRotation = transform.rotation;
            positionBeforeRelease = lastPosition;
        }
        else if (isFreeFalling)
        {
            EaseDescent();
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
    public void InstantiatePrefabObjPos(GameObject prefab)
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
            rigidBody.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Platform" && !collision.gameObject.CompareTag("Shuriken") && !collision.gameObject.CompareTag("Chicken"))
        {
            Collider triggerCollider = GetComponent<Collider>();
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }

            anim.SetBool("Spin", false);
            isFreeFalling = false;
            StartCoroutine(ScaleDownAndDestroy());
        }
    }

    private IEnumerator ScaleDownAndDestroy()
    {
        yield return new WaitForSeconds(scaleDownInSec);
        float elapsed = 0;
        Vector3 originalScale = transform.localScale;

        if (transform.localScale.x > 0.1f && transform.localScale.y > 0.1f && transform.localScale.z > 0.1f)
        {
            elapsed += Time.deltaTime;
            float blend = Mathf.Clamp01(elapsed / scaleDuration);
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, blend);
            yield return null;
        }

        StopCoroutine("ScaleDownAndDestroy");
        ResetObject();
    }
}
