using UnityEngine;

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

    /// <summary>
    /// 获取随机缩放值
    /// </summary>
    public Vector3 GetRandomScale()
    {
        return GameMathf.RandomVector3(minScale, maxScale);
    }
}