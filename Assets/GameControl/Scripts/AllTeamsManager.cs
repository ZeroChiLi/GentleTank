using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Configure/All Teams Manager")]
public class AllTeamsManager : ScriptableObject
{
    //static private AllTeamsManager instance;                            // 团队管理器单例
    //static public AllTeamsManager Instance { get { return instance; } } 

    //public List<TeamManager> teamList;                          // 所有团队

    //private Dictionary<int, TeamManager> playerFromTeam;        // 玩家ID对应团队

    //public TeamManager this[int index]
    //{
    //    get { return teamList[index]; }
    //    set { teamList[index] = value; }
    //}

    //public int Length { get { return teamList.Count; } }

    ///// <summary>
    ///// 设置单例
    ///// </summary>
    //public void SetupInstance()
    //{
    //    instance = this;
    //}

    ///// <summary>
    ///// 启用时，初始化坦克队伍键值表
    ///// </summary>
    //private void OnEnable()
    //{
    //    playerFromTeam = new Dictionary<int, TeamManager>();
    //    if (teamList == null)
    //        teamList = new List<TeamManager>();
    //    for (int i = 0; i < teamList.Count; i++)
    //    {
    //        teamList[i].SetTeamID(i);
    //        for (int j = 0; j < teamList[i].Count; j++)
    //            playerFromTeam.Add(teamList[i][j], teamList[i]);
    //    }
    //}

    ///// <summary>
    ///// 是否包含该玩家
    ///// </summary>
    ///// <param name="playerID">玩家ID</param>
    ///// <returns>返回是否包含该玩家</returns>
    //public bool ContainsPlayer(int playerID)
    //{
    //    return playerFromTeam.ContainsKey(playerID);
    //}

    ///// <summary>
    ///// 判断两个玩家ID是否是队友
    ///// </summary>
    ///// <param name="id1">第一个玩家ID</param>
    ///// <param name="id2">第二个玩家ID</param>
    ///// <returns>返回是否是队友</returns>
    //public bool IsTeammate(int id1,int id2)
    //{
    //    if (!playerFromTeam.ContainsKey(id1) || !playerFromTeam.ContainsKey(id2))
    //        return false;
    //    return playerFromTeam[id1] == playerFromTeam[id2];
    //}

    ///// <summary>
    ///// 通过玩家ID获取房间
    ///// </summary>
    ///// <param name="playerID">玩家ID</param>
    ///// <returns>返回对应团队</returns>
    //public TeamManager GetTeamByPlayerID(int playerID)
    //{
    //    if (!playerFromTeam.ContainsKey(playerID))
    //        return null;
    //    return playerFromTeam[playerID];
    //}

    ///// <summary>
    ///// 获取玩家对应的团队颜色
    ///// </summary>
    ///// <param name="playerID">玩家ID</param>
    ///// <returns>返回对应团队颜色</returns>
    //public Color GetTeamColor(int playerID)
    //{
    //    if (!playerFromTeam.ContainsKey(playerID))
    //        return Color.white;
    //    return playerFromTeam[playerID].TeamColor;
    //}

    ///// <summary>
    ///// 将玩家添加到团队中
    ///// </summary>
    ///// <param name="playerID"></param>
    ///// <param name="teamID"></param>
    //public void AddToTeam(int playerID,TeamManager team)
    //{
    //    if (playerFromTeam.ContainsKey(playerID))
    //        RemoveFromTeam(playerID);
    //    team.Add(playerID);
    //    playerFromTeam[playerID] = team;
    //}

    ///// <summary>
    ///// 移除玩家出团队
    ///// </summary>
    ///// <param name="playerID">玩家ID</param>
    //public void RemoveFromTeam(int playerID)
    //{
    //    if (!playerFromTeam.ContainsKey(playerID))
    //        return;
    //    playerFromTeam[playerID].Remove(playerID);
    //    playerFromTeam.Remove(playerID);
    //}

}
