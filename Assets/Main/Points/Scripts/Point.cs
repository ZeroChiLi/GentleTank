using UnityEngine;

[System.Serializable]
public class Point
{
    public Vector3 position;
    public Vector3 eulerAngles;
    public Quaternion rotation { get { return Quaternion.Euler(eulerAngles); } set { eulerAngles = value.eulerAngles; } }

    public Point() { }
    public Point(Vector3 position, Quaternion rotation) { this.position = position; this.rotation = rotation; }
    public Point(Vector3 position, Vector3 eulerAngles) { this.position = position; this.eulerAngles = eulerAngles; }
    public Point(Point point) { position = point.position;eulerAngles = point.eulerAngles; }

    /// <summary>
    /// 获取世界坐标下位置
    /// </summary>
    /// <param name="parent">父对象</param>
    public Vector3 GetWorldPosition(Transform parent)
    {
        return parent.TransformPoint(position);
    }

    /// <summary>
    /// 获取世界坐标下旋转四元数
    /// </summary>
    /// <param name="parent">父对象</param>
    public Quaternion GetWorldRotation(Transform parent)
    {
        return parent.rotation * rotation;
    }

    /// <summary>
    /// 获取世界坐标下的点
    /// </summary>
    /// <param name="parent">父对象</param>
    public Point GetWorldPoint(Transform parent)
    {
        return new Point(parent.TransformPoint(position), parent.rotation * rotation);
    }

    /// <summary>
    /// 获取世界坐标下点的向前方向单位向量
    /// </summary>
    /// <param name="parent">父对象</param>
    public Vector3 GetWorldForward(Transform parent)
    {
        return rotation * parent.forward;
    }

    /// <summary>
    /// 将数值赋给目标转换
    /// </summary>
    /// <param name="target">目标</param>
    public void SetToTransform(Transform target)
    {
        target.position = position;
        target.rotation = rotation;
    }
}

