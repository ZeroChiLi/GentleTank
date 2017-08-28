using UnityEngine;

[ExecuteInEditMode]
public class SpringManager : MonoBehaviour
{
    public enum Axis { x, y, z }

    public Transform anchor;                        // 锚点
    public Transform spring;                        // 弹簧
    public Vector3 anchorStartPos = Vector3.zero;   // 起始锚点位置（本地）
    public Vector3 springStartScale = Vector3.one;  // 弹簧起始缩放（本地）
    public Axis movableAxis = Axis.z;               // 可移动的轴
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
        switch (movableAxis)
        {
            case Axis.x:
                anchor.localPosition = anchorStartPos + new Vector3(currentDistance, 0, 0);
                spring.localScale = springStartScale + new Vector3(scaleFactor * (anchor.localPosition.x - anchorStartPos.x), 0, 0);
                break;
            case Axis.y:
                anchor.localPosition = anchorStartPos + new Vector3(0, currentDistance, 0);
                spring.localScale = springStartScale + new Vector3(0, scaleFactor * (anchor.localPosition.y - anchorStartPos.y), 0);
                break;
            case Axis.z:
                anchor.localPosition = anchorStartPos + new Vector3(0, 0, currentDistance);
                spring.localScale = springStartScale + new Vector3(0, 0, scaleFactor * (anchor.localPosition.z - anchorStartPos.z));
                break;
            default:
                return;
        }
    }

}
