using UnityEngine;

/// <summary>
/// 自定义游戏数学库
/// </summary>
public static class GameMathf
{
    /// <summary>
    /// 两个点位置是否在距离范围内
    /// </summary>
    /// <param name="pos1">第一个点</param>
    /// <param name="pos2">第二个点</param>
    /// <param name="maxDisatnce">最大距离</param>
    /// <returns>两点在指定范围内</returns>
    static public bool TwoPosInRange(Vector3 pos1, Vector3 pos2, float maxDisatnce)
    {
        return Vector3.SqrMagnitude(pos1 - pos2) < maxDisatnce * maxDisatnce;
    }

    /// <summary>
    /// 获取value在两个值之间的百分比（小数），失败返回-1
    /// </summary>
    /// <param name="a">第一个值</param>
    /// <param name="b">第二个值</param>
    /// <param name="value">当前值</param>
    /// <returns>返回当前值在两个值之间内的百分比位置</returns>
    static public float Persents(float a, float b, float value)
    {
        if (a == b)
            return -1;
        value = a < b ? Mathf.Clamp(value, a, b) : Mathf.Clamp(value, b, a);
        return (value - a) / (b - a);
    }

    /// <summary>
    /// 判断value是否在min和max的闭区间内
    /// </summary>
    /// <param name="value">需要判断的数值</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <returns>value是否在范围内</returns>
    static public bool InRange(float value,float min,float max)
    {
        return value >= min && value <= max;
    }
}
