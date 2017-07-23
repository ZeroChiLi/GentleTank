using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

public class TanksToTeams : EditorWindow
{
    public AllTanksManager tanksManager;        // 坦克管理器
    public AllTeamsManager teamsManager;        // 团队管理器

    static private EditorWindow window;         // 编辑窗口
    private bool resetID;                       // 是否已经重载ID
    private bool isSetupTeams;                  // 是否已经配置了团队管理器
    private Vector2 scrollPos;                  // 滑动面板位置
    private bool[] tanksManagerShow;            // 对应坦克管理是否显示在面板

    // 坦克和选择的团队索引值，用来当回调函数参数用
    class TankIdAndTeamIndex { public int tankId; public TeamManager team; }

    [MenuItem("Window/Tanks To Teams")]
    static void Init()
    {
        window = GetWindow<TanksToTeams>();
        window.minSize = new Vector2(500f, 280f);
        window.Show();
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (!GetTanksTeamsManager())
            return;
        if (!resetID)
            ResetTanksAndTeamsID();
        TanksAndTeamsOperation();

        EditorGUILayout.EndScrollView();
    }

    // 强制重置坦克和团队们的ID
    private void ResetTanksAndTeamsID()
    {
        for (int i = 0; i < tanksManager.OriginalLength; i++)
            tanksManager.GetOriginalTank(i).SetPlayerID(i);
        for (int i = 0; i < teamsManager.Length; i++)
            teamsManager[i].SetTeamID(i);
        resetID = true;
    }

    // 同时获取到坦克、团队管理器
    private bool GetTanksTeamsManager()
    {
        EditorGUILayout.Space();

        Horizontal(true);
        EditorGUILayout.PrefixLabel("Tanks Manager");
        tanksManager = EditorGUILayout.ObjectField(tanksManager, typeof(AllTanksManager), false) as AllTanksManager;
        Horizontal(false);

        Horizontal(true);
        EditorGUILayout.PrefixLabel("Teams Manager");
        teamsManager = EditorGUILayout.ObjectField(teamsManager, typeof(AllTeamsManager), false) as AllTeamsManager;
        Horizontal(false);

        if (tanksManager == null || teamsManager == null)
            return false;

        // 如果没有初始化显示折叠坦克管理器，或者长度变化了，重新定义之
        if (tanksManagerShow == null || tanksManagerShow.Length != tanksManager.OriginalLength)
            tanksManagerShow = new bool[tanksManager.OriginalLength];
        return true;
    }

    // 坦克、团队操作
    private void TanksAndTeamsOperation()
    {
        EditorGUILayout.LabelField("Tanks Size ： " + tanksManager.OriginalLength);
        EditorGUILayout.LabelField("Teams Size ： " + teamsManager.Length);

        for (int i = 0; i < tanksManager.OriginalLength; i++)
        {
            EditorGUILayout.BeginVertical("Box");
            TankOperation(i);
            EditorGUILayout.EndVertical();
        }
    }

    // 单个坦克管理
    private void TankOperation(int index)
    {
        if (!ShowPlayerTeamInfo(index))
            return;
        GUILayout.Space(5);

        TeamManager playerTeam = new TeamManager();

        // 如果坦克已经有队伍了，获取信息到playerTeam和dropdownContent
        if (teamsManager.ContainsPlayer(tanksManager.GetOriginalTank(index).PlayerID))
            playerTeam = teamsManager.GetTeamByPlayerID(tanksManager.GetOriginalTank(index).PlayerID);

        //玩家无效且有队伍，踢出团队
        if (!tanksManager.GetOriginalTank(index).active)
        {
            if (playerTeam != null)
                QuitTeam(tanksManager.GetOriginalTank(index).PlayerID);
            return;
        }


        Horizontal(true);
        EditorGUILayout.PrefixLabel("Select Player's Team : ");
        Vertical(true);

        ShowDropdown(playerTeam, index);                        //显示下拉菜单
        ShowTeamInfo(playerTeam);                               //显示选中团队信息

        Vertical(false);
        Horizontal(false);
    }

