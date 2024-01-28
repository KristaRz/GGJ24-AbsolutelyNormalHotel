using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] private GameObject chickenPrefab;
    [SerializeField] private GameObject eggyolkPrefab;
    
    private float _spawnTimer = 10f;
    private float _time;
    
    private void Update()
    {
        
        _time += Time.deltaTime;
        
        if ( _spawnTimer < _time)
        {
            SpawnChicken();

        }
    }
    private void OnCollisionEnter(Collision collidedWith)
    {
        if (collidedWith.gameObject.CompareTag("Player"))
        {
            SpawnEggyolk();
        }
    }

    private void SpawnEggyolk()
    {
        //play sound of egg crushing maybe??
        Instantiate(eggyolkPrefab, gameObject.transform.position, quaternion.identity);
        Destroy(gameObject);
    }

    private void SpawnChicken()
    {
        Instantiate(chickenPrefab, gameObject.transform.position, quaternion.identity);
        Destroy(gameObject);
    }
    
}
