using UnityEngine;

[CreateAssetMenu(menuName = "Game Configure/Team Manager")]
public class TeamManager : ScriptableObject
{
    [SerializeField]
    private int teamID = -1;                                    //团队ID
    [SerializeField]
    private string teamName;                                    //团队名称
    [SerializeField]
    private Color teamColor = Color.white;                      //团队颜色

    public int TeamID { get { return teamID; } }
    public string TeamName { get { return teamName; } }
    public Color TeamColor { get { return teamColor; } }

    // 带团队颜色的团队名称富文本
    public string ColoredTeamName { get { return "<color=#" + ColorUtility.ToHtmlStringRGB(TeamColor) + ">" + TeamName + " </color>"; } }
}