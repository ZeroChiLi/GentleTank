using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour 
{
    public Text toastText;          // 文本信息

    private Image toastImg;         // 文本框背景图片
    private bool isShowed;          // 是否正在显示
    private float elapsed;          // 结束显示时间
    private Color temColor;         // 颜色临时变量

    /// <summary>
    /// 初始化获取背景颜色和文本颜色，用来渐变消失
    /// </summary>
    private void Awake()
    {
        toastImg = GetComponent<Image>();
    }

    /// <summary>
    /// 规定时间后隐藏提示
    /// </summary>
    private void Update()
    {
        if (!isShowed)
            return;
        elapsed -= Time.deltaTime;
        if (elapsed <= 0f)
        {
            gameObject.SetActive(false);
            isShowed = false;
        }
        ToastHiding();
    }

    /// <summary>
    /// 提示消失颜色变化过程，最后1秒渐变消失，所以直接用elapsed
    /// </summary>
    private void ToastHiding()
    {
        toastImg.color = new Color(toastImg.color.r, toastImg.color.g, toastImg.color.b,elapsed);
        toastText.color = new Color(toastText.color.r, toastText.color.g, toastText.color.b, elapsed);
    }

    /// <summary>
    /// 显示提示
    /// </summary>
    /// <param name="duration">持续时间</param>
    /// <param name="content">显示内容</param>
    public void ShowToast(float duration, string content)
    {
        elapsed = duration;
        isShowed = true;
        toastText.text = content;
        gameObject.SetActive(true);
    }


}
