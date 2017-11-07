using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于单个对象的信息列表存储
/// </summary>
public sealed class SafeDictionary<Key,Value>
{
    private Dictionary<Key, Value> preferences = new Dictionary<Key, Value>();        // 信息列表
    public Dictionary<Key, Value> Preferneces { get { return preferences; } }

    public Value this[Key key]                              // 信息索引器
    {
        get { return GetValue(key); }
        set { AddOrModifyValue(key, value); }
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
    public void Remove(Key key)
    {
        if (preferences.ContainsKey(key))
            preferences.Remove(key);
    }

    /// <summary>
    /// 是否包含该键值
    /// </summary>
    /// <param name="key">键</param>
    public bool Contains(Key key)
    {
        return preferences.ContainsKey(key);
    }

    /// <summary>
    /// 添加键值
    /// </summary>
    public void AddIfNotContains(Key key, Value value)
    {
        if (!preferences.ContainsKey(key))
            preferences.Add(key, value);
    }

    /// <summary>
    /// 添加键值，如果已经存在，覆盖掉
    /// </summary>
    public void AddOrModifyValue(Key key, Value value)
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
    public Value GetValue(Key key)
    {
        return preferences.ContainsKey(key) ? preferences[key] : default(Value);
    }

    /// <summary>
    /// 获取键值，如果不存在，创建之
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">如果存在，可以无视。如果不存在，则作为初始值</param>
    /// <returns></returns>
    public Value GetOrAddValue(Key key, Value value)
    {
        if (!preferences.ContainsKey(key))
            preferences.Add(key, value);
        return preferences[key];
    }

}
