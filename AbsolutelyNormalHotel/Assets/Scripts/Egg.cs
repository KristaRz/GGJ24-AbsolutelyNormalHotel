using System;
using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Events;

public class Egg : MonoBehaviour
{
    [SerializeField] private GameObject chickenPrefab;
    [SerializeField] private GameObject eggyolkPrefab;
    
    public float _spawnTimer = 10f;
    private float _time;

    public UnityEvent onSpawn;

    private void Update()
    {
        
        _time += Time.deltaTime;
        
        if ( _spawnTimer < _time)
        {
            onSpawn.Invoke();


        }
    }


    //private void OnCollisionEnter(Collision collidedWith)
    //{
    //    if (collidedWith.gameObject.CompareTag("Player"))
    //    {
    //        SpawnEggyolk();
    //    }
    //}

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
