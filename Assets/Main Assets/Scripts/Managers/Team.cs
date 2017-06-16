using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team
{
    public int TeamID;                              //团队ID
    public string TeamName;                         //团队名称
    public List<int> playerIdList;                  //成员列表ID
    [ColorUsage(false)] public Color TeamColor = Color.white;           //团队颜色

    public string NameColored                       //带颜色的团队名
    {
        get { return "<color=#" + ColorUtility.ToHtmlStringRGB(TeamColor) + ">" + TeamName + " </color>"; }
    }

    public int Count { get { return playerIdList.Count; } }             //获取成员数量
    public int this[int index] { get { return playerIdList[index]; } }  //成员索引器

    /// <summary>
    /// 是否包含该玩家
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    /// <returns>返回True如果包含。</returns>
    public bool Contains(int playerID)
    {
        return playerIdList.Contains(playerID);
    }


}
