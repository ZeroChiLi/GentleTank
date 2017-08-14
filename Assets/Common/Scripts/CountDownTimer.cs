using UnityEngine;

/// <summary>
/// 倒计时器。使用UpdateAndCheck来更新计时器
/// </summary>
public class CountDownTimer
{
    public bool IsAutoCycle { get; private set; }                   // 是否自动循环（小于等于0后重置）
    public float CurrentTime { get; private set; }                  // 当前时间
    public bool IsTimeUp { get { return CurrentTime <= 0; } }       // 是否时间到
    public bool IsStoped { get; private set; }                      // 是否暂停了

    private float startTime;                                        // 起始时间
    public float StartTime
    {
        get { return startTime; }
        set
        {
            startTime = value;
            CurrentTime = startTime;
        }
    }

    /// <summary>
    /// 构造倒计时器
    /// </summary>
    /// <param name="startTime">起始时间</param>
    /// <param name="autocycle">是否自动循环</param>
    public CountDownTimer(float startTime,bool autocycle = false)
    {
        StartTime = Mathf.Max(0f, startTime);
        IsAutoCycle = autocycle;
    }

    /// <summary>
    /// 重置计时器
    /// </summary>
    public void Reset()
    {
        CurrentTime = StartTime;
    }

    /// <summary>
    /// 更新倒计时，计时结束返回true
    /// </summary>
    /// <param name="interval">时间间隔</param>
    /// <returns>返回是否几时结束</returns>
    public bool UpdateAndCheck(float interval)
    {
        if (IsStoped)
            return CurrentTime <= 0;
        if (CurrentTime <= 0)
        {
            if (IsAutoCycle)
                CurrentTime = StartTime;
            return true;
        }
        CurrentTime -= interval;
        return false;
    }
    /// <summary>
    /// 设置当前值为0
    /// </summary>
    public void End()
    {
        CurrentTime = 0f;
    }

    /// <summary>
    /// 获取倒计时完成率（0为没开始计时，1为计时结束）
    /// </summary>
    /// <returns></returns>
    public float GetPercent()
    {
        if (CurrentTime <= 0 || StartTime <= 0)
            return 1f;
        return 1 - CurrentTime / StartTime;
    }

}
