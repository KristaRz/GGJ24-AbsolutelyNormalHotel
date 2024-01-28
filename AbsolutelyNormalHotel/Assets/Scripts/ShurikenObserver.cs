using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class ShurikenReload : MonoBehaviour
{
    public GameObject shurikenPrefab; // Prefab to instantiate
    public float instantiationDelay = 2f; // Delay in seconds before instantiation
    public UnityEngine.Events.UnityEvent onShurikenInstantiated; // Event to invoke after instantiation

    public UnityEvent OnTake;


    public void InstantiateShurikenWithDelay()
    {
        Invoke("InstantiateShurikenWithDelay", instantiationDelay);

    }


    private void Instantiate()
    {

        Instantiate(shurikenPrefab, transform.position, Quaternion.identity, transform);
        onShurikenInstantiated.Invoke(); // Invoke the event after instantiation

    }
}