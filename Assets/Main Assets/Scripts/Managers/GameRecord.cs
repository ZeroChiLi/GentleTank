using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRecord
{
    static public GameRecord instance;

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
        instance = this;
        this.maxRound = maxRound;
        allTanksManager = tanksManager;
        allTeamsManager = teamsManager;
        tanksIdTeamsIdDic = new Dictionary<int, int>();
        playerWonTimes = new Dictionary<int, int>();
        InitTanksTeamsDic();
    }

    /// <summary>
    /// 初始化坦克团队字典
    /// </summary>
    private void InitTanksTeamsDic()
    {
        //先从所有队伍里面填进去进去
        for (int i = 0; i < allTeamsManager.Length; i++)
            for (int j = 0; j < allTeamsManager[i].Count; j++)
                tanksIdTeamsIdDic[allTeamsManager[i][j]] = allTeamsManager[i].TeamID;
        //没有队伍的加进去赋值-1,顺便初始化playerWonTimes
        for (int i = 0; i < allTanksManager.Count; i++)
        {
            if (!tanksIdTeamsIdDic.ContainsKey(allTanksManager[i].PlayerID))
                tanksIdTeamsIdDic[allTanksManager[i].PlayerID] = -1;

            //这里是初始化playerWonTimes，为了省一圈循环
            playerWonTimes[allTanksManager[i].PlayerID] = 0;
        }

    }

    /// <summary>
    /// 开始新游戏，设置当前回合数为0
    /// </summary>
    public void StartGame()
    {
        currentRound = 0;
    }

    /// <summary>
    /// 开始新的回合，增加当前回合数，设置获胜玩家ID为-1
    /// </summary>
    public void StartRound()
    {
        ++currentRound;
        wonPlayerID = -1;
    }

    /// <summary>
    /// 判断是否结束回合，并设置获胜者ID，平局设置ID为-1
    /// </summary>
    /// <returns>是否结束回合</returns>
    public bool IsEndOfTheRound()
    {
        bool haveWinner = false;
        int playerID = -1;
        for (int i = 0; i < allTanksManager.Count; i++)
            if (allTanksManager[i].Instance.activeSelf)
            {
                int playerTeamID = tanksIdTeamsIdDic[allTanksManager[i].PlayerID];
                // 遍历获取第一个赢的人
                if (!haveWinner)
                {
                    playerID = allTanksManager[i].PlayerID;
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

    /// <summary>
    /// 判断是否结束游戏,遍历一遍所有坦克的获胜次数，有等于最大回合数就是结束游戏了
    /// </summary>
    /// <returns>是否结束游戏</returns>
    public bool IsEndOfTheGame()
    {
        foreach (var item in playerWonTimes)
            if (item.Value == maxRound)
                return true;
        return false;
    }

    /// <summary>
    /// 判断是否平局，就是判断获胜玩家ID是否为-1
    /// </summary>
    /// <returns>是否平局</returns>
    public bool IsDraw()
    {
        return wonPlayerID == -1;
    }

    /// <summary>
    /// 判断是否是团队获胜，否则是个人获胜，就是判断玩家ID对应的团队ID
    /// </summary>
    /// <returns>是否团队获胜</returns>
    public bool IsTeamWon()
    {
        return tanksIdTeamsIdDic[wonPlayerID] != -1;
    }

    /// <summary>
    /// 更新获胜玩家们数据
    /// </summary>
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

    /// <summary>
    /// 获取获胜名字，团队加‘TEAM’，个人加‘PLAYER’
    /// </summary>
    /// <returns>返回获胜玩家（团队）字符串</returns>
    private string GetWinnerName()
    {
        string message;
        if (IsTeamWon())
            message = GetWonTeam().ColoredTeamName + " TEAM";
        else
            message = GetWonTank().ColoredPlayerName + " PLAYER";
        return message;
    }

    /// <summary>
    /// 获取获胜团队
    /// </summary>
    /// <returns>获胜团队</returns>
    private Team GetWonTeam()
    {
        return allTeamsManager.GetTeamByPlayerID(wonPlayerID);
    }

    /// <summary>
    /// 获取个人获胜坦克ID
    /// </summary>
    /// <returns>获胜玩家</returns>
    private TankManager GetWonTank()
    {
        return allTanksManager.GetTankByID(wonPlayerID);
    }

    /// <summary>
    /// 获取回合或总的游戏结束信息
    /// </summary>
    /// <returns></returns>
    public string GetEndingMessage()
    {
        string message = "DRAW!";                       // 默认平局

        if (!IsDraw())                                  // 不是平局，获取胜利者
            message = GetWinnerName() + " WINS THE ROUND!";

        message += "\n\n";

        foreach (var item in playerWonTimes)            // 获取所有玩家胜利信息
            message += allTanksManager.GetTankByID(item.Key).ColoredPlayerName + " : " + item.Value + "WINS\n";

        if (IsEndOfTheGame())                           // 如果是最后结束，输出最后赢最多的玩家
            message = GetWinnerName() + " WINS THE GAME!";
        return message;
    }

}
