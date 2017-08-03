using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoommatesPanelManager : MonoBehaviour
{
    public GameObject othersPanelPerfab;                    // 单个其他玩家面板预设
    public List<EmptyPanelManager> emptyPanelList;          // 其他玩家空面板列表

    private int enableCount;                                // 有效玩家面板数量

    /// <summary>
    /// 初始化有效面板数量
    /// </summary>
    /// <param name="playerMaxCount">房间玩家总数</param>
    public void Init(int playerMaxCount)
    {
        enableCount = Mathf.Max(0,emptyPanelList.Count + 1 - playerMaxCount);
        Debug.Log(emptyPanelList.Count + "  " + playerMaxCount+ "  " + enableCount);
        for (int i = 0; i < enableCount; i++)
            emptyPanelList[emptyPanelList.Count - 1 - i].IsEnable = false;
    }

}
