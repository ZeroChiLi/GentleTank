using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRecord
{
    private AllTanksManager allTanksManager;
    private AllTeamsManager allTeamsManager;
    private Dictionary<int, int> tanksIdTeamsIdDic;             //坦克ID对应团队ID，没队的-1

    private int maxRound;                                       //最大回合数
    private int currentRound;                                   //当前回合数
    private Team wonTeam;                                       //当前（最终）获胜团队
    private TankManager[] wonTanks;                             //获胜的玩家们

    GameRecord(int maxRound, AllTanksManager tanksManager, AllTeamsManager teamsManager)
    {
        this.maxRound = maxRound;
        allTanksManager = tanksManager;
        allTeamsManager = teamsManager;
        InitTanksTeamsDic();
    }

    //初始化字典
    public void InitTanksTeamsDic()
    {
        //先从所有队伍里面填进去进去
        for (int i = 0; i < allTeamsManager.Length; i++)
            for (int j = 0; j < allTeamsManager[i].Count; j++)
                tanksIdTeamsIdDic[allTeamsManager[i][j]] = allTeamsManager[i].TeamID;
        //没有队伍的加进去赋值-1
        for (int i = 0; i < allTanksManager.Length; i++)
            if (!tanksIdTeamsIdDic.ContainsKey(allTanksManager[i].playerID))
                tanksIdTeamsIdDic[allTanksManager[i].playerID] = -1;
    }

    public bool EndOfTheRound()
    {
        int teamLeftCount = 0;
        for (int i = 0; i < allTanksManager.Length; i++)
        {

        }
        return false;
    }

}
