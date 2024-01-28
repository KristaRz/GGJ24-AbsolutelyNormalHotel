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
    

    private void Awake()
    {
        _spawnTimer = Random.Range(7, 10);
    }

    private void Update()
    {
        
        _time += Time.deltaTime;
        
        if ( _spawnTimer < _time)
        {
            //Spawn egg.
            print("egg spawned");
            var pos = gameObject.transform.position;
            Instantiate(eggPrefab, pos, quaternion.identity);
            _time = 0;
            _spawnTimer = Random.Range(3, 7);

        }
    }
}
