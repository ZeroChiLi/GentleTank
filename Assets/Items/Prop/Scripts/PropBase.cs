using System.Collections;
using UnityEngine;

public abstract class PropBase : MonoBehaviour
{
    public new Collider collider;           // 碰撞体
    public MeshRenderer meshRenderer;       // 渲染网格
    public Effect touchEffect;              // 碰撞有效后特效

    public delegate void PropEventHandle(PropBase prop);
    public event PropEventHandle OnTouchFinishedEvent;

    /// <summary>
    /// 初始化：开启碰撞体、渲染网格、关掉碰撞特效
    /// </summary>
    protected void OnEnable()
    {
        collider.enabled = true;
        meshRenderer.gameObject.SetActive(true);
        touchEffect.gameObject.SetActive(false);
    }

    protected void OnTriggerEnter(Collider other)
    {
        PlayerManager target = other.GetComponentInParent<PlayerManager>();
        if (target == null)
            return;
        if (OnPlayerTouch(target))
        {
            StartCoroutine(InactiveAndShowEffect());
            OnTouchFinishedEvent(this);
        }
    }

    /// <summary>
    /// 玩家（包含PlayerManager）碰撞时响应，返回值为是否碰撞有效
    /// </summary>
    protected abstract bool OnPlayerTouch(PlayerManager player);

    /// <summary>
    /// 失活道具，显示特效
    /// </summary>
    protected IEnumerator InactiveAndShowEffect()
    {
        if (touchEffect != null)
        {
            collider.enabled = false;
            meshRenderer.gameObject.SetActive(false);
            touchEffect.gameObject.SetActive(true);
            while (touchEffect.isActiveAndEnabled)
                yield return null;
        }
        gameObject.SetActive(false);
    }
}
