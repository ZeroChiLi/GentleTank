using UnityEngine;
using UnityEngine.UI;

public class ChargeButtonMananger : CoolDownButtonManager
{
    public Image sliderFullImg;             // 蓄力变化的图片
    public float minValue = 50f;            // 最小值
    public float maxValue = 100f;           // 最大值
    public float chargeRate = 50f;          // 变化率

    protected float currentValue;           // 当前值
    public float CurrentValue
    {
        get { return currentValue; }
        set { currentValue = Mathf.Clamp(value, minValue, maxValue); }
    }

    public bool isCharging { get; protected set; }      // 是否正在变化

    /// <summary>
    /// 初始化
    /// </summary>
    private void Start()
    {
        Init();
        isCharging = false;
        ResetChargeValue();
    }

    /// <summary>
    /// 重置蓄力按钮值
    /// </summary>
    public void ResetChargeValue()
    {
        sliderFullImg.fillAmount = 0f;
        CurrentValue = minValue;
    }

    /// <summary>
    /// 更新按钮冷却时间
    /// </summary>
    private void Update()
    {
        if (!UpdateCoolDownAndCheck() || !isCharging)
            return;

        CurrentValue += Time.deltaTime * chargeRate;
        sliderFullImg.fillAmount = GameMathf.Persents(minValue, maxValue, CurrentValue);
    }

    /// <summary>
    /// 点击按下时响应
    /// </summary>
    public void OnPointerDown()
    {
        if (!coolDownTimer.IsTimeUp)
            return;
        isCharging = true;
        ResetChargeValue();
    }

    /// <summary>
    /// 点击松开时响应
    /// </summary>
    public void OnPointerUp()
    {
        if (!coolDownTimer.IsTimeUp)
            return;
        isCharging = false;
        ResetChargeValue();
    }

    /// <summary>
    /// 指针离开时响应
    /// </summary>
    public void OnPointerExit()
    {
        if (!coolDownTimer.IsTimeUp)
            return;
        isCharging = false;
        ResetChargeValue();
    }

}
