using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    public float coolDownTime = 1f;                 // 冷却时间
    public Slider coolDownSlider;                   // 冷却条

    public Color normalColor = Color.white;         // 默认颜色
    public Color hightLightColor = Color.yellow;    // 高亮颜色
    public Color pressedColor = Color.red;          // 点击颜色
    public Color disableColor = Color.gray;         // 失效颜色

    protected Image buttonImage;                    // 按钮图片
    protected float remainReleaseTime = 0f;         // 距离下一次可以释放技能的时间，为0就是可以释放技能

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
    /// 点击技能时响应
    /// </summary>
    virtual public void OnClicked()
    {
        if (CanRelease())
            Release();
    }

    /// <summary>
    /// 鼠标移到按钮时响应
    /// </summary>
    virtual public void OnEnter()
    {
        buttonImage.color = hightLightColor;
    }

    /// <summary>
    /// 鼠标离开按钮是响应
    /// </summary>
    virtual public void OnExit()
    {
        buttonImage.color = normalColor;
    }

    /// <summary>
    /// 更新冷却时间，通过Time.deltaTime改变。同时改变按钮颜色和滑动条长度
    /// </summary>
    virtual public void UpdateCoolDown()
    {
        coolDownSlider.value = Mathf.Lerp(coolDownSlider.maxValue, coolDownSlider.minValue, remainReleaseTime /(coolDownSlider.maxValue - coolDownSlider.minValue));
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

    /// <summary>
    /// 是否可释放技能
    /// </summary>
    /// <returns>返回True，可以释放技能</returns>
    virtual public bool CanRelease()
    {
        return remainReleaseTime <= 0f;
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    virtual public void Release()
    {
        remainReleaseTime = coolDownSlider.maxValue;
        SkillEffect();
    }

    /// <summary>
    /// 技能效果
    /// </summary>
    abstract public void SkillEffect();
}
