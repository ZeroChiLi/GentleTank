using System.Collections.Generic;
using UnityEngine;

public class ChargeAreaManager : MonoBehaviour 
{
    public PointList flagPointList;                             // 旗帜产生的位置
    public GameObject flagPerfab;                               // 旗帜预设

    private List<ChargeArea> flagList = new List<ChargeArea>(); // 所有旗帜
    private GameState lastGameState = GameState.None;           // 上一次回合状态，通过这个来触发旗帜状态变化

    /// <summary>
    /// 创建旗帜们
    /// </summary>
    private void Awake()
    {
        CreateFlags();
    }

    /// <summary>
    /// 通过点列表创建旗帜们
    /// </summary>
    private void CreateFlags()
    {
        for (int i = 0; i < flagPointList.Count; i++)
            flagList.Add(Instantiate(flagPerfab, flagPointList[i].position, Quaternion.Euler(flagPointList[i].rotation), transform).GetComponent<ChargeArea>());
    }

    /// <summary>
    /// 更新旗帜状态
    /// </summary>
    private void Update()
    {
        ChangeFlagsStateWithGameRoundState();
    }

    /// <summary>
    /// 通过游戏的回合状态改变旗帜是否激活状态
    /// </summary>
    private void ChangeFlagsStateWithGameRoundState()
    {
        if (lastGameState != GameState.Playing && GameRecord.Instance.CurrentGameState == GameState.Playing)
        {
            lastGameState = GameState.Playing;
            OpenFlags();
        }
        else if (lastGameState == GameState.Playing && GameRecord.Instance.CurrentGameState != GameState.Playing)
        {
            lastGameState = GameState.None;
            ShutDownFlags();
        }
    }

    /// <summary>
    /// 开启所有旗帜
    /// </summary>
    private void OpenFlags()
    {
        for (int i = 0; i < flagList.Count; i++)
            flagList[i].enabled = true;
    }

    /// <summary>
    /// 关闭所有旗帜，并重置
    /// </summary>
    private void ShutDownFlags()
    {
        for (int i = 0; i < flagList.Count; i++)
        {
            flagList[i].ResetChargeArea();
            flagList[i].enabled = false;
        }
    }

}
