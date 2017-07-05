using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    public float coolDownTime = 1f;                 // 冷却时间
    public Slider coolDownSlider;                   // 冷却条
    public Aim aim;                                 // 瞄准操作

    protected Image buttonImage;                    // 按钮图片
    protected float remainReleaseTime = 0f;         // 距离下一次可以释放技能的时间，为0就是可以释放技能
    protected bool isReady = false;                 // 是否准备释放技能（第一次点击）
    protected bool gamePlaying = false;             // 是否正在游戏中

    private Color normalColor = Color.white;                        // 默认颜色
    private Color hightLightColor = new Color(1, 1, 0.45f);         // 高亮颜色
    private Color pressedColor = new Color(1, 0.27f, 0.27f);        // 点击颜色
    private Color disableColor = new Color(0.46f, 0.46f, 0.46f);    // 失效颜色

    /// <summary>
    /// 初始化滑动条最大值
    /// </summary>
    private void Awake()
    {
        coolDownSlider.maxValue = coolDownTime;
        remainReleaseTime = coolDownTime;
        buttonImage = GetComponent<Image>();
        buttonImage.color = Color.white;
    }

    #region 更新冷却时间、检查是否在游戏进行回合状态，并设置技能是否可用

    /// <summary>
    /// 更新冷却时间、鼠标位置、鼠标点击时释放技能
    /// </summary>
    private void Update()
    {
        if (!GameRoundPlaying())        //游戏没开始就直接return
            return;
        UpdateCoolDown();               //更新冷却时间
    }

    /// <summary>
    /// 根据游戏是否开始，来设置技能能否使用，返回是否可以使用（是否正在游戏）
    /// </summary>
    /// <returns>目前游戏回合状态</returns>
    private bool GameRoundPlaying()
    {
        if (GameRecord.Instance.CurrentGameState == GameState.Playing)
            return SetSkillEnableByGamePlaying(true);
        return SetSkillEnableByGamePlaying(false);
    }

    /// <summary>
    /// 只有当gamePlaying 和 isPlaying 不同时才会改变技能是否有效。即只有从 开始到进行触发一次，进行到结束触发一次
    /// </summary>
    /// <param name="isPlaying">是否正在游戏</param>
    /// <returns>返回参数</returns>
    private bool SetSkillEnableByGamePlaying(bool isPlaying)
    {
        if (gamePlaying != isPlaying)
        {
            gamePlaying = isPlaying;
            SetSkillEnable(isPlaying);
        }
        return isPlaying;
    }

    /// <summary>
    /// 更新冷却时间，通过Time.deltaTime改变。同时改变按钮颜色和滑动条长度
    /// </summary>
    private void UpdateCoolDown()
    {
        if (remainReleaseTime == 0)
            return;
        if (remainReleaseTime < 0)
        {
            buttonImage.color = normalColor;
            remainReleaseTime = 0;
            coolDownSlider.value = coolDownSlider.maxValue;
            return;
        }
        coolDownSlider.value = Mathf.Lerp(coolDownSlider.maxValue, coolDownSlider.minValue, remainReleaseTime / (coolDownSlider.maxValue - coolDownSlider.minValue));
        remainReleaseTime -= Time.deltaTime;
        buttonImage.color = disableColor;
    }

    #endregion

    /// <summary>
    /// 鼠标移动到按钮或移出按钮时响应
    /// </summary>
    /// <param name="enter">true进入，否则是移出</param>
    public void OnMouseOnButton(bool enter)
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
        aim.SetActive(true);
        aim.SetPos(Input.mousePosition);        // 重新改变鼠标瞄准位置（因为在按钮上鼠标是失效的）
        buttonImage.color = pressedColor;
    }

    /// <summary>
    /// 取消准备释放技能状态
    /// </summary>
    public void Cancel()
    {
        isReady = false;
        aim.SetActive(false);
        buttonImage.color = normalColor;
    }

    /// <summary>
    /// 设置技能能否使用
    /// </summary>
    /// <param name="enable">是否激活技能</param>
    /// <returns>返回是否激活</returns>
    public bool SetSkillEnable(bool enable)
    {
        if (enable)
            buttonImage.color = normalColor;
        else
        {
            coolDownSlider.value = coolDownSlider.minValue;
            remainReleaseTime = coolDownTime;
            buttonImage.color = disableColor;
            if (isReady)
            {
                isReady = false;
                aim.SetActive(false);
            }
            //Debug.Log("Stop SkillEffect");
            //StopCoroutine(SkillEffect());
        }
        return enabled;
    }

    /// <summary>
    /// 是否可释放技能（冷却时间结束，已经点击了技能，鼠标不在按钮上面）
    /// </summary>
    /// <returns>返回True，可以释放技能</returns>
    public bool CanRelease()
    {
        return remainReleaseTime <= 0f && isReady && ReleaseCondition();
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    public void Release()
    {
        remainReleaseTime = coolDownSlider.maxValue;
        isReady = false;
        Cancel();
        StartCoroutine(SkillEffect());
    }

    /// <summary>
    /// 自定义条件判断是否释放技能
    /// </summary>
    /// <returns>返回是否满足自定义条件</returns>
    virtual public bool ReleaseCondition()
    { return true; }

    /// <summary>
    /// 技能效果
    /// </summary>
    abstract public IEnumerator SkillEffect();

    #region 其他操作 （Aim是否激活）

    /// <summary>
    /// 设置Aim的激活状态
    /// </summary>
    /// <param name="active">是否激活</param>
    public void SetAimActive(bool active)
    {
        aim.SetActive(active);
    }
    #endregion
}
