using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeamManager
{
    [SerializeField]
    private int teamID = -1;                                    //团队ID
    public string TeamName;                                     //团队名称
    [ColorUsage(false)] public Color TeamColor = Color.white;   //团队颜色
    public List<int> playerIdList;                              //成员列表ID

    public string ColoredTeamName                       //带颜色的团队名
    {
        get { return "<color=#" + ColorUtility.ToHtmlStringRGB(TeamColor) + ">" + TeamName + " </color>"; }
    }

    public int Count { get { return playerIdList.Count; } }

    public int TeamID { get { return teamID; } }

    public int this[int index] { get { return playerIdList[index]; } }  //成员索引器

    /// <summary>
    /// 设置团队ID
    /// </summary>
    /// <param name="teamId">团队ID</param>
    public void SetTeamID(int teamId)
    {
        this.teamID = teamId;
    }

    /// <summary>
    /// 是否包含该玩家
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    /// <returns>返回True如果包含。</returns>
    public bool Contains(int playerID)
    {
        return playerIdList.Contains(playerID);
    }

    /// <summary>
    /// 是否包含传入的所有玩家ID
    /// </summary>
    /// <param name="playerIDs">玩家ID数组</param>
    /// <returns>是否包含所有玩家ID</returns>
    public bool Contains(params int[] playerIDs)
    {
        for (int i = 0; i < playerIDs.Length; i++)
            if (!playerIdList.Contains(playerIDs[i]))
                return false;
        return true;
    }

    /// <summary>
    /// 添加玩家到该团队
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    public void Add(int playerID)
    {
        playerIdList.Add(playerID);
    }

    /// <summary>
    /// 将玩家移除该团队
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    public void Remove(int playerID)
    {
        playerIdList.Remove(playerID);
    }


}
