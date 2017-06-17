using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Configure/All Teams Manager")]
public class AllTeamsManager : ScriptableObject
{
    public Team[] teamArray;                                // 所有团队

    private Dictionary<int, int> TeamIdTeamIndexDic;        // 团队ID 和 对应团队 字典
    private Dictionary<int, int> playerIdTeamIdDic;         // 玩家ID 和 对应团队ID 字典
    private bool isCreatedDictionray = false;               // 是否已经创建了字典

    public Team this[int index]
    {
        get { return teamArray[index]; }
        set { teamArray[index] = value; }
    }

    public int Length { get { return teamArray.Length; } }

    /// <summary>
    /// 如果还没创建，那就创建字典。
    /// </summary>
    public void CreateDictionaryIfNeed()
    {
        if (isCreatedDictionray)
            return;

        TeamIdTeamIndexDic = new Dictionary<int, int>();
        for (int i = 0; i < teamArray.Length; i++)
            TeamIdTeamIndexDic[teamArray[i].TeamID] = i;

        playerIdTeamIdDic = new Dictionary<int, int>();
        for (int i = 0; i < teamArray.Length; i++)
            for (int j = 0; j < teamArray[i].Count; j++)
                playerIdTeamIdDic[teamArray[i][j]] = teamArray[i].TeamID;
    }
    
    /// <summary>
    /// 是否包含该玩家
    /// </summary>
    /// <param name="id">玩家ID</param>
    /// <returns>返回是否包含该玩家</returns>
    public bool ContainsPlayer(int id)
    {
        CreateDictionaryIfNeed();
        return playerIdTeamIdDic.ContainsKey(id);
    }

    /// <summary>
    /// 判断两个玩家ID是否是队友
    /// </summary>
    /// <param name="id1">第一个玩家ID</param>
    /// <param name="id2">第二个玩家ID</param>
    /// <returns>返回是否是队友</returns>
    public bool IsTeammate(int id1,int id2)
    {
        CreateDictionaryIfNeed();
        if (!ContainsPlayer(id1) || !ContainsPlayer(id2))
            return false;
        return playerIdTeamIdDic[id1] == playerIdTeamIdDic[id2];
    }

    /// <summary>
    /// 通过玩家ID获取房间
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    /// <returns>返回对应团队</returns>
    public Team GetTeamByPlayerID(int playerID)
    {
        CreateDictionaryIfNeed();
        if (!ContainsPlayer(playerID))
            return null;
        for (int i = 0; i < teamArray.Length; i++)
            if (teamArray[i].TeamID == playerIdTeamIdDic[playerID]) //先判断是否对应玩家ID的团队ID
            {
                if (teamArray[i].Contains(playerID))
                    return teamArray[i];
                return null;
            }
        return null;
    }

    /// <summary>
    /// 通过团队ID获取团队
    /// </summary>
    /// <param name="teamID">团队ID</param>
    /// <returns>返回对应团队</returns>
    public Team GetTeamByTeamID(int teamID)
    {
        CreateDictionaryIfNeed();
        return teamArray[TeamIdTeamIndexDic[teamID]];
    }

    /// <summary>
    /// 获取玩家对应的团队颜色
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    /// <returns>返回对应团队颜色</returns>
    public Color GetTeamColor(int playerID)
    {
        CreateDictionaryIfNeed();
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
        playerIdTeamIdDic[playerID] = teamID;
    }

}
