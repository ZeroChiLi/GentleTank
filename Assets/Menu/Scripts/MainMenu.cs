using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour 
{
    public CinemachineVirtualCamera cmMainMenuCamera;
    public CinemachineVirtualCamera cmArmsMenuCamera;
    public TMPButton[] tmpButtons;

    private CinemachineTrackedDolly track;

    private void Awake()
    {
        track = cmArmsMenuCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    /// <summary>
    /// 主菜单到装备菜单
    /// </summary>
    public void MainToArms()
    {
        cmMainMenuCamera.enabled = false;
        cmArmsMenuCamera.enabled = true;
        EnableAllButtons(false);
    }

    /// <summary>
    /// 装备菜单到主菜单
    /// </summary>
    public void ArmsToMain()
    {
        cmMainMenuCamera.enabled = true;
        cmArmsMenuCamera.enabled = false;
        EnableAllButtons(true);
    }

    /// <summary>
    /// 设置所有按钮的可交互性
    /// </summary>
    public void EnableAllButtons(bool enable)
    {
        for (int i = 0; i < tmpButtons.Length; i++)
            tmpButtons[i].enabled = enable;
    }


    /// <summary>
    /// 设置当前装备相机在轨道的位置
    /// </summary>
    /// <param name="pos"></param>
    public void SetPosition(float pos)
    {
        track.m_PathPosition = pos;
    }

}
