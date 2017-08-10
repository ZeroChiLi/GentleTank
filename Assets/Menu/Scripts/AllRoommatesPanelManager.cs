using System.Collections.Generic;
using UnityEngine;


public class AllRoommatesPanelManager : MonoBehaviour
{
    public GameObject othersPanelPerfab;                    // 单个其他玩家面板预设
    public List<PlayerPanelManager> othersPanelList;        // 其他玩家空板列表

    private int enableCount;                                // 有效其他玩家面板数量
    private Dictionary<PhotonPlayer, PlayerPanelManager> playerPanelDic;        // 玩家对应面板字典
    private PhotonPlayer temPlayer;                         // 临时玩家变量
    private PhotonView temPhotonView;                       // 同步视角组件

    public PlayerPanelManager this[PhotonPlayer player]
    {
        get
        {
            if (playerPanelDic.ContainsKey(player))
                return playerPanelDic[player];
            return null;
        }
    }

    /// <summary>
    /// 初始化有效面板数量
    /// </summary>
    /// <param name="playerMaxCount">房间玩家总数</param>
    public void Init(int playerMaxCount)
    {
        playerPanelDic = new Dictionary<PhotonPlayer, PlayerPanelManager>();
        enableCount = Mathf.Max(0, playerMaxCount - 1);     // 除去自己剩余有效玩家面板数量
        for (int i = 0; i < othersPanelList.Count; i++)
        {
            othersPanelList[i].Clear();
            if (i >= enableCount)
                othersPanelList[i].IsEnable = false;
        }
    }

    /// <summary>
    /// 是否存在可用面板
    /// </summary>
    /// <returns>是否存在可用面板</returns>
    public bool ExistEnablePanel()
    {
        return enableCount > 0;
    }

    /// <summary>
    /// 是否包含该玩家
    /// </summary>
    /// <param name="player">玩家</param>
    /// <returns>是否包含该玩家</returns>
    public bool ContainPlayer(PhotonPlayer player)
    {
        return playerPanelDic.ContainsKey(player);
    }

    /// <summary>
    /// 获取可用且为空的面板
    /// </summary>
    /// <returns>可用且为空的面板</returns>
    public PlayerPanelManager GetEmptyPanel()
    {
        if (!ExistEnablePanel())
            return null;
        for (int i = 0; i < enableCount; i++)
            if (othersPanelList[i].IsEmpty)
                return othersPanelList[i];
        return null;
    }

    /// <summary>
    /// 添加新玩家
    /// </summary>
    /// <param name="player">玩家</param>
    public void AddPlayer(PhotonPlayer player)
    {
        if (playerPanelDic.ContainsKey(player) || !ExistEnablePanel())  // 如果已经存在或者不存在但没有可用的，直接返回
            return;
        playerPanelDic.Add(player, GetEmptyPanel());
        playerPanelDic[player].SetupInfo(player, player.IsMasterClient);
    }

    /// <summary>
    /// 更新玩家信息
    /// </summary>
    /// <param name="player">玩家</param>
    /// <param name="color">玩家坦克颜色</param>
    public void UpdatePlayerInfo(PhotonPlayer player, Color color)
    {
        if (!playerPanelDic.ContainsKey(player))
            return;
        playerPanelDic[player].SetColor(color);
    }

    /// <summary>
    /// 如果存在玩家，更新信息。如果不存在，添加且更新信息
    /// </summary>
    /// <param name="player">玩家</param>
    /// <param name="color">玩家颜色</param>
    public void AddOrUpdatePlayer(PhotonPlayer player, Color color)
    {
        if (!playerPanelDic.ContainsKey(player))
            AddPlayer(player);
        UpdatePlayerInfo(player, color);
    }

    /// <summary>
    /// 移除玩家
    /// </summary>
    /// <param name="player">要移除的玩家</param>
    public void RemovePlayer(PhotonPlayer player)
    {
        if (!playerPanelDic.ContainsKey(player))
            return;
        playerPanelDic[player].Clear();
        playerPanelDic.Remove(player);
    }

}
