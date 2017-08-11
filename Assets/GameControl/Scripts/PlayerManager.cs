using UnityEngine;

[System.Serializable]
public class PlayerInformation
{
    public int id = -1;                     // 玩家ID
    public string name;                     // 玩家名称
    public bool isMine;                     // 玩家是否是自己控制的
    public bool isActive;                   // 玩家是否激活
    public bool isAI;                       // 玩家是否是AI
    public GameObject perfab;               // 玩家预设
    public GameObject inactivePerfab;       // 玩家失效预设
    public Color representColor;            // 玩家代表颜色
    public PlayerTeamManager team;                // 玩家所在的团队，没有则为null

    /// <summary>
    /// 创建游戏对象实例，并把该玩家信息作为组件（PlayerManager）添加进去
    /// </summary>
    /// <returns>返回玩家信息管理</returns>
    public PlayerManager CreateGameObjectWithPlayerManager(Transform parent = null)
    {
        GameObject gameObject = Object.Instantiate(perfab);
        if (parent != null)
            gameObject.transform.parent = parent;
        // 如果已经有该组件，直接修改。如果没有该组件，创建之
        if (gameObject.GetComponent<PlayerManager>() == null)
        {
            Debug.Log("ASIOFJSJJJJJJJJJJJJJJJJJJJJJ");
        }
        PlayerManager playerManager = gameObject.GetComponent<PlayerManager>() ?? gameObject.AddComponent<PlayerManager>();
        playerManager.SetInformation(this);
        return playerManager;
    }
}

public class PlayerManager : MonoBehaviour
{
    private PlayerInformation information;      // 玩家信息

    public int PlayerID { get { return information.id; } }
    public string PlayerName { get { return information.name; } }
    public bool IsMine { get { return information.isMine; } }
    public bool Active { get { return information.isActive; } }
    public bool IsAI { get { return information.isAI; } }
    public GameObject Perfab { get { return information.perfab; } }
    public GameObject InactivePerfab { get { return information.inactivePerfab; } }
    public Color RepresentColor { get { return information.representColor; } }
    public PlayerTeamManager Team { get { return information.team; } }

    // 带玩家颜色的玩家名字富文本
    public string ColoredPlayerName { get { return "<color=#" + ColorUtility.ToHtmlStringRGB(RepresentColor) + ">" + PlayerName + "</color>"; ; } }

    /// <summary>
    /// 设置玩家信息
    /// </summary>
    /// <param name="playerInfo">玩家信息</param>
    public void SetInformation(PlayerInformation playerInfo)
    {
        information = playerInfo;
    }

    /// <summary>
    /// 判断传入玩家是否是队友
    /// </summary>
    /// <param name="player">另一个玩家</param>
    /// <returns>是否是队友</returns>
    public bool IsTeammate(PlayerManager player)
    {
        if (Team == null || player.Team == null)
            return false;

        if (Team == player.Team)
            return true;
        else
            return false;
    }

}
