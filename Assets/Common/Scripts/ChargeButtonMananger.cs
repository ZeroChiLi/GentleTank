using UnityEngine;
using UnityEngine.UI;

public class ChargeButtonMananger : CoolDownButtonManager
{
    public Image sliderFullImg;             // 蓄力变化的图片
    public float minValue = 50f;            // 最小值
    public float maxValue = 100f;           // 最大值
    public float changingRate = 10f;        // 变化率

    private bool isCharging;                // 是否正在变化
    private float currentChargeValue;       // 当前值

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
        currentChargeValue = minValue;
    }

    /// <summary>
    /// 更新按钮冷却时间
    /// </summary>
    private void Update()
    {
        if (!UpdateCoolDownAndCheck() || !isCharging)
            return;
        currentChargeValue += Time.deltaTime * changingRate;
        sliderFullImg.fillAmount = GameMathf.Persents(minValue, maxValue, currentChargeValue);
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
