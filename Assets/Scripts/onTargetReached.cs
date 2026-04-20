using UnityEngine;
using UnityEngine.Events;

public class onTargetReached : MonoBehaviour
{
    public float threshhold = 0.02f;
    public Transform target;
    public UnityEvent onTargetReachedEvent;
    private bool hasReached = true;
    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < threshhold && !hasReached)
        {
            onTargetReachedEvent.Invoke();
            hasReached = true;
        }
        else if (distance >= threshhold)
        {
            hasReached = false;
        }
    }
}
