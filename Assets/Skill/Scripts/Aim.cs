using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AimState
{
    Normal, Friendly, Warnning
}

public class Aim : MonoBehaviour
{
    public Sprite normal;                           // 普通状态
    public Sprite friendly;                         // 友好状态
    public Sprite warnning;                         // 警告状态

    public AimState CurrentAimState { get { return currentAimState; } }

    private AimState currentAimState;               // 当前状态
    private Image currentImage;                     // 当前瞄准图片

    /// <summary>
    /// 获取图片组件
    /// </summary>
    private void Awake()
    {
        currentImage = GetComponent<Image>();
    }

    /// <summary>
    /// 设置对象激活状态
    /// </summary>
    /// <param name="active">是否激活</param>
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    /// <summary>
    /// 设置瞄准图片位置
    /// </summary>
    /// <param name="position">位置</param>
    public void SetPos(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    /// <summary>
    /// 设置瞄准状态
    /// </summary>
    /// <param name="aimState">瞄准状态</param>
    public void SetState(AimState aimState)
    {
        if (aimState == CurrentAimState)
            return;
        currentAimState = aimState;
        switch (aimState)
        {
            case AimState.Normal:
                SetCurrentImage(normal);
                break;
            case AimState.Friendly:
                SetCurrentImage(friendly);
                break;
            case AimState.Warnning:
                SetCurrentImage(warnning);
                break;
        }
    }

    /// <summary>
    /// 设置当前瞄准图片
    /// </summary>
    /// <param name="sprite">瞄准图片精灵</param>
    private void SetCurrentImage(Sprite sprite)
    {
        currentImage.sprite = sprite;
    }
}
