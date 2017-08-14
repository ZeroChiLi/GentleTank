using UnityEngine;
using UnityEngine.UI;

public class CoolDownButtonManager : MonoBehaviour
{
    public Button button;                       // 按钮
    public Image buttonFullImg;                 // 按钮滑动条图片
    public bool coolDownAtStart = true;         // 是否在开始时先冷却
    public float coolDownTime = 1f;             // 冷却时间

    protected CountDownTimer timer;             // 计时器
    protected readonly string coolDownTimer = "CoolDownTimer";      // 计时器名称

    /// <summary>
    /// 初始化
    /// </summary>
    private void Start()
    {
        Init();
    }

    /// <summary>
    /// 更新倒计时
    /// </summary>
    private void Update()
    {
        UpdateCoolDownAndCheck();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        timer = new CountDownTimer();
        timer.AddOrResetTimer(coolDownTimer, coolDownAtStart ? coolDownTime : 0);
        buttonFullImg.fillAmount = coolDownAtStart ? 1 : 0;
        button.onClick.AddListener(OnClick);
    }

    /// <summary>
    /// 点击按钮时响应
    /// </summary>
    public void OnClick()
    {
        if (!timer.IsTimeUp(coolDownTimer))
            return;
        button.interactable = false;
        timer.AddOrResetTimer(coolDownTimer, coolDownTime);
    }

    /// <summary>
    /// 更新冷却时间同时改变滑动条和按钮状态，返回是否时间结束
    /// </summary>
    /// <returns>是否结束</returns>
    protected bool UpdateCoolDownAndCheck()
    {
        if (!timer.IsTimeUp(coolDownTimer))
        {
            timer.UpdateTimer(coolDownTimer, Time.deltaTime);
            buttonFullImg.fillAmount = GameMathf.Persents(0, coolDownTime, timer[coolDownTimer]);
            return false;
        }
        button.interactable = true;
        return true;
    }


}
