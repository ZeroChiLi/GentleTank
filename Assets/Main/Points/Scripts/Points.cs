using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    [System.Serializable]
    public sealed class PointAppearance
    {
        public bool keepGizoms = true;
        public bool useScreenSize;
        public Color selectedColor = Color.magenta;
        [Range(0.1f, 1f)]
        public float pointSize = 0.3f;
        public Color pointColor = new Color(0.5f,0,1,1);
        [Range(10, 30)]
        public int indexFontSize = 20;
        public Color indexFontColor = Color.black;
        [Range(0.1f, 5f)]
        public float axisLength = 3f;
        [Range(10f, 50f)]
        public float focusSize = 20f;
    }

    public PointAppearance appearance = new PointAppearance();
    public int currentIndex = -1;
    public bool looped = true;
    public List<Point> points = new List<Point>();

    public int Count { get { return points.Count; } }
    public Point this[int index] { get { return points[index]; } set { points[index] = value; } }

    private void OnDrawGizmos()
    {
        if (!appearance.keepGizoms)
            return;
        Gizmos.color = appearance.pointColor;
        for (int i = 0; i < points.Count; i++)
            Gizmos.DrawSphere(GetWorldPosition(points[i]), appearance.pointSize * 2f);
    }

    /// <summary>
    /// 获取下一个点（如果looped为false，最后一个点之后为null）
    /// </summary>
    /// <param name="toWorldSpace">转换到世界坐标</param>
    /// <returns>下一个点</returns>
    public Point GetNextPoint(bool toWorldSpace = true)
    {
        if (Count == 0 || (looped == false && currentIndex >= Count - 1))
            return null;
        currentIndex = (currentIndex + 1) % Count;
        return toWorldSpace ? GetWorldSpacePoint(points[currentIndex]) : points[currentIndex];
    }

    /// <summary>
    /// 获取随机点
    /// </summary>
    /// <param name="toWorldSpace">转换到世界坐标</param>
    /// <returns>随机点</returns>
    public Point GetRandomPoint(bool toWorldSpace = true)
    {
        if (Count == 0)
            return null;
        return toWorldSpace ? GetWorldSpacePoint(points[Random.Range(0, Count)]) : points[Random.Range(0, Count)];
    }

    /// <summary>
    /// 添加或插入新点对象，index为-1代表插入到最后
    /// </summary>
    /// <param name="point">点对象</param>
    /// <param name="index">插入索引</param>
    public void Add(Point point, int index = -1)
    {
        if (index == -1)
            index = Count;
        if (index >= 0 && index <= Count)
            points.Insert(index, point);
    }

    /// <summary>
    /// 插入新的点，index为-1代表插入到最后
    /// </summary>
    /// <param name="pos">点位置</param>
    /// <param name="eulerAngles">点的欧拉角</param>
    /// <param name="index">插入位置</param>
    public void Add(Vector3 pos, Vector3 eulerAngles, int index = -1)
    {
        Add(new Point(pos, eulerAngles), index);
    }

    /// <summary>
    /// 插入新的点，index为-1代表插入到最后
    /// </summary>
    /// <param name="pos">点位置</param>
    /// <param name="rotation">点的旋转值</param>
    /// <param name="index">插入位置</param>
    public void Add(Vector3 pos, Quaternion rotation, int index = -1)
    {
        Add(new Point(pos, rotation), index);
    }

    /// <summary>
    /// 移除点对象
    /// </summary>
    /// <param name="point">点对象</param>
    public void Remove(Point point)
    {
        if (points.Contains(point))
            points.Remove(point);
    }

    /// <summary>
    /// 移除点对象
    /// </summary>
    /// <param name="index">点的索引值</param>
    public void Remove(int index)
    {
        if (index >= 0 && index < Count)
            points.RemoveAt(index);
    }

    /// <summary>
    /// 获取点位置的世界坐标
    /// </summary>
    public Vector3 GetWorldPosition(Point p)
    {
        return transform.rotation * p.position + transform.position;
    }

    /// <summary>
    /// 获取点旋转的世界坐标
    /// </summary>
    public Quaternion GetWorldRotation(Point p)
    {
        return transform.rotation * p.rotation;
    }

    /// <summary>
    /// 获取世界坐标下的点
    /// </summary>
    public Point GetWorldSpacePoint(Point p)
    {
        return new Point(GetWorldPosition(p), GetWorldRotation(p));
    }

    /// <summary>
    /// 获取世界坐标下的点
    /// </summary>
    public Point GetWorldSpacePoint(int index)
    {
        return GetWorldSpacePoint(points[index]);
    }
}
