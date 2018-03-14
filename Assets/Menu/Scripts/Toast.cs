using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    static public Toast Instance { get; private set; }
    public Text toastText;          // 文本信息
    public float duration = 3f;

    private CountDownTimer timer;

    public CountDownTimer Timer
    {
        get { return timer = timer ?? new CountDownTimer(duration, false, false); }
        set { timer = value; }
    }

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 规定时间后隐藏提示
    /// </summary>
    private void Update()
    {
        if (Timer.IsTimeUp)
        {
            Timer.Reset(duration, true);
            gameObject.SetActive(false);
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
        toastText.text = content;
        Timer.Start();
        gameObject.SetActive(true);
        Debug.Log(content);
    }


}
