using System;
using UnityEngine;

/// <summary>
/// 自定义游戏数学库
/// </summary>
public static class GameMathf
{
    /// <summary>
    /// 上下左右的整形偏移量，int[4, 2] { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } }
    /// </summary>
    static public readonly int[,] UpDownLeftRight = new int[4, 2] { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };

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
    static public Vector3 Round(Vector3 value, int precision = 2)
    {
        return new Vector3((float)Math.Round(value.x, precision), (float)Math.Round(value.y, precision), (float)Math.Round(value.z, precision));
    }

    /// <summary>
    /// 重置默认转换（本地坐标）
    /// </summary>
    /// <param name="transform">转换目标</param>
    /// <param name="pos">是否修改位置</param>
    /// <param name="rotate">是否修改旋转</param>
    /// <param name="scale">是否修改缩放</param>
    static public void ResetLocalTransform(Transform transform, bool pos = true, bool rotate = true, bool scale = true)
    {
        if (pos)
            transform.localPosition = Vector3.zero;
        if (rotate)
            transform.localRotation = Quaternion.identity;
        if (scale)
            transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 复制位置和旋转
    /// </summary>
    /// <param name="form">复制源</param>
    /// <param name="to">目标转换</param>
    static public void CopyPositionAndRotation(Transform form, Transform to)
    {
        to.position = form.position;
        to.rotation = form.rotation;
    }

    /// <summary>
    /// 求向量每个分量的绝对值
    /// </summary>
    /// <param name="vec3">目标向量</param>
    static public Vector3 Abs(Vector3 vec3)
    {
        return new Vector3(Mathf.Abs(vec3.x), Mathf.Abs(vec3.y), Mathf.Abs(vec3.z));
    }

    /// <summary>
    /// 求向量每个分量的绝对值
    /// </summary>
    /// <param name="vec2">目标向量</param>
    static public Vector2 Abs(Vector2 vec2)
    {
        return new Vector2(Mathf.Abs(vec2.x), Mathf.Abs(vec2.y));
    }

    /// <summary>
    /// 取value的二次幂
    /// </summary>
    static public float Pow2(float value)
    {
        return value * value;
    }

    /// <summary>
    /// 转换Vector2向量到Vector3的x、z分量中，即Vector3(vec2.x, 0, vec2.y)。
    /// </summary>
    static public Vector3 Vec2ToVec3XZ(Vector2 vec2)
    {
        return new Vector3(vec2.x, 0, vec2.y);
    }

    /// <summary>
    /// 随机Vector3向量，每个分量在a和b对应分量之间。即返回的x为a.x到b.x的范围内。
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    static public Vector3 RandomVector3(Vector3 a, Vector3 b)
    {
        return new Vector3(UnityEngine.Random.Range(a.x, b.x), UnityEngine.Random.Range(a.y, b.y), UnityEngine.Random.Range(a.z, b.z));
    }

    /// <summary>
    /// 弧度转换成角度
    /// </summary>
    static public float RadianToAngle(float value)
    {
        return (float)(value * 180 / Math.PI);
    }

    /// <summary>
    /// 角度转换成弧度
    /// </summary>
    static public float AngleToRadian(float value)
    {
        return (float)(value / Math.PI * 180);
    }

    /// <summary>
    /// 返回随机Y值的Vector3，x和z值为0
    /// </summary>
    static public Vector3 RandomY()
    {
        return new Vector3(0, UnityEngine.Random.Range(-180, 180), 0);
    }

    /// <summary>
    /// 随机返回正1或负1
    /// </summary>
    static public int RandomPlusOrMinus()
    {
        return UnityEngine.Random.value > 0.5f ? 1 : -1;
    }

    /// <summary>
    /// 判断xy值是否在[0,maxX)和[0,maxY)区间
    /// </summary>
    static public bool XYIsInRange(int x, int y, int maxX, int maxY)
    {
        return x >= 0 && y >= 0 && x < maxX && y < maxY;
    }

    /// <summary>
    /// 从传入参数中的数中（不能为空），随机返回一个
    /// </summary>
    static public int RandomNumber(params int[] numbers)
    {
        if (numbers.Length == 0)
            Debug.LogError("RandomNumber() args should not be empty.");
        return numbers[UnityEngine.Random.Range(0,numbers.Length)];
    }
}
