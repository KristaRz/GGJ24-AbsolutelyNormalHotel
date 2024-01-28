using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
            print("egg spawned");
            var pos = gameObject.transform.position;
            Instantiate(eggPrefab, pos, Quaternion.identity);
        }
        else
        {
            print("no egg spawned this time");
        }

    }


    public void DestroyThisComponent()
    {
        Destroy(this);
    }
}
