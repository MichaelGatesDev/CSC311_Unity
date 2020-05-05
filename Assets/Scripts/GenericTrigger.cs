using UnityEngine;
using UnityEngine.Events;

public class GenericTrigger : MonoBehaviour
{
    public UnityEvent TestEvent;
    public GameObject Target;

    private void OnTriggerEnter(Collider other)
    {
        if (!Target || other.gameObject == Target)
            TestEvent.Invoke();
    }
}