using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    public float coolDownTime = 1f;                 // 冷却时间
    public Slider coolDownSlider;                   // 冷却条
    public Camera mainCamera;                       // 相机，用于获取鼠标点击位置
    public Image aimImage;                          // 瞄准图片

    public Color normalColor = Color.white;         // 默认颜色
    public Color hightLightColor = Color.yellow;    // 高亮颜色
    public Color pressedColor = Color.red;          // 点击颜色
    public Color disableColor = Color.gray;         // 失效颜色

    protected Image buttonImage;                    // 按钮图片
    protected float remainReleaseTime = 0f;         // 距离下一次可以释放技能的时间，为0就是可以释放技能
    protected bool isReady = false;                 // 准备释放技能（第一次点击）
    protected bool isOnButton = false;              // 鼠标是否位于按钮上
    protected Vector3 inputHitPos;                  // 点击的位置
    protected GameObject inputHitGameObject;        // 点击的物体

    /// <summary>
    /// 初始化滑动条最大值
    /// </summary>
    private void Awake()
    {
        coolDownSlider.maxValue = coolDownTime;
        buttonImage = GetComponent<Image>();
    }

    #region 更新冷却时间、检查是否释放技能

    /// <summary>
    /// 更新冷却时间、鼠标位置、鼠标点击时释放技能
    /// </summary>
    private void Update()
    {
        UpdateCoolDown();
        if (CanRelease())
        {
            aimImage.transform.position = Input.mousePosition;
            if (Input.GetMouseButton(0))
            {
                RaycastObject(Input.mousePosition);
                Release();
            }
        }
    }

    /// <summary>
    /// 点击更新点击位置以及点击物体
    /// </summary>
    /// <param name="screenPos">点击屏幕位置</param>
    private void RaycastObject(Vector2 screenPos)
    {
        RaycastHit info;
        if (Physics.Raycast(mainCamera.ScreenPointToRay(screenPos), out info, 200))
        {
            inputHitPos = info.point;
            inputHitGameObject = info.collider.gameObject;
            Debug.Log(info.point + "  " + inputHitGameObject.name);
        }
        inputHitGameObject = null;
    }

    /// <summary>
    /// 更新冷却时间，通过Time.deltaTime改变。同时改变按钮颜色和滑动条长度
    /// </summary>
    private void UpdateCoolDown()
    {
        coolDownSlider.value = Mathf.Lerp(coolDownSlider.maxValue, coolDownSlider.minValue, remainReleaseTime / (coolDownSlider.maxValue - coolDownSlider.minValue));
        if (remainReleaseTime == 0)
            return;
        if (remainReleaseTime < 0)
        {
            buttonImage.color = normalColor;
            remainReleaseTime = 0;
            return;
        }
        remainReleaseTime -= Time.deltaTime;
        buttonImage.color = disableColor;
    }

    #endregion

    #region 鼠标点击、进入、离开，准备状态、取消准备

    /// <summary>
    /// 点击技能时响应
    /// </summary>
    public void OnClicked()
    {
        isReady = !isReady;
        if (isReady)
            Ready();
        else
            Cancel();
    }

    /// <summary>
    /// 鼠标移到按钮时响应
    /// </summary>
    public void OnEnter()
    {
        isOnButton = true;
        if (isReady)
        {
            aimImage.gameObject.SetActive(false);
            return;
        }
        buttonImage.color = hightLightColor;
    }

    /// <summary>
    /// 鼠标离开按钮时响应
    /// </summary>
    public void OnExit()
    {
        isOnButton = false;
        if (isReady)
        {

            aimImage.gameObject.SetActive(true);
            return;
        }
        buttonImage.color = normalColor;
    }

    /// <summary>
    /// 准备释放技能状态
    /// </summary>
    public void Ready()
    {
        aimImage.gameObject.SetActive(true);
        buttonImage.color = pressedColor;
    }

    /// <summary>
    /// 取消准备释放技能状态
    /// </summary>
    public void Cancel()
    {
        aimImage.gameObject.SetActive(false);
        buttonImage.color = normalColor;
    }

    #endregion

    /// <summary>
    /// 是否可释放技能
    /// </summary>
    /// <returns>返回True，可以释放技能</returns>
    virtual public bool CanRelease()
    {
        return remainReleaseTime <= 0f && isReady && !isOnButton;
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    virtual public void Release()
    {
        remainReleaseTime = coolDownSlider.maxValue;
        isReady = false;
        Cancel();
        SkillEffect();
    }

    /// <summary>
    /// 技能效果
    /// </summary>
    abstract public void SkillEffect();
}
