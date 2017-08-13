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
    static public bool TwoPosInRange(Vector3 pos1,Vector3 pos2,float maxDisatnce)
    {
        return Vector3.SqrMagnitude(pos1 - pos2) < maxDisatnce * maxDisatnce;
    }
}
