
using UnityEngine;
using Random = UnityEngine.Random;


public class EggSpawner : MonoBehaviour
{
    [SerializeField] private GameObject eggPrefab;



    public float spawnProbability = 0.5f; // 50% chance to spawn an egg

    public void SpawnEgg()
    {
        // Check if an egg should be spawned based on probability.
        if (Random.value < spawnProbability)
        {
            Instantiate(eggPrefab, gameObject.transform.position, Quaternion.identity);
        }
    }


    public void DestroyThisComponent()
    {
        Destroy(this);
    }
}
