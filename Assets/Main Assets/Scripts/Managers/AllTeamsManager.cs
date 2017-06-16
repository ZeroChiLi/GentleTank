using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Configure/All Teams Manager")]
public class AllTeamsManager : ScriptableObject
{
    public Team[] TeamArray;                                //所有团队

    private Dictionary<int, int> playerIdTeamIdDic;         //玩家ID和对应团队ID字典
    private bool isCreatedDictionray = false;               //是否已经创建了字典

    /// <summary>
    /// 如果还没创建，那就创建字典。
    /// </summary>
    public void CreateDictionaryIfNeed()
    {
        if (isCreatedDictionray)
            return;
        playerIdTeamIdDic = new Dictionary<int, int>();
        for (int i = 0; i < TeamArray.Length; i++)
            for (int j = 0; j < TeamArray[i].Count; j++)
                playerIdTeamIdDic[TeamArray[i][j]] = TeamArray[i].TeamID;
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
        return playerIdTeamIdDic[id1] == playerIdTeamIdDic[id2];
    }

    /// <summary>
    /// 通过玩家ID获取房间
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    /// <returns>返回对应团队</returns>
    public Team GetTeam(int playerID)
    {
        CreateDictionaryIfNeed();
        for (int i = 0; i < TeamArray.Length; i++)
            if (TeamArray[i].TeamID == playerIdTeamIdDic[playerID]) //先判断是否对应玩家ID的团队ID
            {
                if (TeamArray[i].Contains(playerID))
                    return TeamArray[i];
                return null;
            }
        return null;
    }

    /// <summary>
    /// 获取玩家对应的团队颜色
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    /// <returns>返回对应团队颜色</returns>
    public Color GetTeamColor(int playerID)
    {
        CreateDictionaryIfNeed();
        if (!playerIdTeamIdDic.ContainsKey(playerID))
            return Color.white;
        return GetTeam(playerID).TeamColor;
    }

}
