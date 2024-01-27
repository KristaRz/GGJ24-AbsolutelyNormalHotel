
using UnityEngine;
using UnityEngine.Events;

public class TagTriggerEvent : MonoBehaviour
{
    [SerializeField] private string TagName;
    [SerializeField] private UnityEvent OnTriggerEntered;

    private bool _triggeredOnce = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggeredOnce)
            enabled = false;

        if (other.CompareTag(TagName))
        {
            OnTriggerEntered.Invoke();
            _triggeredOnce = true;
        }
    }
}
