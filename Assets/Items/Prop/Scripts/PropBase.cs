using UnityEngine;

public abstract class PropBase : MonoBehaviour 
{
    protected void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        PlayerManager target = other.GetComponentInParent<PlayerManager>();
        if (target == null)
            return;
        OnPlayerTouch(target);
    }

    protected abstract void OnPlayerTouch(PlayerManager player);

}
