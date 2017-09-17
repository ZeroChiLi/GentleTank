using UnityEngine;

[CreateAssetMenu(menuName = "Game Configure/Team Manager")]
public class TeamManager : ScriptableObject
{
    public int TeamID = -1;                                    //团队ID
    public string TeamName;                                    //团队名称
    public Color TeamColor = Color.white;                      //团队颜色


    // 带团队颜色的团队名称富文本
    public string ColoredTeamName { get { return "<color=#" + ColorUtility.ToHtmlStringRGB(TeamColor) + ">" + TeamName + " </color>"; } }
}