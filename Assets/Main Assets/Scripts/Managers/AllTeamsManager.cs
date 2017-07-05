using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Configure/All Teams Manager")]
public class AllTeamsManager : ScriptableObject
{
    [Header("Open 'Window/Tanks To Team',")]
    [Header("And Select Player's Team.")]
    [Space()]

    public TeamManager[] teamArray;                                // 所有团队

    public TeamManager this[int index]
    {
        get { return teamArray[index]; }
        set { teamArray[index] = value; }
    }

    public int Length { get { return teamArray.Length; } }

    /// <summary>
    /// 是否包含该玩家
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    /// <returns>返回是否包含该玩家</returns>
    public bool ContainsPlayer(int playerID)
    {
        for (int i = 0; i < teamArray.Length; i++)
            if (teamArray[i].Contains(playerID))
                return true;
        return false;
    }

    /// <summary>
    /// 判断两个玩家ID是否是队友
    /// </summary>
    /// <param name="id1">第一个玩家ID</param>
    /// <param name="id2">第二个玩家ID</param>
    /// <returns>返回是否是队友</returns>
    public bool IsTeammate(int id1,int id2)
    {
        for (int i = 0; i < teamArray.Length; i++)
        {
            if (teamArray[i].Contains(id1))
            {
                if (teamArray[i].Contains(id2))
                    return true;
                return false;
            }
            if (teamArray[i].Contains(id2))
            {
                if (teamArray[i].Contains(id1))
                    return true;
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// 通过玩家ID获取房间
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    /// <returns>返回对应团队</returns>
    public TeamManager GetTeamByPlayerID(int playerID)
    {
        for (int i = 0; i < teamArray.Length; i++)
            if (teamArray[i].Contains(playerID))
                return teamArray[i];
        return null;
    }

    /// <summary>
    /// 通过团队ID获取团队
    /// </summary>
    /// <param name="teamID">团队ID</param>
    /// <returns>返回对应团队</returns>
    public TeamManager GetTeamByTeamID(int teamID)
    {
        for (int i = 0; i < teamArray.Length; i++)
            if (teamArray[i].TeamID == teamID)
                return teamArray[i];
        return null;
    }

    /// <summary>
    /// 获取玩家对应的团队颜色
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    /// <returns>返回对应团队颜色</returns>
    public Color GetTeamColor(int playerID)
    {
        if (!ContainsPlayer(playerID))
            return Color.white;
        return GetTeamByPlayerID(playerID).TeamColor;
    }

    /// <summary>
    /// 将玩家添加到团队中
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="teamID"></param>
    public void AddToTeam(int playerID,int teamID)
    {
        if (ContainsPlayer(playerID))
            GetTeamByPlayerID(playerID).Remove(playerID);
        GetTeamByTeamID(teamID).Add(playerID);
    }

    /// <summary>
    /// 移除玩家出团队
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    public void RemoveFromTeam(int playerID)
    {
        TeamManager team = GetTeamByPlayerID(playerID);
        if (team != null && team.Contains(playerID))
            team.Remove(playerID);
    }

}
