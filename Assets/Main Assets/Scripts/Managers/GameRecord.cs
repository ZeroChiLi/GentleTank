using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRecord
{
    private AllTanksManager allTanksManager;
    private AllTeamsManager allTeamsManager;
    private Dictionary<int, int> tanksIdTeamsIdDic;             // 坦克ID对应团队ID，没队的-1

    private int maxRound;                                       // 最大回合数
    private int currentRound = 0;                               // 当前回合数
    private int wonPlayerID;                                    // 当前（最终）获胜团队
    public Dictionary<int, int> playerWonTimes;                 // 玩家ID，获胜次数

    public int CurrentRound { get { return currentRound; } }

    /// <summary>
    /// 初始化构造游戏记录
    /// </summary>
    /// <param name="maxRound">最大回合数</param>
    /// <param name="tanksManager">坦克管理器</param>
    /// <param name="teamsManager">团队管理器</param>
    public GameRecord(int maxRound, AllTanksManager tanksManager, AllTeamsManager teamsManager)
    {
        this.maxRound = maxRound;
        allTanksManager = tanksManager;
        allTeamsManager = teamsManager;
        tanksIdTeamsIdDic = new Dictionary<int, int>();
        playerWonTimes = new Dictionary<int, int>();
        InitTanksTeamsDic();
    }

    // 初始化字典
    public void InitTanksTeamsDic()
    {
        //先从所有队伍里面填进去进去
        for (int i = 0; i < allTeamsManager.Length; i++)
            for (int j = 0; j < allTeamsManager[i].Count; j++)
                tanksIdTeamsIdDic[allTeamsManager[i][j]] = allTeamsManager[i].TeamID;
        //没有队伍的加进去赋值-1,顺便初始化playerWonTimes
        for (int i = 0; i < allTanksManager.Length; i++)
        {
            if (!tanksIdTeamsIdDic.ContainsKey(allTanksManager[i].playerID))
                tanksIdTeamsIdDic[allTanksManager[i].playerID] = -1;

            //这里是初始化playerWonTimes，为了省一圈循环
            playerWonTimes[allTanksManager[i].playerID] = 0;
        }

    }

    // 开始新游戏
    public void StartGame()
    {
        currentRound = 0;
    }

    // 开始新局，初始化,返回当前回合数
    public void StartRound()
    {
        ++currentRound;
        wonPlayerID = -1;
    }

    // 判断是否结束本局
    public bool IsEndOfTheRound()
    {
        bool haveWinner = false;
        int playerID = -1;
        for (int i = 0; i < allTanksManager.Length; i++)
            if (allTanksManager[i].Instance.activeSelf)
            {
                int playerTeamID = tanksIdTeamsIdDic[allTanksManager[i].playerID];
                // 遍历获取第一个赢的人
                if (!haveWinner)
                {
                    playerID = allTanksManager[i].playerID;
                    haveWinner = true;
                }
                // 第二个赢得的人没有队伍或与第一个人队伍不同，也说明没结束
                else if (playerTeamID == -1 || tanksIdTeamsIdDic[playerID] != playerTeamID)
                    return false;
            }
        //如果等于-1，说明平局
        wonPlayerID = playerID;
        return true;
    }

    // 判断是否结束游戏
    public bool IsEndOfTheGame()
    {
        foreach (var item in playerWonTimes)
            if (item.Value ==maxRound)
                return true;
        return false;
    }

    // 判断是否平局
    public bool IsDraw()
    {
        return wonPlayerID == -1;
    }

    // 是否团队获胜，否则个人获胜
    public bool IsTeamWon()
    {
        return tanksIdTeamsIdDic[wonPlayerID] != -1;
    }

    // 更新获胜数据
    public void UpdateWonData()
    {
        // 个人获胜，只加个人即可
        if (!IsTeamWon())
        {
            playerWonTimes[wonPlayerID]++;
            return;
        }

        // 团队获胜， 加团队所有人
        Team wonTeam = GetWonTeam();
        for (int i = 0; i < wonTeam.Count; i++)
            playerWonTimes[wonTeam[i]]++;

    }

    // 获取获胜名字，团队加‘TEAM’，个人加‘PLAYER’
    public string GetWinnerName()
    {
        string message;
        if (IsTeamWon())
            message = GetWonTeam().NameColored + " TEAM";
        else
            message = GetWonTank().ColoredPlayerName + " PLAYER";
        return message;
    }

    // 获取获胜团队
    public Team GetWonTeam()
    {
        return allTeamsManager.GetTeamByPlayerID(wonPlayerID);
    }

    // 获取个人获胜坦克ID
    public TankManager GetWonTank()
    {
        return allTanksManager.GetTankByID(wonPlayerID);
    }

}
