
using UnityEngine;
using UnityEngine.Events;

public class Egg : MonoBehaviour
{
    [SerializeField] private GameObject chickenPrefab;
    [SerializeField] private GameObject eggyolkPrefab;
    public Collider EggCollider;

    public float _spawnTimer = 3f;
    private float _time;

    public UnityEvent onSpawn;

    private bool _countTime = true;

    private void Update()
    {
        if (!_countTime) return;

        _time += Time.deltaTime;

        if (_spawnTimer < _time)
        {
            onSpawn.Invoke();
        }
    }
   
    private void OnTriggerEnter(Collider collidedWith)
    {
        if (collidedWith.gameObject.CompareTag("Player"))
        {
            SpawnEggyolk();
            Debug.Log("Collided with player");
            _countTime = false;
        }
    }

    private void OnCollisionEnter(Collision collidedWith)
    {
        if (collidedWith.gameObject.CompareTag("Player"))
        {
            SpawnEggyolk();
            Debug.Log("Collided with player");
            _countTime = false;
        }
        if (collidedWith.gameObject.CompareTag("Ground"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            EggCollider.enabled = false;
        }
    }

    private void SpawnEggyolk()
    {
        // Instantiate the eggyolk and set this as the parent
        Instantiate(eggyolkPrefab, transform.position, Quaternion.identity, transform);
        GetComponent<MeshRenderer>().enabled = false;
        Debug.Log("Spawning eggyolk");
        // Destroy the current gameObject
        Invoke("DestroyThis", 1f); 
    }

    private void DestroyThis() { Destroy(gameObject); Debug.Log("Destroying egg"); }

    public void InstantiatePrefab(GameObject prefab)
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
    }

    public void SpawnChicken(GameObject prefab)
    {
        // Generate a random Y rotation
        float randomYRotation = UnityEngine.Random.Range(0f, 360f);
        Quaternion randomRotation = Quaternion.Euler(0, randomYRotation, 0);

        // Instantiate the chicken with random Y rotation
        GameObject chicken = Instantiate(prefab, gameObject.transform.position + new Vector3(0, 0, -0.2f), randomRotation);

        // Add explosion force if Rigidbody is present
        Rigidbody chickenRigidbody = chicken.GetComponent<Rigidbody>();
        if (chickenRigidbody != null)
        {
            Vector3 explosionPosition = transform.position + new Vector3(0, 0, -0.2f);
            chickenRigidbody.AddExplosionForce(2, explosionPosition, 1, 1, ForceMode.Impulse);
        }

        // Destroy the current gameObject
        Destroy(gameObject);
    }


}
