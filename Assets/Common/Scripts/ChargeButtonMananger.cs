using UnityEngine;
using UnityEngine.UI;

public class ChargeButtonMananger : MonoBehaviour
{
    public Button button;
    public Image buttonEmptyImg;
    public Image buttonFullImg;
    public float coolDownTime = 1f;
    public Image sliderEmptyImg;
    public Image sliderFullImg;
    public float minValue = 50f;
    public float maxValue = 100f;
    public float changingRate = 10f;

    private CountDownTimer timer;
    private readonly string coolDownTimer = "CoolDownTimer";
    private bool isCharging;
    private float currentChargeValue;

    /// <summary>
    /// 初始化
    /// </summary>
    private void Start()
    {
        timer = new CountDownTimer();
        timer.AddOrResetTimer(coolDownTimer, 0);
        buttonFullImg.fillAmount = 0f;
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
        if (!timer.IsTimeUp(coolDownTimer))
        {
            timer.UpdateTimer(coolDownTimer, Time.deltaTime);
            buttonFullImg.fillAmount = GameMathf.Persents(0, coolDownTime, timer[coolDownTimer]);
            return;
        }
        button.enabled = true;
        if (!isCharging)
            return;
        currentChargeValue += Time.deltaTime * changingRate;
        sliderFullImg.fillAmount = GameMathf.Persents(minValue, maxValue, currentChargeValue);
    }

    /// <summary>
    /// 点击按钮时响应
    /// </summary>
    public void OnClick()
    {
        if (!timer.IsTimeUp(coolDownTimer))
            return;
        button.enabled = false;
        timer.AddOrResetTimer(coolDownTimer, coolDownTime);
    }

    /// <summary>
    /// 点击按下时响应
    /// </summary>
    public void OnPointerDown()
    {
        if (!timer.IsTimeUp(coolDownTimer))
            return;
        isCharging = true;
        ResetChargeValue();
    }

    /// <summary>
    /// 点击松开时响应
    /// </summary>
    public void OnPointerUp()
    {
        if (!timer.IsTimeUp(coolDownTimer))
            return;
        isCharging = false;
        ResetChargeValue();
    }

    /// <summary>
    /// 指针离开时响应
    /// </summary>
    public void OnPointerExit()
    {
        if (!timer.IsTimeUp(coolDownTimer))
            return;
        isCharging = false;
        ResetChargeValue();
    }

}
