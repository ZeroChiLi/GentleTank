using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "All Player Manager")]
public class AllPlayerManager : ScriptableObject
{
    static private AllPlayerManager instance;                           // 所有玩家列表单例
    static public AllPlayerManager Instance { get { return instance; } }

    public List<PlayerInformation> playerInformationList;               // 玩家信息列表（用于外部配置创建游戏对象）

    private List<PlayerManager> playerManagerList;                      // 玩家列表（用于作为组件添加到游戏对象）

    public int Count { get { return playerManagerList.Count; } }        // 玩家数量

    public PlayerManager this[int index] { get { return playerManagerList[index]; } }

    /// <summary>
    /// 配置单例
    /// </summary>
    public void SetupInstance()
    {
        instance = this;
    }

    /// <summary>
    /// 创建玩家对象们（playerManagerList）
    /// </summary>
    public void CreatePlayerGameObjects()
    {
        playerManagerList = new List<PlayerManager>();
        for (int i = 0; i < playerInformationList.Count; i++)
            playerManagerList.Add(playerInformationList[i].CreateGameObjectWithPlayerManager());
    }

    /// <summary>
    /// 是否包含玩家
    /// </summary>
    /// <param name="player">玩家</param>
    /// <returns>是否存在在列表中</returns>
    public bool Contain(PlayerManager player)
    {
        return playerManagerList.Contains(player);
    }

    /// <summary>
    /// 添加玩家
    /// </summary>
    /// <param name="player">新玩家</param>
    public void Add(PlayerManager player)
    {
        if (Contain(player))
            return;
        playerManagerList.Add(player);
    }

    /// <summary>
    /// 移除玩家
    /// </summary>
    /// <param name="player">要移除的玩家</param>
    public void Remove(PlayerManager player)
    {
        if (!Contain(player))
            return;
        playerManagerList.Remove(player);
    }

    /// <summary>
    /// 判断两人是否是队友
    /// </summary>
    /// <param name="player1">玩家1</param>
    /// <param name="player2">玩家2</param>
    /// <returns></returns>
    public bool IsTeammate(PlayerManager player1,PlayerManager player2)
    {
        if (!Contain(player1) || !Contain(player2))
            return false;
        return player1.IsTeammate(player2);
    }

    /// <summary>
    /// 获取第一个归自己控制的玩家
    /// </summary>
    /// <returns>自己控制的玩家</returns>
    public PlayerManager GetMyPlayer()
    {
        for (int i = 0; i < playerManagerList.Count; i++)
            if (playerManagerList[i].IsMine)
                return playerManagerList[i];
        return null;
    }
}
