
using UnityEngine;

public class MoveObjectFromTo : MonoBehaviour
{
    [SerializeField] private GameObject ObjectToMove;
    [SerializeField] private Transform ObjTarget;   
    
    private Transform ObjStart;

    void Start()
    {
        ObjStart = transform;
    }
    
    public void MoveToTarget()
    {
        ObjectToMove.transform.position = ObjTarget.position;
    }
    
    public void MoveFromTarget()
    {
        ObjectToMove.transform.position = ObjStart.position;
    }

    public void BounceObj()
    {
        ObjectToMove.transform.position = ObjTarget.position;
        Invoke("MoveFromTarget", 0.4f);
    }

}
