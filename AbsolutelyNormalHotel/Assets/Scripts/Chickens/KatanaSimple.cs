
using UnityEngine;

public class KatanaSimple : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chicken"))                                       
        {
            other.gameObject.GetComponent<ChickenGame>().DestroySelf();

        }
    }
}
