using System.Collections.Generic;
using UnityEngine;
using CameraRig;
using Cinemachine;
using UnityEngine.Events;

public class AllCameraRigManager : MonoBehaviour
{
    static private AllCameraRigManager instance;
    static public AllCameraRigManager Instance { get { return instance; } }

    public enum CameraRigType
    {
        AutoFollow, MultiTarget
    }

    public Camera CurrentCamera { get { return GetCurrentCamera(); } }

    public AutoFollowCamareRig autoCameraRig;
    public MultiCameraRig multiCameraRig;
    public CameraRigType currentCameraRigType = CameraRigType.MultiTarget;
    public UnityEvent OnCameraChangeEvent;

    /// <summary>
    /// 初始单例
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 初始化镜头追踪对象
    /// </summary>
    /// <param name="target">单独追踪对象</param>
    /// <param name="targets">所有对象</param>
    public void Init(Transform target, List<Transform> targets)
    {
        autoCameraRig.SetTarget(target);
        multiCameraRig.targets = targets;
    }

    /// <summary>
    /// 关闭所有相机设备
    /// </summary>
    public void InactiveAllCameraRig()
    {
        autoCameraRig.gameObject.SetActive(false);
        multiCameraRig.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置单独追踪的对象
    /// </summary>
    /// <param name="target">单独追踪的对象</param>
    public void SetTarget(Transform target)
    {
        autoCameraRig.SetTarget(target);
    }

    /// <summary>
    /// 变换相机设备
    /// </summary>
    /// <param name="cameraRigType">相机类型</param>
    public void ChangeCameraRig(CameraRigType cameraRigType)
    {
        if (currentCameraRigType == cameraRigType)
            return;
        currentCameraRigType = cameraRigType;
        InactiveAllCameraRig();
        switch (cameraRigType)
        {
            case CameraRigType.AutoFollow:
                autoCameraRig.gameObject.SetActive(true);
                break;
            case CameraRigType.MultiTarget:
                multiCameraRig.gameObject.SetActive(true);
                break;
        }
        OnCameraChangeEvent.Invoke();
    }

    /// <summary>
    /// 方便动画后的事件调用，转换到跟踪相机
    /// </summary>
    public void ChangeToAutoFollowCameraRig()
    {
        ChangeCameraRig(CameraRigType.AutoFollow);
    }

    public Camera GetCurrentCamera()
    {
        switch (currentCameraRigType)
        {
            case CameraRigType.AutoFollow:
                return autoCameraRig.controlCamera;
            case CameraRigType.MultiTarget:
                return multiCameraRig.controlCamera;
        }
        return null;
    }

}
