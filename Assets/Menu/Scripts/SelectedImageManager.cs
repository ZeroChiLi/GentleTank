using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedImageManager : MonoBehaviour 
{
    public RectTransform rectTransform;         // 自身矩形变化
    public List<RectTransform> targetList;      // 目标列表
    public float smoothTime = 0.3f;             // 平滑时间
    public int nextIndex = 0;                   // 下一个目标的索引

    private Vector3 velocity;                   // 当前偏移速度

    public void Update()
    {
        rectTransform.position = Vector3.SmoothDamp(rectTransform.position, targetList[nextIndex].position, ref velocity, smoothTime);
    }
}
