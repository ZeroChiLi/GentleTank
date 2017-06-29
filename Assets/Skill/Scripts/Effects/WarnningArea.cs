using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarnningArea : MonoBehaviour
{
    public float duration = 10f;                //显示区域的持续时间
    public float blinkDuration = 2f;            //闪烁时间
    public int blinkTimes = 3;                  //出现时闪烁次数  
    public float disappearDuration = 1f;        //警告区域消失持续时间

    private Color originColor;                  //初始颜色
    private float elapsedTime = 0f;             //计时器
    private float onceBlinkTime;                //每一次闪烁持续时间
    private Image warnningImage;                //警告区域图片
    private int currentBlink = 0;               //当前闪烁的次数

    private void Awake()
    {
        warnningImage = GetComponent<Image>();
        originColor = warnningImage.color;
        onceBlinkTime = blinkDuration / blinkTimes;
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime <= duration - disappearDuration)
            ShowWarnningArea();
        else if (elapsedTime <= duration)
            HideWarnningArea();
        else
            gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示警告区域，大小随攻击范围改变
    /// </summary>
    private void ShowWarnningArea()
    {
        if (elapsedTime < (blinkTimes -1) * onceBlinkTime)
            OnceBlink(elapsedTime * 2, onceBlinkTime);
        else if (elapsedTime < blinkTimes * onceBlinkTime)
            OnceBlink(elapsedTime, onceBlinkTime);
        else
            warnningImage.color = originColor;
    }

    /// <summary>
    /// 延时一段时间后，隐藏警告区域
    /// </summary>
    private void HideWarnningArea()
    {
        OnceBlink(duration - elapsedTime, disappearDuration);
    }

    /// <summary>
    /// 每一次闪烁，（颜色渐变）
    /// </summary>
    /// <param name="elapsed">根据这个值变化改变透明度</param>
    /// <param name="period">周期长度</param>
    /// <returns></returns>
    private void OnceBlink(float elapsed, float period)
    {
        warnningImage.color = new Color(warnningImage.color.r, warnningImage.color.g, warnningImage.color.b, Mathf.PingPong(elapsed, period) / period);
    }
}
