using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AimState
{
    Normal, Friendly, Warnning, Disable
}

public class Aim : MonoBehaviour
{
    public Image stateImage;                        // 当前瞄准图片
    public Color normalColor = Color.black;         // 普通状态
    public Color friendlyColor = Color.green;       // 友好状态
    public Color warnningColor = Color.red;         // 警告状态
    public Color disableColor = Color.gray;         // 无效状态
    //public SkillLayout skillLayout;                 // 技能布局

    public AimState CurrentAimState { get { return currentAimState; } }         //获取当前瞄准状态
    public Vector3 HitPosition { get { return inputHitPos; } }                  //获取指中目标位置
    public GameObject HitGameObject { get { return inputHitGameObject; } }      //获取指中对象

    private Camera gameCamera;                      // 游戏镜头
    private AimState currentAimState;               // 当前状态
    private Vector3 inputHitPos;                    // 鼠标射线射到的点
    private GameObject inputHitGameObject;          // 射线射到的物体

    /// <summary>
    /// 获取图片组件
    /// </summary>
    private void Awake()
    {
        stateImage = GetComponent<Image>();
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        SetStateColor(normalColor);
    }

    /// <summary>
    /// 更新瞄准获取的对象值
    /// </summary>
    private void Update()
    {
        SetPos(Input.mousePosition);
        RaycastObject();
        UpdateState();
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
    /// 更新状态
    /// </summary>
    private void UpdateState()
    {
        SetState(ConvertState());
    }

    #region 设置激活状态、位置、瞄准状态

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
        //skillLayout.ContainPosition(position);
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
                SetStateColor(normalColor);
                break;
            case AimState.Friendly:
                SetStateColor(friendlyColor);
                break;
            case AimState.Warnning:
                SetStateColor(warnningColor);
                break;
            case AimState.Disable:
                SetStateColor(disableColor);
                break;
        }
    }

    /// <summary>
    /// 设置瞄准状态颜色
    /// </summary>
    /// <param name="color">瞄准颜色</param>
    private void SetStateColor(Color color)
    {
        stateImage.color = color;
    }

    #endregion

    /// <summary>
    /// 自定义状态转换
    /// </summary>
    /// <returns>返回转换后的状态</returns>
    virtual protected AimState ConvertState() { return AimState.Normal; }
}
