using Item.Ammo;
using UnityEngine;

[ExecuteInEditMode]
public class SpringBoxingGloveManager : MonoBehaviour
{
    public BoxCollider glove;                       // 拳套碰撞体
    public BoxingAmmo ammo;                         // 拳套弹药
    public Transform spring;                        // 弹簧
    public float scaleFactor = 200f;                // 弹簧缩放随锚点位置变化的缩放因子
    public float minDistance = 0f;                  // 最小距离
    public float maxDistance = 5f;                  // 最大距离
    [Range(0, 1)]
    public float fillAmount = 0.5f;                 // 距离插值

    public float CurrentDistance { get { return currentDistance; } }
    private float currentDistance;                  // 当前距离

    /// <summary>
    /// 更新当前距离，弹簧缩放，锚点位置
    /// </summary>
    private void LateUpdate()
    {
        currentDistance = Mathf.Lerp(minDistance, maxDistance, fillAmount);
        UpdateAnchorPosAndSpringScale();
    }

    /// <summary>
    /// 更新锚点位置和弹簧缩放
    /// </summary>
    private void UpdateAnchorPosAndSpringScale()
    {
        glove.transform.localPosition = new Vector3(0, 0, currentDistance);
        spring.localScale = new Vector3(0, 0, scaleFactor * glove.transform.localPosition.z);
    }

}
