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

    /// <summary>
    /// 初始化滑动条最大值
    /// </summary>
    private void Awake()
    {
        coolDownSlider.maxValue = coolDownTime;
        buttonImage = GetComponent<Image>();
    }

    /// <summary>
    /// 更新冷却时间和滑动条位置
    /// </summary>
    private void Update()
    {
        UpdateCoolDown();
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

    #region 鼠标点击、进入、离开

    /// <summary>
    /// 点击技能时响应
    /// </summary>
    public void OnClicked()
    {
        isReady = !isReady;
        if (isReady)
        {
            aimImage.gameObject.SetActive(true);
            buttonImage.color = pressedColor;
        }
        else
        {
            aimImage.gameObject.SetActive(false);
            buttonImage.color = normalColor;
        }
    }

    /// <summary>
    /// 鼠标移到按钮时响应
    /// </summary>
    public void OnEnter()
    {
        if (isReady)
            return;
        buttonImage.color = hightLightColor;
    }

    /// <summary>
    /// 鼠标离开按钮时响应
    /// </summary>
    public void OnExit()
    {
        if (isReady)
            return;
        buttonImage.color = normalColor;
    }

    #endregion

    /// <summary>
    /// 是否可释放技能
    /// </summary>
    /// <returns>返回True，可以释放技能</returns>
    virtual public bool CanRelease()
    {
        return remainReleaseTime <= 0f && isReady;
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    virtual public void Release()
    {
        remainReleaseTime = coolDownSlider.maxValue;
        isReady = false;
        SkillEffect();
    }

    /// <summary>
    /// 技能效果
    /// </summary>
    abstract public void SkillEffect();
}
