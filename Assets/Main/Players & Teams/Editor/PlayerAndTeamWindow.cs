using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;

public class PlayerAndTeamWindow : EditorWindow
{
    public AllPlayerManager players;
    public List<PlayerInformation> playerInfoList;
    public List<TeamManager> teams = new List<TeamManager>();

    private bool showTeamList = true;           // 是否显示团队列表
    private int teamsSize = 2;                  // 团队数量
    private Vector2 scrollPos;                  // 滑动面板位置
    private bool openAllInfo;                   // 是打开所有玩家信息列表
    private bool[] playerShow;                  // 对应坦克管理是否显示在面板
    private GUIContent content;                 // 临时GUI内容

    [MenuItem("Window/Player And Team")]
    static void ShowWindows()
    {
        EditorWindow window = GetWindow<PlayerAndTeamWindow>();
        window.minSize = new Vector2(500f, 280f);
        window.Show();
    }

    private void OnGUI()
    {
        if (!GetPlayers())
            return;
        SelectedMyPlayer();
        GetTeams();
        GUILayout.Label("============================================================================================================");
        OpenOrCloseAllPlayerInfo();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        PlayersAndTeamsOperation();
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// 获取玩家列表，强制重置玩家ID
    /// </summary>
    /// <returns>成功返回ture</returns>
    private bool GetPlayers()
    {
        players = EditorGUILayout.ObjectField("All Players", players, typeof(AllPlayerManager), false) as AllPlayerManager;
        if (players == null)
            return false;
        playerInfoList = players.playerInfoList;
        if (playerInfoList == null || playerInfoList.Count <= 0)
        {
            EditorGUILayout.HelpBox("Waring : playerInfoList Is Empty", MessageType.Warning);
            return false;
        }
        for (int i = 0; i < playerInfoList.Count; i++)
            playerInfoList[i].id = i;
        if (playerShow == null || playerShow.Length != playerInfoList.Count)
            playerShow = new bool[playerInfoList.Count];
        return true;
    }

    /// <summary>
    /// 选择我的玩家
    /// </summary>
    private void SelectedMyPlayer()
    {
        Horizontal(true);
        EditorGUILayout.PrefixLabel("Select My Player : ");
        if (EditorGUILayout.DropdownButton(GetContentByPlayerIndex(players.myPlayerIndex), FocusType.Passive))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("None"), false, () => { players.myPlayerIndex = -1; });
            for (int i = 0; i < playerInfoList.Count; i++)
            {
                if (players.myPlayerIndex == i)
                    continue;
                menu.AddItem(GetContentByPlayerIndex(i), false, (object index) => { players.myPlayerIndex = (int)index; }, i);
            }
            menu.ShowAsContext();
        }
        Horizontal(false);
    }

    /// <summary>
    /// 获取索引对应玩家内容信息（ID 和 名字），失败为空
    /// </summary>
    /// <param name="index">索引值</param>
    /// <returns>返回信息</returns>
    private GUIContent GetContentByPlayerIndex(int index)
    {
        content = new GUIContent();
        if (GameMathf.InRange(index, 0, playerInfoList.Count - 1))
            content.text = string.Format("ID : {0}   Name : {1}", playerInfoList[index].id, playerInfoList[index].name);
        return content;
    }

    /// <summary>
    /// 获取团队列表，并且重置ID
    /// </summary>
    private void GetTeams()
    {
        showTeamList = EditorGUILayout.Foldout(showTeamList, "Team List");
        if (!showTeamList)
            return;
        EditorGUI.indentLevel = 1;
        teamsSize = EditorGUILayout.IntField("Size", teamsSize);
        if (teams.Count > teamsSize)                                        // 如果大小改小了，删掉
            teams.RemoveRange(teamsSize, teams.Count - teamsSize);
        for (int i = 0; i < teamsSize; i++)
        {
            if (teams.Count < teamsSize)                                    // 如果大了，添加
                teams.Add(null);
            teams[i] = EditorGUILayout.ObjectField("Team " + i, teams[i], typeof(TeamManager), false) as TeamManager;
            teams[i].TeamID = i;
        }
        EditorGUI.indentLevel = 0;
    }

    /// <summary>
    /// 打开我的玩家列表、全部打开、全部关闭玩家信息列表按钮
    /// </summary>
    private void OpenOrCloseAllPlayerInfo()
    {
        Horizontal(true);
        if (GUILayout.Button("Open My Player") && GameMathf.InRange(players.myPlayerIndex, 0, playerInfoList.Count - 1))
        {
            AllPlayerInfoTrigger(false);
            playerShow[players.myPlayerIndex] = true;
        }
        if (GUILayout.Button("Open All"))
            AllPlayerInfoTrigger(true);
        if (GUILayout.Button("Close All"))
            AllPlayerInfoTrigger(false);
        Horizontal(false);
    }

    /// <summary>
    /// 所有玩家信息列表开关
    /// </summary>
    /// <param name="open">是否打开（否则关闭）</param>
    private void AllPlayerInfoTrigger(bool open)
    {
        for (int i = 0; i < playerShow.Length; i++)
            playerShow[i] = open;
    }

    /// <summary>
    /// 玩家和团队选择列表
    /// </summary>
    private void PlayersAndTeamsOperation()
    {
        for (int i = 0; i < playerInfoList.Count; i++)
        {
            EditorGUILayout.BeginVertical("Box");
            PlayerOperation(i);
            EditorGUILayout.EndVertical();
        }
    }

    /// <summary>
    /// 单个玩家管理
    /// </summary>
    /// <param name="index">当前玩家索引</param>
    private void PlayerOperation(int index)
    {
        if (!ShowPlayerTeamInfo(index))
            return;
        GUILayout.Space(5);
        
        Horizontal(true);
        EditorGUILayout.PrefixLabel("Select Player's Team : ");
        EditorGUILayout.BeginVertical();

        ShowDropdown(index);                                    //显示下拉菜单
        ShowTeamInfo(playerInfoList[index].team);               //显示选中团队信息

        EditorGUILayout.EndVertical();
        Horizontal(false);
    }

    /// <summary>
    /// 显示玩家信息，并返回是否显示团队选择信息
    /// </summary>
    /// <param name="index">当前玩家索引</param>
    /// <returns>是否显示团队选择信息</returns>
    private bool ShowPlayerTeamInfo(int index)
    {
        Horizontal(true);
        playerShow[index] = EditorGUILayout.Foldout(playerShow[index], " ID : " + playerInfoList[index].id);

        EditorGUILayout.LabelField(" Name : " + playerInfoList[index].name);

        EditorGUILayout.LabelField(" AI : ", GUILayout.Width(30));
        playerInfoList[index].isAI = EditorGUILayout.Toggle(playerInfoList[index].isAI);

        EditorGUILayout.LabelField(" Color : ", GUILayout.Width(50));
        playerInfoList[index].representColor = EditorGUILayout.ColorField(playerInfoList[index].representColor, GUILayout.Width(100));

        Horizontal(false);
        return playerShow[index];
    }

    /// <summary>
    /// 显示团队下拉列表，第一项是玩家已经有队伍
    /// </summary>
    /// <param name="index">玩家索引</param>
    private void ShowDropdown(int index)
    {
        content = new GUIContent();
        //如果存在该队伍，直接显示在下拉菜单中
        if (playerInfoList[index].team != null)
            content.text = playerInfoList[index].team.TeamName;

        //下拉菜单，显示可选择队伍，修改后对应也会修改teamsManager
        if (EditorGUILayout.DropdownButton(content, FocusType.Passive))
        {
            GenericMenu menu = new GenericMenu();
            // 添加一条空的，就是可以不选择任何队伍
            menu.AddItem(new GUIContent("None (No Team)"), false,() => { playerInfoList[index].team = null; });
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i] == null || playerInfoList[index].team == teams[i])
                    continue;
                menu.AddItem(new GUIContent(teams[i].TeamName), false, (object teamIndex) => {playerInfoList[index].team = teams[(int)teamIndex]; },i);
            }
            menu.ShowAsContext();
        }
    }

    /// <summary>
    /// 显示团队ID、颜色
    /// </summary>
    /// <param name="team">团队</param>
    private void ShowTeamInfo(TeamManager team)
    {
        if (team == null)
        {
            EditorGUILayout.Space();
            return;
        }

        Horizontal(true);
        EditorGUILayout.LabelField("Team ID : " + team.TeamID);
        team.TeamColor = EditorGUILayout.ColorField(team.TeamColor);
        Horizontal(false);
    }

    /// <summary>
    /// 使用水平布局，参数是开始或结束
    /// </summary>
    /// <param name="trigger">是否开始（否则结束）</param>
    private void Horizontal(bool trigger)
    {
        if (trigger)
            EditorGUILayout.BeginHorizontal();
        else
            EditorGUILayout.EndHorizontal();
    }

}
