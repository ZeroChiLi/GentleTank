using UnityEngine;
using UnityEngine.UI;

public class WarnningArea : MonoBehaviour
{
    public float duration = 10f;                //显示区域的持续时间
    public float blinkDuration = 2f;            //闪烁时间
    public int blinkTimes = 3;                  //出现时闪烁次数  
    public float endDuration = 1f;              //警告区域消失持续时间

    private Color originColor;                  //初始颜色
    private float elapsedTime = 0f;             //计时器
    private float onceBlinkTime;                //每一次闪烁持续时间
    private Image warnningImage;                //警告区域图片
    private int currentBlink = 0;               //当前闪烁的次数

    /// <summary>
    /// 取图片组件，原始颜色
    /// </summary>
    private void Awake()
    {
        warnningImage = GetComponent<Image>();
        originColor = warnningImage.color;
    }

    /// <summary>
    /// 当被激活，计算每一次闪烁时间，计时器设置为0
    /// </summary>
    private void OnEnable()
    {
        onceBlinkTime = blinkDuration / blinkTimes;
        elapsedTime = 0f;
    }

    /// <summary>
    /// 通过计时器和持续时间判断闪烁、保持、隐藏还是失活对象
    /// </summary>
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime <= duration - endDuration)
            Blinking();
        else if (elapsedTime <= duration)
            HideWarnningArea();
        else
            gameObject.SetActive(false);
    }

    /// <summary>
    /// 闪烁、超过闪烁时间将保持源色
    /// </summary>
    private void Blinking()
    {
        if (elapsedTime < (blinkTimes - 1) * onceBlinkTime)
            OnceBlink(elapsedTime * 2, onceBlinkTime);
        else if (elapsedTime < blinkTimes * onceBlinkTime)
            OnceBlink(elapsedTime, onceBlinkTime);
        else
            warnningImage.color = originColor;
    }

    /// <summary>
    /// 隐藏警告区域
    /// </summary>
    private void HideWarnningArea()
    {
        OnceBlink(duration - elapsedTime, endDuration);
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
