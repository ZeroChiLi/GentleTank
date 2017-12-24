using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class PropBase : MonoBehaviour 
{
    public delegate void PropEventHandle(PropBase prop);
    public event PropEventHandle OnTouchFinishedEvent;

    protected void OnTriggerEnter(Collider other)
    {
        PlayerManager target = other.GetComponentInParent<PlayerManager>();
        if (target == null)
            return;
        if (OnPlayerTouch(target))
            OnTouchFinishedEvent(this);
    }

    protected abstract bool OnPlayerTouch(PlayerManager player);

}
