using UnityEngine;

/// <summary>
/// 倒计时器。
/// </summary>
public sealed class CountDownTimer
{
    public bool IsAutoCycle { get; private set; }                   // 是否自动循环（小于等于0后重置）
    public bool IsStoped { get; private set; }                      // 是否是否暂停了
    public float CurrentTime { get { return UpdateCurrentTime(); } }// 当前时间
    public bool IsTimeUp { get { return CurrentTime <= 0; } }       // 是否时间到
    public float Duration { get; private set; }                     // 计时时间长度

    private float lastTime;                                         // 上一次更新的时间
    private int lastUpdateFrame;                                    // 上一次更新倒计时的帧数（避免一帧多次更新计时）
    private float currentTime;                                      // 当前计时器剩余时间

    /// <summary>
    /// 构造倒计时器
    /// </summary>
    /// <param name="duration">起始时间</param>
    /// <param name="autocycle">是否自动循环</param>
    public CountDownTimer(float duration, bool autocycle = false, bool autoStart = true)
    {
        IsStoped = true;
        Duration = Mathf.Max(0f, duration);
        IsAutoCycle = autocycle;
        Reset(duration,!autoStart);
    }

    /// <summary>
    /// 更新计时器时间
    /// </summary>
    /// <returns>返回剩余时间</returns>
    private float UpdateCurrentTime()
    {
        if (IsStoped || lastUpdateFrame == Time.frameCount)         // 暂停了或已经这一帧更新过了，直接返回
            return currentTime;
        if (currentTime <= 0)                                       // 小于等于0直接返回，如果循环那就重置时间
        {
            if (IsAutoCycle)
                Reset(Duration,false);
            return currentTime;
        }
        currentTime -= Time.time - lastTime;
        UpdateLastTimeInfo();
        return currentTime;
    }

    /// <summary>
    /// 更新时间标记信息
    /// </summary>
    private void UpdateLastTimeInfo()
    {
        lastTime = Time.time;
        lastUpdateFrame = Time.frameCount;
    }

    /// <summary>
    /// 重置计时器，并开始计时
    /// </summary>
    public void Start()
    {
        Reset(Duration,false);
    }

    /// <summary>
    /// 重置计时器
    /// </summary>
    /// <param name="duration">持续时间</param>
    /// <param name="isStoped">是否暂停</param>
    public void Reset(float duration,bool isStoped = false)
    {
        UpdateLastTimeInfo();
        Duration = Mathf.Max(0f, duration);
        currentTime = Duration;
        IsStoped = isStoped;
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        UpdateCurrentTime();    // 暂停前先更新一遍
        IsStoped = true;
    }

    /// <summary>
    /// 继续（取消暂停）
    /// </summary>
    public void Continue()
    {
        UpdateLastTimeInfo();   // 继续前先更新一般时间信息
        IsStoped = false;
    }

    /// <summary>
    /// 终止，暂停且设置当前值为0
    /// </summary>
    public void End()
    {
        IsStoped = true;
        currentTime = 0f;
    }

    /// <summary>
    /// 获取倒计时完成率（0为没开始计时，1为计时结束）
    /// </summary>
    /// <returns></returns>
    public float GetPercent()
    {
        UpdateCurrentTime();
        if (currentTime <= 0 || Duration <= 0)
            return 1f;
        return 1f - currentTime / Duration;
    }

}
