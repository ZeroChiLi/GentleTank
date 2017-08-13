using System.Collections.Generic;

/// <summary>
/// 用于单个对象的信息列表存储
/// </summary>
public sealed class ObjectPreferences
{
    private Dictionary<string, object> preferences;             // 信息列表
    public Dictionary<string, object> Preferneces { get { return preferences; } }

    public object this[string key]                              // 信息索引器
    {
        get { return GetValue(key); }
        set { AddOrModifyValue(key, value); }
    }

    /// <summary>
    /// 初始化构造创建列表
    /// </summary>
    public ObjectPreferences()
    {
        preferences = new Dictionary<string, object>();
    }

    /// <summary>
    /// 清除列表所有信息
    /// </summary>
    public void Clear()
    {
        preferences.Clear();
    }

    /// <summary>
    /// 移除键值
    /// </summary>
    /// <param name="key">需要移除信息的键</param>
    public void Remove(string key)
    {
        if (preferences.ContainsKey(key))
            preferences.Remove(key);
    }

    /// <summary>
    /// 是否包含该键值
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>是否包含该对象键值</returns>
    public bool Contains(string key)
    {
        return preferences.ContainsKey(key);
    }

    /// <summary>
    /// 添加键值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddValue(string key, object value)
    {
        if (!preferences.ContainsKey(key))
            preferences.Add(key, value);
    }

    /// <summary>
    /// 添加键值，如果已经存在，覆盖掉
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">对象</param>
    public void AddOrModifyValue(string key, object value)
    {
        if (preferences.ContainsKey(key))
            preferences[key] = value;
        else
            preferences.Add(key, value);
    }

    /// <summary>
    /// 获取键值，不存在返回null
    /// </summary>
    /// <param name="key">获取对象的键</param>
    /// <returns>返回对应对象，不存在为null</returns>
    public object GetValue(string key)
    {
        return preferences.ContainsKey(key) ? preferences[key] : null;
    }

    /// <summary>
    /// 获取指定类型的值
    /// </summary>
    /// <typeparam name="T">获取的类型</typeparam>
    /// <param name="key">键</param>
    /// <returns>返回对应对象，失败为null</returns>
    public T GetValue<T>(string key) 
    {
        if (!preferences.ContainsKey(key))
            return default(T);
        return (T)preferences[key];
    }

    /// <summary>
    /// 获取键值，如果不存在，创建之
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">如果存在，可以无视。如果不存在，则作为初始值</param>
    /// <returns></returns>
    public object GetOrAddValue(string key, object value)
    {
        if (!preferences.ContainsKey(key))
            preferences.Add(key, value);
        return preferences[key];
    }

}
