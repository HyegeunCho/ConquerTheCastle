using UnityEngine;

public class TriggerDelegator : MonoBehaviour
{
    public delegate void TriggerDelegate(Collider other);

    public event TriggerDelegate DelegateTriggerEnter;
    public event TriggerDelegate DelegateTriggerExit;
    public event TriggerDelegate DelegateTriggerStay;

    public void Clear()
    {
        DelegateTriggerEnter = null;
        DelegateTriggerExit = null;
        DelegateTriggerStay = null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        DelegateTriggerEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        DelegateTriggerExit?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        DelegateTriggerStay?.Invoke(other);
    }
}
