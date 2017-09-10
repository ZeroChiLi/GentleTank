using System;
using UnityEngine;

/// <summary>
/// 自定义游戏数学库
/// </summary>
public static class GameMathf
{
    /// <summary>
    /// 判断value是否在min和max的闭区间内
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="value">需要判断的数值</param>
    /// <returns>value是否在范围内</returns>
    static public bool ValueInRange(float min, float max, float value)
    {
        return value >= min && value <= max;
    }

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
    /// 获取value在两个值之间的百分比（小数），失败返回0
    /// </summary>
    /// <param name="a">第一个值</param>
    /// <param name="b">第二个值</param>
    /// <param name="value">当前值</param>
    /// <returns>返回当前值在两个值之间内的百分比位置</returns>
    static public float Persents(float a, float b, float value)
    {
        if (a == b)
        {
            Debug.LogWarningFormat("GameMathf.Persents : a & b Is Same.");
            return 0;
        }
        value = a < b ? Mathf.Clamp(value, a, b) : Mathf.Clamp(value, b, a);
        return Mathf.Clamp01((value - a) / (b - a));
    }

    /// <summary>
    /// 四舍五入三维值
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="precision">精度</param>
    /// <returns>返回处理后的值</returns>
    static public Vector3 Round(Vector3 value,int precision = 2)
    {
        return new Vector3((float)Math.Round(value.x,precision), (float)Math.Round(value.y, precision), (float)Math.Round(value.z, precision));
    }

    /// <summary>
    /// 重置默认转换（本地坐标）
    /// </summary>
    /// <param name="transform">转换</param>
    static public void ResetTransform(Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
