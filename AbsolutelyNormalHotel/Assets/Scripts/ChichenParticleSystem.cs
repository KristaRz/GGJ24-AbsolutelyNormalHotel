using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChichenParticleSystem : MonoBehaviour 
{
    public GameObject particleSys1;
    public GameObject particleSys2;

    private IEnumerator coroutine;
    
    void Awake()
    {
        
    }

   
    private IEnumerator PlayParticleSytems()
    {
        particleSys2.SetActive(true);
        yield return StartCoroutine(WaitToPlayNext(2f));
        particleSys1.SetActive(true);
    }
    
    private IEnumerator WaitToPlayNext(float waitTime)
    {
      
            yield return new WaitForSeconds(waitTime);
            //print("WaitAndPrint " + Time.time);
        
    }
}
