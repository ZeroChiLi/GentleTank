
/// <summary>
/// 倒计时器
/// </summary>
public static class CountDownTimer
{
    /// <summary>
    /// 使用其对象信息存储倒计时器
    /// </summary>
    /// <param name="prefs">对象信息列表</param>
    /// <param name="key">计时器名字</param>
    /// <param name="startTime">起始倒计时值</param>
    /// <param name="interval">时间间隔</param>
    /// <param name="recycle">是否循环（每次计时结束后重新赋值）</param>
    /// <returns>是否倒计时结束</returns>
    static public bool IsCountDownEnd(ObjectPreferences prefs,string key,float startTime,float interval,bool recycle = false)
    {
        if (!prefs.Contains(key))
            prefs.AddValue(key, startTime);
        prefs[key] = (float)prefs[key] - interval;
        if ((float)prefs[key] > 0)
            return false;
        if (recycle)
            prefs[key] =startTime;
        return true;
    }
}
