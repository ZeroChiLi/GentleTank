using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CaveItem
{
    public GameObject item;                         // 预设对象
    public Vector3 size = new Vector3(1, 0, 1);     // 大小
    public Vector3 offset = Vector3.zero;           // 锚点偏移量
    public Vector3 minScale = Vector3.one;          // 最小缩放大小
    public Vector3 maxScale = Vector3.one;          // 最大缩放大小
    public bool inverse = false;                    // 是否翻转
    public bool isRandomRotationY = false;          // 是否随机旋转Y轴

    public float Aspect { get { return size.z / size.x; } } // 宽高比
    public float AreaSize { get { return size.x * size.z; } }   // 占地面积
    public int InverseInt { get { return inverse ? -1 : 1; } }

    /// <summary>
    /// 获取随机缩放值
    /// </summary>
    public Vector3 GetRandomScale()
    {
        return GameMathf.RandomVector3(minScale, maxScale);
    }

    /// <summary>
    /// 比较两个洞穴物体的占地面积大小：x.Area - y.Area
    /// </summary>
    static public int CompareArea(CaveItem x, CaveItem y)
    {
        return (int)(x.AreaSize - y.AreaSize);
    }
}
