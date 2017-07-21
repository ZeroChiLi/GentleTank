using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour 
{
    public Slider slider;                           // 滑动条
    public Image fullImage;                         // 滑动条图片
    public Image emptyImage;                        // 滑动条背景图片
    public Skill skill;                             // 技能

    protected float remainReleaseTime = 0f;         // 距离下一次可以释放技能的时间，为0就是可以释放技能
    protected bool isReady = false;                 // 是否准备释放技能（第一次点击）
    protected bool gamePlaying = false;             // 是否正在游戏中

    private Image buttonImage;                                      // 按钮图片
    private Color normalColor = Color.white;                        // 默认颜色
    private Color hightLightColor = new Color(1, 1, 0.45f);         // 高亮颜色
    private Color pressedColor = new Color(1, 0.27f, 0.27f);        // 点击颜色
    private Color disableColor = new Color(0.46f, 0.46f, 0.46f);    // 失效颜色
    private Coroutine currentSkillCoroutine;                         // 当前技能释放时的协程

    /// <summary>
    /// 初始化技能管理器
    /// </summary>
    public void InitSkillManager(Skill skill,Sprite fullSprite,Sprite emptySprite)
    {
        this.skill = skill;
        fullImage.sprite = fullSprite;
        emptyImage.sprite = emptySprite;
        buttonImage = GetComponent<Image>();
        slider.maxValue = skill.coolDownTime;
        remainReleaseTime = skill.coolDownTime;
        buttonImage.color = disableColor;
        SetupButtonEvent();
    }

    /// <summary>
    /// 配置按钮事件（鼠标进入，离开）
    /// </summary>
    public void SetupButtonEvent()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry();               // 鼠标进入事件
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { MouseOnButton(true); });
        eventTrigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry();                // 鼠标离开事件
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { MouseOnButton(false); });
        eventTrigger.triggers.Add(entryExit);
    }

    /// <summary>
    /// 鼠标移动到按钮或移出按钮时响应
    /// </summary>
    /// <param name="enter">true进入，否则是移出</param>
    public void MouseOnButton(bool enter)
    {
        if (remainReleaseTime > 0 || isReady)       //如果还在冷却时间，保持灰色；如果还在准备状态，保持高亮。
            return;

        // 进入高亮，出去正常
        if (enter)
            buttonImage.color = hightLightColor;
        else
            buttonImage.color = normalColor;
    }

    /// <summary>
    /// 更新冷却时间，通过Time.deltaTime改变。同时改变按钮颜色和滑动条长度
    /// </summary>
    public void UpdateCoolDown()
    {
        if (remainReleaseTime == 0)
            return;
        if (remainReleaseTime < 0)
        {
            buttonImage.color = normalColor;
            remainReleaseTime = 0;
            slider.value = slider.maxValue;
            return;
        }
        slider.value = Mathf.Lerp(slider.maxValue, slider.minValue, remainReleaseTime / (slider.maxValue - slider.minValue));
        remainReleaseTime -= Time.deltaTime;
        buttonImage.color = disableColor;
    }

    /// <summary>
    /// 是否可以进入准备状态，冷却时间过了
    /// </summary>
    /// <returns></returns>
    public bool CanReady()
    {
        return remainReleaseTime == 0;
    }

    /// <summary>
    /// 准备释放技能状态
    /// </summary>
    public void Ready()
    {
        isReady = true;
        buttonImage.color = pressedColor;
    }

    /// <summary>
    /// 取消准备释放技能状态
    /// </summary>
    public void Cancel()
    {
        isReady = false;
        buttonImage.color = normalColor;
    }

    /// <summary>
    /// 设置技能能否使用
    /// </summary>
    /// <param name="enable">是否激活技能</param>
    /// <returns>返回是否激活</returns>
    public void SetSkillEnable(bool enable)
    {
        if (enable)
            buttonImage.color = normalColor;
        else if (!enable)
        {
            slider.value = slider.minValue;
            remainReleaseTime = skill.coolDownTime;
            buttonImage.color = disableColor;
            isReady = false;
            Stop();             // 停止掉技能协程
        }
    }

    /// <summary>
    /// 是否可释放技能（冷却时间结束，已经点击了技能，鼠标不在按钮上面）
    /// </summary>
    /// <returns>返回True，可以释放技能</returns>
    public bool CanRelease()
    {
        return remainReleaseTime <= 0f && isReady && skill.ReleaseCondition();
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    public void Release()
    {
        remainReleaseTime = slider.maxValue;
        isReady = false;
        Cancel();
        currentSkillCoroutine = StartCoroutine(skill.SkillEffect());
    }

    /// <summary>
    /// 终止当前技能释放
    /// </summary>
    public void Stop()
    {
        if (currentSkillCoroutine != null)
            StopCoroutine(currentSkillCoroutine);
    }

}
