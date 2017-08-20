
using CameraRig;
using UnityEngine;
using UnityEngine.UI;

public class MinimapWitchCamera : MonoBehaviour 
{
    public Image rotatedBorder;                         // 小地图的可旋转外边
    public FixedTargetFollower fixedTargetFollow;       // 跟随镜头

    /// <summary>
    /// 旋转小地图外边
    /// </summary>
    private void FixedUpdate()
    {
        if (fixedTargetFollow.Target == null)
            return;
        rotatedBorder.rectTransform.rotation = Quaternion.Euler(0, 0, fixedTargetFollow.Target.transform.eulerAngles.y);
    }

    /// <summary>
    /// 设置目标
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        fixedTargetFollow.SetTarget(target);
    }

    /// <summary>
    /// 设置小地图激活状态
    /// </summary>
    /// <param name="active"></param>
    public void SetMinimapActive(bool active)
    {
        fixedTargetFollow.gameObject.SetActive(active);
        gameObject.SetActive(active);
    }
}
