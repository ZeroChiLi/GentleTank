using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    static public Toast Instance { get; private set; }
    public Text toastText;          // 文本信息
    public float duration = 3f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float moveDuration = 1f;
    public Vector3 startPos = new Vector3(0, -32, 0);
    public Vector3 endPos = new Vector3(0, 64, 0);

    private RectTransform rectTransform;
    private CountDownTimer timer;
    private CountDownTimer moveTimer;

    public CountDownTimer Timer
    {
        get { return timer = timer ?? new CountDownTimer(duration, false, false); }
        set { timer = value; }
    }

    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
        moveTimer = new CountDownTimer(moveDuration, false, false);
    }

    /// <summary>
    /// 规定时间后隐藏提示
    /// </summary>
    private void Update()
    {
        if (!moveTimer.IsTimeUp)
            rectTransform.localPosition = Vector3.LerpUnclamped(startPos, endPos, moveCurve.Evaluate(moveTimer.GetPercent()));
        else
            rectTransform.localPosition = endPos;

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
        moveTimer.Start();
        gameObject.SetActive(true);
        Debug.Log(content);
    }


}
