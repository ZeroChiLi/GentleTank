using System.Collections.Generic;
using UnityEngine;
using CameraRig;
using Cinemachine;
using UnityEngine.Playables;
using System.Collections;

public class AllCameraRigManager : MonoBehaviour
{
    static private AllCameraRigManager instance;
    static public AllCameraRigManager Instance { get { return instance; } }

    public enum CameraRigType
    {
        AutoFollow, MultiTarget, CMFollow, CMMultiTarget
    }

    public AutoFollowCamareRig autoCameraRig;
    public MultiCameraRig multiCameraRig;
    public CinemachineBrain cmCameraBrain;
    public CMFollowCameraRig cmFollowCameraRig;
    public CMMultiTargetCameraRig cmMultiTargetCameraRig;
    public CameraRigType currentCameraRigType = CameraRigType.CMMultiTarget;

    /// <summary>
    /// 初始单例
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 初始化为多人相机
    /// </summary>
    private void Start()
    {
        //ChangeCameraRig(currentCameraRigType);
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
        cmFollowCameraRig.SetTarget(target);
        cmMultiTargetCameraRig.SetTargets(targets.ToArray());
    }

    /// <summary>
    /// 关闭所有相机设备
    /// </summary>
    public void InactiveAllCameraRig()
    {
        autoCameraRig.gameObject.SetActive(false);
        multiCameraRig.gameObject.SetActive(false);
        //cmFollowCameraRig.gameObject.SetActive(false);
        //cmMultiTargetCameraRig.gameObject.SetActive(false);
        //cmCameraBrain.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置单独追踪的对象
    /// </summary>
    /// <param name="target">单独追踪的对象</param>
    public void SetTarget(Transform target)
    {
        autoCameraRig.SetTarget(target);
        cmFollowCameraRig.SetTarget(target);
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
            case CameraRigType.CMFollow:
                cmFollowCameraRig.followCamera.enabled = true;
                cmMultiTargetCameraRig.virtualCamera.enabled = false;
                break;
            case CameraRigType.CMMultiTarget:
                cmFollowCameraRig.followCamera.enabled = false;
                cmMultiTargetCameraRig.virtualCamera.enabled = true;
                break;
        }
    }

    public void ActiveCMCameraRig()
    {
        cmCameraBrain.gameObject.SetActive(true);
        cmFollowCameraRig.gameObject.SetActive(true);
        cmMultiTargetCameraRig.gameObject.SetActive(true);
    }

    private void CMFollowMultiTrigger(bool isFollow)
    {
        cmFollowCameraRig.followCamera.enabled = isFollow;
        cmMultiTargetCameraRig.virtualCamera.enabled = !isFollow;
    }

    /// <summary>
    /// 方便动画后的事件调用，转换到跟踪相机
    /// </summary>
    public void ChangeToAutoFollowCameraRig()
    {
        ChangeCameraRig(CameraRigType.AutoFollow);
    }

}
