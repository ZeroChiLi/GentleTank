using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Configure/Point List")]
public class PointList : ScriptableObject
{
    public Color pointColor;            // 在场景中颜色
    public bool showPointColor = true;  // 是否显示点
    public bool showAxis = true;        // 是否显示坐标轴
    public List<Point> pointList;       // 所有点列表

    private int currentIndex = -1;      // 当前索引
    private float axisLength = 2;       // 坐标长度     

    public Point this[int index] { get { return pointList[index]; } }

    public int Count { get { return pointList.Count; } }

    /// <summary>
    /// 激活所有点
    /// </summary>
    public void EnableAllPoints()
    {
        pointColor.a = 1;
        foreach (var item in pointList)
            item.enable = true;
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
    public bool IsEmpty()
    {
        if (pointList.Count == 0)
        {
            Debug.LogError("PointList Is Empty!");
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取随机点
    /// </summary>
    /// <param name="allowDisable">是否允许获取使用过的点</param>
    /// <param name="isDifferent">是否是不同于当前的点</param>
    /// <returns>返回获取到的点</returns>
    public Point GetRandomPoint(bool allowDisable = true, bool isDifferent = true)
    {
        if (IsEmpty())
            return null;
        if (allowDisable)           //允许获取使用过的点
        {
            if (isDifferent)        //要求是不同于上一次选择的点
                return SetCurrentPointByIndex(GetRandomDifferenceIndex(currentIndex, 0, pointList.Count));
            return SetCurrentPointByIndex(Random.Range(0, pointList.Count));    //随机获取点列表任意点
        }

        //下面获取未使用过的点（Point.enable == falase）

        List<int> enablePoints = GetEnablePointsIndex(isDifferent);         // 获取有效的点索引列表
        if (enablePoints == null)                                           // 不存在返回null
            return null;
        currentIndex = enablePoints[Random.Range(0, enablePoints.Count)];   // 再从有效点索引列表中随机获取有效点索引
        pointList[currentIndex].enable = false;                             // 设置点无效（已经使用过）

        return pointList[currentIndex];
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
        } while (index == current);                 //死循环获取不等于current值的随机数
        return index;
    }

    /// <summary>
    /// 获取未使用过的所有点
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
    /// 调试时显示所有点对应场景位置
    /// </summary>
    public void DebugDrawPoint()
    {
        ShowPosition(showPointColor);
        ShowAxis(showAxis);
    }

    /// <summary>
    /// 显示在场景中的位置
    /// </summary>
    /// <param name="show">是否显示</param>
    public void ShowPosition(bool show)
    {
        if (!show)
            return;
        Gizmos.color = pointColor;
        for (int i = 0; i < pointList.Count; i++)
            Gizmos.DrawSphere(pointList[i].position, 1);
    }

    /// <summary>
    /// 显示在点的方向坐标
    /// </summary>
    /// <param name="show">是否显示</param>
    public void ShowAxis(bool show)
    {
        if (!show)
            return;
        GizomsDrawLine(Color.red, Vector3.right);
        GizomsDrawLine(Color.green, Vector3.up);
        GizomsDrawLine(Color.blue, Vector3.forward);
    }

    /// <summary>
    /// 绘制Gizoms线
    /// </summary>
    /// <param name="color">颜色</param>
    /// <param name="distance">相对距离</param>
    private void GizomsDrawLine(Color color, Vector3 distance)
    {
        Gizmos.color = color;
        for (int i = 0; i < pointList.Count; i++)
            Gizmos.DrawLine(pointList[i].position, pointList[i].Rotation * distance * axisLength + pointList[i].position);
    }
}

