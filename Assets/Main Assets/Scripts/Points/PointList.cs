using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Configure/Point List")]
public class PointList : ScriptableObject
{
    [ColorUsage(false)]
    public Color sceneColor;
    public List<Point> pointList;

    private int currentIndex = -1;

    public Point this[int index] { get { return pointList[index]; } }

    public int Count { get { return pointList.Count; } }

    /// <summary>
    /// 激活所有点
    /// </summary>
    public void EnableAllPoints()
    {
        sceneColor.a = 1;
        foreach (var item in pointList)
            item.enable = true;
    }

    /// <summary>
    /// 获取随机点
    /// </summary>
    /// <param name="allowDisable">是否允许获取使用过的点</param>
    /// <param name="isDifferent">是否是不同于当前的点</param>
    /// <returns></returns>
    public Point GetRandomPoint(bool allowDisable = true, bool isDifferent = true)
    {
        if (IsEmpty())
            return null;
        if (allowDisable)
        {
            if (isDifferent)
                return SetCurrentPointByIndex(GetRandomDifferenceIndex(currentIndex, 0, pointList.Count));
            return SetCurrentPointByIndex(Random.Range(0, pointList.Count));
        }

        List<int> enablePoints = GetEnablePointsIndex(isDifferent);     // 获取有效的点列表
        if (enablePoints == null)
            return null;

        int randomIndex = Random.Range(0, enablePoints.Count);
        pointList[enablePoints[randomIndex]].enable = false;            // 设置点无效（已经使用过）
        currentIndex = enablePoints[randomIndex];

        return pointList[enablePoints[randomIndex]];
    }

    /// <summary>
    /// 获取当前点的下一个点
    /// </summary>
    /// <returns></returns>
    public Point GetNextPoint()
    {
        if (IsEmpty())
            return null;
        currentIndex = (currentIndex + 1) % pointList.Count;
        return pointList[currentIndex];
    }

    /// <summary>
    /// 获取点数量
    /// </summary>
    /// <returns></returns>
    public int GetPointsCount()
    {
        return pointList.Count;
    }

    /// <summary>
    /// 判断是否为空
    /// </summary>
    private bool IsEmpty()
    {
        if (pointList.Count == 0)
        {
            Debug.LogError("PointList Is Empty!");
            return true;
        }
        return false;
    }

    /// <summary>
    /// 设置当前索引值，并返回对应的点
    /// </summary>
    /// <param name="index">索引值</param>
    /// <returns>返回该索引对应的点</returns>
    private Point SetCurrentPointByIndex(int index)
    {
        currentIndex = index;
        return pointList[index];
    }

    /// <summary>
    /// 获取随机未使用过的点的索引
    /// </summary>
    /// <param name="isDifferent">是否需要不同于当前的点</param>
    /// <returns></returns>
    private List<int> GetEnablePointsIndex(bool isDifferent = false)
    {
        List<int> enablePoints = new List<int>();        // 存放点索引
        for (int i = 0; i < pointList.Count; i++)
        {
            if (!pointList[i].enable || (isDifferent && i == currentIndex))
                continue;
            enablePoints.Add(i);
        }

        if (enablePoints.Count == 0)
        {
            Debug.LogError("There are no enable Point can use!");
            return null;
        }
        return enablePoints;
    }

    /// <summary>
    /// 获取在min到max范围内不同于current值的随机数
    /// </summary>
    /// <param name="current">当前值</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <returns>失败返回-1</returns>
    public int GetRandomDifferenceIndex(int current, int min, int max)
    {
        if (current == min && min == max)
            return -1;

        int index;
        do
        {
            index = Random.Range(min, max);
        } while (index == current);
        return index;
    }

    /// <summary>
    /// 调试时显示所有点对应场景位置
    /// </summary>
    public void DebugDrawPoint()
    {
        Gizmos.color = sceneColor;
        for (int i = 0; i < pointList.Count; i++)
            Gizmos.DrawSphere(pointList[i].position, 1);
    }
}

