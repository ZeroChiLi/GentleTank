using System.Collections.Generic;
using UnityEngine;

public class SelectedImageManager : MonoBehaviour 
{
    public RectTransform rectTransform;         // 自身矩形变化
    public List<RectTransform> targetList;      // 目标列表
    public float smoothTime = 0.3f;             // 平滑时间
    public int index = 0;                       // 下一个目标的索引

    private Vector3 velocity;                   // 当前偏移速度

    /// <summary>
    /// 平滑移动
    /// </summary>
    public void Update()
    {
        rectTransform.position = Vector3.SmoothDamp(rectTransform.position, targetList[index].position, ref velocity, smoothTime);
    }

    /// <summary>
    /// 设置目标索引值
    /// </summary>
    /// <param name="index"></param>
    public void SetTargetIndex(int index)
    {
        this.index = index;
    }

    /// <summary>
    /// 立即移动目标到指定位置
    /// </summary>
    /// <param name="index">索引值</param>
    public void SetTargetImmediately(int index)
    {
        this.index = index;
        transform.position = targetList[index].position;
    }

    /// <summary>
    /// 设置目标索引为当前坦克索引
    /// </summary>
    public void SetTargetIndexByCurrentTankIndex()
    {
        index = AllCustomTankManager.Instance.CurrentIndex;
    }
}
