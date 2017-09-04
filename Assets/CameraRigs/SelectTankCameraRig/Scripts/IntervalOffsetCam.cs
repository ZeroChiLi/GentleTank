using System;
using CameraRig;
using UnityEngine;
using System.Collections;

public class IntervalOffsetCam : MonoBehaviour
{
    public new Camera camera;
    public Vector3 startPosition;
    public Vector3 offset;
    public int index;
    public float smoothTime = 0.3f;
    public bool enableSmooth;

    private CountDownTimer timer;
    private Vector3 velocity;
    private Vector3 temPos;

    /// <summary>
    /// 立即变换位置
    /// </summary>
    /// <param name="index">索引值</param>
    public void FollowImmediately(int index)
    {
        enableSmooth = false;
        transform.localPosition = startPosition + (index * offset);
    }

    /// <summary>
    /// 如果开启平滑移动，自动移动
    /// </summary>
    private void Update()
    {
        if (enableSmooth)
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, startPosition + (index * offset), ref velocity, smoothTime);
    }

    /// <summary>
    /// 跟随目标索引
    /// </summary>
    /// <param name="index">索引值</param>
    public void FollowTarget(int index)
    {
        this.index = index;
    }

}
