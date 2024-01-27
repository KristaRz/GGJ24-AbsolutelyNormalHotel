using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class EggSpawner : MonoBehaviour
{
    [SerializeField] private GameObject eggPrefab;
    private float _spawnTimer;
    private float _time;
    private bool eggSpawned;

    private void Awake()
    {
        _spawnTimer = Random.Range(7, 15);
        eggSpawned = false;
    }

    private void Update()
    {
        
        _spawnTimer += Time.deltaTime;
        
        if (!eggSpawned && _spawnTimer<_time)
        {
            //Spawn egg.
            var pos = gameObject.transform.position;
            Instantiate(eggPrefab, pos, quaternion.identity);
            eggSpawned = true;

        }
    }
}