    // 显示玩家信息，并返回是否显示团队选择信息
    private bool ShowPlayerTeamInfo(int index)
    {
        Horizontal(true);
        string showText = " ID : " + tanksManager.GetOriginalTank(index).PlayerID;
        tanksManagerShow[index] = EditorGUILayout.Foldout(tanksManagerShow[index], showText);

        Label(" Name : " + tanksManager.GetOriginalTank(index).playerName);

        Label(" Active : ", GUILayout.Width(50));
        tanksManager.GetOriginalTank(index).active = EditorGUILayout.Toggle(tanksManager.GetOriginalTank(index).active);

        Label(" AI : ", GUILayout.Width(30));
        tanksManager.GetOriginalTank(index).isAI = EditorGUILayout.Toggle(tanksManager.GetOriginalTank(index).isAI);

        Label(" Color : ", GUILayout.Width(50));
        tanksManager.GetOriginalTank(index).playerColor = EditorGUILayout.ColorField(tanksManager.GetOriginalTank(index).playerColor, GUILayout.Width(100));
        tanksManager.GetOriginalTank(index).playerColor.a = 1;

        Horizontal(false);
        return tanksManagerShow[index];
    }

    // 选择团队回调函数
    private void SelectedTeam(object tankToTeam)
    {
        TankIdAndTeamIndex tankIdTeam = tankToTeam as TankIdAndTeamIndex;
        if (tankIdTeam == null)
            return;

        teamsManager.AddToTeam(tankIdTeam.tankId, tankIdTeam.team);
    }

    // 退出团队回调函数
    private void QuitTeam(object tankID)
    {
        teamsManager.RemoveFromTeam((int)tankID);
    }

    // 显示团队下拉列表，第一参数是如果当前玩家已经有队伍
    private void ShowDropdown(TeamManager team, int currentIndex)
    {
        GUIContent content = new GUIContent();
        //如果存在该队伍，直接显示在下拉菜单中
        if (team != null)
            content.text = team.TeamName;

        //下拉菜单，显示可选择队伍，修改后对应也会修改teamsManager
        if (EditorGUILayout.DropdownButton(new GUIContent(team.TeamName), FocusType.Passive))
        {
            GenericMenu menu = new GenericMenu();
            // 添加一条空的，就是可以不选择任何队伍
            menu.AddItem(new GUIContent("None (No Team)"), false, QuitTeam, currentIndex);
            for (int i = 0; i < teamsManager.Length; i++)
            {
                if (team == teamsManager[i])        //跳过已经选中的队伍
                    continue;
                menu.AddItem(new GUIContent(teamsManager[i].TeamName), false, SelectedTeam, new TankIdAndTeamIndex { tankId = tanksManager.GetOriginalTank(currentIndex).PlayerID, team = teamsManager[i] });
            }
            menu.ShowAsContext();
        }
    }

    // 显示团队ID、人数以及颜色
    private void ShowTeamInfo(TeamManager team)
    {
        if (team == null || team.TeamID == -1)
        {
            EditorGUILayout.Space();
            return;
        }

        Horizontal(true);
        Label("Team ID : " + team.TeamID);
        Label("Team Count : " + team.Count);

        team.TeamColor = EditorGUILayout.ColorField(team.TeamColor);
        team.TeamColor.a = 1;

        Horizontal(false);
    }

    // 标签。名称，长度
    private void Label(string label, params GUILayoutOption[] options)
    {
        EditorGUILayout.LabelField(label, options);
    }

    // 使用水平布局，参数是开始或结束
    private void Horizontal(bool trigger)
    {
        if (trigger)
            EditorGUILayout.BeginHorizontal();
        else
            EditorGUILayout.EndHorizontal();
    }

    // 使用垂直布局，参数是开始或结束
    private void Vertical(bool trigger)
    {
        if (trigger)
            EditorGUILayout.BeginVertical();
        else
            EditorGUILayout.EndVertical();
    }

}