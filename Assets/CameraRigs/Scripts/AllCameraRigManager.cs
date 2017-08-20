using System.Collections.Generic;
using UnityEngine;
using CameraRig;

public class AllCameraRigManager : MonoBehaviour
{
    static private AllCameraRigManager instance;
    static public AllCameraRigManager Instance { get { return instance; } }

    public AutoCam autoCam;
    public MultiCam multiCam;

    public Camera CurrentCamera { get; private set; }

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
        TurnToMultiCam();
    }

    /// <summary>
    /// 初始化镜头追踪对象
    /// </summary>
    /// <param name="target">单独追踪对象</param>
    /// <param name="targets">所有对象</param>
    public void Init(Transform target, List<Transform> targets)
    {
        autoCam.SetTarget(target);
        multiCam.targets = targets;
    }

    /// <summary>
    /// 转为单独追踪
    /// </summary>
    public void TurnToAutoCam()
    {
        multiCam.gameObject.SetActive(false);
        autoCam.gameObject.SetActive(true);
        CurrentCamera = autoCam.controlCamera;
    }

    /// <summary>
    /// 转为所有对象
    /// </summary>
    public void TurnToMultiCam()
    {
        autoCam.gameObject.SetActive(false);
        multiCam.gameObject.SetActive(true);
        CurrentCamera = multiCam.controlCamera;
    }

    /// <summary>
    /// 设置单独追踪的对象
    /// </summary>
    /// <param name="target">单独追踪的对象</param>
    public void SetAutoCamTarget(Transform target)
    {
        autoCam.SetTarget(target);
    }
}
