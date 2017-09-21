using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SprialTrackCameraRig : MonoBehaviour 
{
    static public SprialTrackCameraRig Instance { get; private set; }

    public Transform target;
    public CinemachineVirtualCamera virtualCamera;
    public Camera targetCamera;
    public CinemachinePath cameraTrack;
    public float duration = 3f;
    public bool playeAtOnEnable = true;

    private float trackLength;
    private float moveRate;
    private float currentPos;
    private bool isStop = true;

    /// <summary>
    /// 初始单例
    /// </summary>
    private void Awake()
    {
        Instance = this;
        trackLength = cameraTrack.MaxPos - cameraTrack.MinPos;
        moveRate = trackLength / duration;
    }

    /// <summary>
    /// 如果选择了激活时启动相机动画，那就启动啊
    /// </summary>
    private void OnEnable()
    {
        if (playeAtOnEnable)
        {
            currentPos = cameraTrack.MinPos;
            isStop = false;
        }
    }

    /// <summary>
    /// 设置相机目标
    /// </summary>
    /// <param name="target">相机目标</param>
    public void SetTraget(Transform target)
    {
        virtualCamera.LookAt = target;
    }

    /// <summary>
    /// 更新轨道位置和相机在轨道的位置
    /// </summary>
    private void Update()
    {
        if (virtualCamera.LookAt == null || isStop)
            return;
        UpdateTrackAnchorPos();
    }

    /// <summary>
    /// 更新轨道位置
    /// </summary>
    public void UpdateTrackAnchorPos()
    {
        GameMathf.CopyPositionAndRotation(virtualCamera.LookAt, cameraTrack.transform);
    }

    /// <summary>
    /// 更新相机位置
    /// </summary>
    public void UpdateCameraPos()
    {
        //virtualCamera.
    }
}
