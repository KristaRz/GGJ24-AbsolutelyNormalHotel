using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shruiken : MonoBehaviour
{
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collidedWith)
    {
        
        if (collidedWith.gameObject.tag == "Chicken")
        {
            // destroy chicken
            collidedWith.gameObject.GetComponent<ChickenGame>().DestroySelf();

        }
    }
}
