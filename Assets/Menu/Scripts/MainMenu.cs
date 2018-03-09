using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public CinemachineVirtualCamera cmMainMenuCamera;
    public CinemachineVirtualCamera cmArmsMenuCamera;
    public CinemachineVirtualCamera cmSettingsMenuCamera;
    public float dollySmoothTime = 0.3f;                     // 平滑时间
    public TMPButton[] tmpButtons;

    private CinemachineTrackedDolly track;
    private float dollyPos;
    private float dollyVelocity;

    private void Awake()
    {
        track = cmArmsMenuCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    private void Update()
    {
        track.m_PathPosition = Mathf.SmoothDamp(track.m_PathPosition, dollyPos, ref dollyVelocity, dollySmoothTime);
    }

    /// <summary>
    /// 加载单人模式场景
    /// </summary>
    public void LoadSoloScene()
    {
        AllSceneManager.LoadScene(AllSceneManager.GameSceneType.SoloScene);
    }

    /// <summary>
    /// 到装备菜单
    /// </summary>
    public void GoToArmsMenu()
    {
        DisableAllCMCameras();
        cmArmsMenuCamera.enabled = true;
        EnableAllButtons(false);
    }

    /// <summary>
    /// 到设置菜单
    /// </summary>
    public void GoToSettingsMenu()
    {
        DisableAllCMCameras();
        cmSettingsMenuCamera.enabled = true;
        EnableAllButtons(false);
    }

    /// <summary>
    /// 回到主菜单
    /// </summary>
    public void BackToMainMenu()
    {
        DisableAllCMCameras();
        cmMainMenuCamera.enabled = true;
        EnableAllButtons(true);
    }

    /// <summary>
    /// 失效所有相机位
    /// </summary>
    private void DisableAllCMCameras()
    {
        cmMainMenuCamera.enabled = false;
        cmArmsMenuCamera.enabled = false;
        cmArmsMenuCamera.enabled = false;
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
        dollyPos = pos;
    }

    /// <summary>
    /// 设置装备相机到默认选中坦克位置
    /// </summary>
    public void SetToCurrentTankPos()
    {
        dollyPos = AllCustomTankManager.Instance.CurrentIndex;
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
