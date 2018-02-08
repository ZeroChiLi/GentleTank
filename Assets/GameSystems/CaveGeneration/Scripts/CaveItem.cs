using UnityEngine;

[System.Serializable]
public class CaveItem
{
    public GameObject prefab;                       // 预设对象
    public Vector3 size = new Vector3(1, 0, 1);     // 大小
    public Vector3 offset = Vector3.zero;           // 锚点偏移量
    public bool randomRotationY = false;          // 是否随机旋转Y轴

    public float Aspect { get { return size.z / size.x; } } // 宽高比
    public float AreaSize { get { return size.x * size.z; } }   // 占地面积

    /// <summary>
    /// 比较两个洞穴物体的占地面积大小：x.Area - y.Area
    /// </summary>
    static public int CompareArea(CaveItem x, CaveItem y)
    {
        return (int)(x.AreaSize - y.AreaSize);
    }
}
