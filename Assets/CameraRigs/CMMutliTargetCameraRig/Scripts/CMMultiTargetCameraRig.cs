using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class CMMultiTargetCameraRig : MonoBehaviour 
{
    static public CMMultiTargetCameraRig Instance { get; private set; }

    public CinemachineVirtualCamera virtualCamera;
    public CinemachineTargetGroup targetGroup;

    private CinemachineTargetGroup.Target[] cmTargets;

    /// <summary>
    /// 初始单例
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 设置目标
    /// </summary>
    /// <param name="targets">目标列表</param>
    public void SetTargets(params Transform[] targets)
    {
        if (targets.Length <= 0)
            return;
        cmTargets = new CinemachineTargetGroup.Target[targets.Length];
        for (int i = 0; i < targets.Length; i++)
            cmTargets[i] = new CinemachineTargetGroup.Target() { target = targets[i], weight = 1f, radius = 1f };
        targetGroup.m_Targets = cmTargets;
    }

}
