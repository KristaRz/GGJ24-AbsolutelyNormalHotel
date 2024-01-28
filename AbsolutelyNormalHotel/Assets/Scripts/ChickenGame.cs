
using UnityEngine.Events;
using UnityEngine;

public class ChickenGame : MonoBehaviour
{
    public float speed = 1;
    public float initialSpeed = 0.5f;
    public float speedIncreaseRate = 0.1f;
    public float jumpForce = 5;
    public float glideForce = 0.5f;
    private Rigidbody rb;

    public float groundCheckDistance = 0.5f;

    private float jumpTimer;
    private float nextJumpTime;

    private bool isGrounded;
    private bool isFirstGrounded = true;

    public UnityEvent OnCollision;
    public UnityEvent OnDeath;


    Vector3 collisionPosition;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ResetJumpTimer();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        isGrounded = IsGrounded();
        float currentSpeed = isFirstGrounded ? initialSpeed : speed;
        transform.localPosition += transform.forward * currentSpeed * Time.deltaTime;

        if (isFirstGrounded && currentSpeed < speed)
        {
            initialSpeed += speedIncreaseRate * Time.deltaTime;
        }

        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0 && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isFirstGrounded = false;
            ResetJumpTimer();
        }

        ApplyAirForces();
    }

    private void ResetJumpTimer()
    {
        nextJumpTime = Random.Range(1f, 2f);
        jumpTimer = nextJumpTime;
    }

    private void ApplyAirForces()
    {
        if (!isGrounded)
        {
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.up * -glideForce);
            }
            else if (rb.velocity.y < 0)
            {
                EaseDescent();
            }
        }
    }

    private void EaseDescent()
    {
        float easeFactor = CalculateEaseFactor();
        Vector3 newVelocity = rb.velocity;
        newVelocity.y *= easeFactor;
        rb.velocity = newVelocity;
    }

    private float CalculateEaseFactor()
    {
        float speed = Mathf.Abs(rb.velocity.y);
        float maxSpeed = jumpForce;
        return 1 - Mathf.Clamp01(speed / maxSpeed);
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance)
            && hit.collider.CompareTag("Ground");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Ground")
        {
            Vector3 hitNormal = collision.contacts[0].normal;
            hitNormal.y = 0; // Flatten the normal to the horizontal plane

            collisionPosition = collision.contacts[0].point;

            // Calculate the new direction on the horizontal plane
            Vector3 newDirection = Vector3.Reflect(transform.forward, hitNormal).normalized;

            // Set the new rotation
            transform.rotation = Quaternion.LookRotation(newDirection);

            OnCollision.Invoke();
        }
    }

    public void InstantiatePrefab(GameObject prefab)
    {

        Instantiate(prefab, collisionPosition, Quaternion.identity);
    }

    public void DestroySelf()
    {
        OnDeath.Invoke();
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
