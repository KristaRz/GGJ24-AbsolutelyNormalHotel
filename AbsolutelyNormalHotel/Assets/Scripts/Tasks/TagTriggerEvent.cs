
using UnityEngine;
using UnityEngine.Events;

public class TagTriggerEvent : MonoBehaviour
{
    [SerializeField] private string TagName;
    [SerializeField] private UnityEvent OnTriggerEntered;
    [SerializeField] private bool DoOnce = true;

    private bool _triggeredOnce = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggeredOnce)
            enabled = false;

        if (other.CompareTag(TagName))
        {
            OnTriggerEntered.Invoke();
            if(DoOnce)_triggeredOnce = true;
        }
    }
}
