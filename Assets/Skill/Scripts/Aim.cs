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
    public Camera gameCamera;                       // 游戏镜头
    public Sprite normal;                           // 普通状态
    public Sprite friendly;                         // 友好状态
    public Sprite warnning;                         // 警告状态

    public AimState CurrentAimState { get { return currentAimState; } }         //获取当前瞄准状态
    public Vector3 HitPosition { get { return inputHitPos; } }                  //获取指中目标位置
    public GameObject HitGameObject { get { return inputHitGameObject; } }      //获取指中对象

    private AimState currentAimState;               // 当前状态
    private Image currentImage;                     // 当前瞄准图片
    private Vector3 inputHitPos;                    // 鼠标射线射到的点
    private GameObject inputHitGameObject;          // 射线射到的物体

    /// <summary>
    /// 获取图片组件
    /// </summary>
    private void Awake()
    {
        currentImage = GetComponent<Image>();
    }

    /// <summary>
    /// 更新瞄准获取的对象值
    /// </summary>
    private void Update()
    {
        SetPos(Input.mousePosition);
        RaycastObject();
    }

    /// <summary>
    /// 以鼠标位置从屏幕射线
    /// </summary>
    private void RaycastObject()
    {
        RaycastHit info;
        if (Physics.Raycast(gameCamera.ScreenPointToRay(gameObject.transform.position), out info, 200))
        {
            inputHitPos = info.point;
            inputHitGameObject = info.collider.gameObject;
        }
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
