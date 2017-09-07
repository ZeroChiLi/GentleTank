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
    /// 将Vector3中所有接近0，但又小于指定精度值的数置0
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="precision">精度</param>
    /// <returns>返回计算后的值</returns>
    static public Vector3 ClampZeroWithPrecision(Vector3 value, float precision = 3)
    {
        Vector3 newValue;
        newValue.x = ClampZeroWithPrecision(value.x,precision);
        newValue.y = ClampZeroWithPrecision(value.y,precision);
        newValue.z = ClampZeroWithPrecision(value.z,precision);
        return newValue;
    }

    /// <summary>
    /// 将接近0，但又小于指定精度值的数置0
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="precision">精度</param>
    /// <returns>返回计算后的值</returns>
    static public float ClampZeroWithPrecision(float value,float precision = 3)
    {
        return Mathf.Abs(value) < Mathf.Pow(0.1f, precision) ? 0 : value;
    }

    /// <summary>
    /// 将Vector3中所有接近0，但又小于指定范围值的数置0
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="round">范围</param>
    /// <returns>计算后的值</returns>
    static public Vector3 ClampZeroWithRound(Vector3 value,float round = 0.02f)
    {
        Vector3 newValue;
        newValue.x = ClampZeroWithRound(value.x, round);
        newValue.y = ClampZeroWithRound(value.y, round);
        newValue.z = ClampZeroWithRound(value.z, round);
        return newValue;
    }

    /// <summary>
    /// 将接近0，但又小于指定范围值的数置0
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="round">范围</param>
    /// <returns>计算后的值</returns>
    static public float ClampZeroWithRound(float value,float round = 0.02f)
    {
        return Mathf.Abs(value) < round ? 0 : value;
    }

}
