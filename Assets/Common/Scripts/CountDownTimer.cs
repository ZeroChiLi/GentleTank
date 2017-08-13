
/// <summary>
/// 倒计时器
/// </summary>
public class CountDownTimer
{
    private ObjectPreferences timerPrefs = new ObjectPreferences();             // 计时器

    public float this[string key] { get { return (float)timerPrefs[key]; } }

    /// <summary>
    /// 添加或重置计时器
    /// </summary>
    /// <param name="key">计时器名称</param>
    /// <param name="startTime">起始时间</param>
    public void AddOrResetTimer(string key,float startTime)
    {
        timerPrefs.AddOrModifyValue(key, startTime);
    }

    /// <summary>
    /// 获取剩余时间
    /// </summary>
    /// <param name="key">计时器名称</param>
    /// <returns>返回剩余时间</returns>
    public float GetTimeLeft(string key)
    {
        return (float)timerPrefs[key];
    }

    /// <summary>
    /// 返回是否到时间结束（时间小于等于0）
    /// </summary>
    /// <param name="key">计时器名称</param>
    /// <returns>是否到时间结束</returns>
    public bool IsTimeUp(string key)
    {
        return (float)timerPrefs[key] <= 0;
    }

    /// <summary>
    /// 更新计时器，并返回是否时间到 
    /// </summary>
    /// <param name="key">计时器名称</param>
    /// <param name="interval">时间间隔</param>
    /// <returns>是否到时间结束</returns>
    public bool UpdateTimer(string key, float interval)
    {
        return CalculateCountDown(timerPrefs, key, interval);
    }

    /// <summary>
    /// 更新所有计时器
    /// </summary>
    /// <param name="interval">时间间隔</param>
    public void UpdateAllTimer(float interval)
    {
        foreach (var timer in timerPrefs.Preferneces)
            CalculateCountDown(timerPrefs, timer.Key, interval);
    }

    /// <summary>
    /// 使用其对象信息存储倒计时器,同时更新倒计时器，返回是否时间到
    /// </summary>
    /// <param name="prefs">对象信息列表</param>
    /// <param name="key">计时器名字</param>
    /// <param name="startTime">起始倒计时值</param>
    /// <param name="interval">时间间隔</param>
    /// <param name="recycle">是否循环（每次计时结束后重新赋值）</param>
    /// <returns>是否倒计时结束</returns>
    static public bool UpdateTimer(ObjectPreferences prefs,string key,float startTime,float interval,bool recycle = false)
    {
        if (!prefs.Contains(key))
            prefs.AddValue(key, startTime);

        if (CalculateCountDown(prefs,key,interval))
        {
            if (recycle)
                prefs[key] = startTime;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 更新倒计时器，返回是否到达时间
    /// </summary>
    /// <param name="prefs">对象信息列表</param>
    /// <param name="key">计时器名字</param>
    /// <param name="interval">时间间隔</param
    /// <returns>是否倒计时结束</returns>
    static private bool CalculateCountDown(ObjectPreferences prefs, string key, float interval)
    {
        if ((float)prefs[key] <= 0)
            return true;
        prefs[key] = (float)prefs[key] - interval;
        return false;
    }

}
