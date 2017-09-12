using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    public Text toastText;          // 文本信息
    public EasyTween easyTween;
    public float duration = 3f;

    private CountDownTimer timer;
    private bool isShowed;          // 是否正在显示

    public CountDownTimer Timer
    {
        get { return timer = timer ?? new CountDownTimer(duration, false, false); }
        set { timer = value; }
    }

    /// <summary>
    /// 规定时间后隐藏提示
    /// </summary>
    private void Update()
    {
        if (Timer.IsTimeUp)
        {
            Timer.Reset(duration, true);
            easyTween.OpenCloseObjectAnimation();
        }
    }

    /// <summary>
    /// 显示提示
    /// </summary>
    /// <param name="duration">持续时间</param>
    /// <param name="content">显示内容</param>
    public void ShowToast(string content, float duration = 3f)
    {
        this.duration = duration;
        isShowed = true;
        toastText.text = content;
        Timer.Start();
        gameObject.SetActive(true);
        if (!easyTween.IsObjectOpened())
            easyTween.OpenCloseObjectAnimation();
        Debug.Log(content);
    }


}
