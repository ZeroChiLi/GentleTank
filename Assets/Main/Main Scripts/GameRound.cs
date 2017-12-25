using System.Collections.Generic;
using System.Text;
using UnityEngine.Events;

/// <summary>
/// 游戏状态
/// </summary>
public enum GameState
{
    None, Start, Playing, End
}

public class GameRound
{
    private static GameRound instance;
    public static GameRound Instance { get { return instance = instance ?? new GameRound(); } }

    public int maxRound;                                        // 最大回合数
    public UnityEvent OnGameRoundStartEvent = new UnityEvent(); // 游戏回合开始时响应
    public UnityEvent OnGameRoundEndEvent = new UnityEvent();   // 游戏回合结束时响应

    private GameState currentGameState;                         // 当前游戏状态
    private int currentRound = 0;                               // 当前回合数
    private PlayerManager winner;                               // 当前（最终）获胜团队
    private Dictionary<PlayerManager, int> playerWonTimes = new Dictionary<PlayerManager, int>();      // 玩家及其对应获胜次数
    private bool haveWinner = false;                            // 临时判断获胜玩家变量
    private PlayerManager temPlayer;                            // 临时玩家变量

    public PlayerManager Winner { get { return winner; } }
    public int CurrentRound { get { return currentRound; } }
    public GameState CurrentGameState { get { return currentGameState; } }
    public bool IsGamePlaying { get { return currentGameState == GameState.Playing; } }

    /// <summary>
    /// 初始化坦克团队字典
    /// </summary>
    public void InitTanksTeamsDic()
    {
        playerWonTimes.Clear();
        for (int i = 0; i < AllPlayerManager.Instance.Count; i++)
            playerWonTimes.Add(AllPlayerManager.Instance[i],0);
    }

    /// <summary>
    /// 开始新游戏，设置当前回合数为0
    /// </summary>
    public void StartGame()
    {
        InitTanksTeamsDic();
        currentRound = 0;
    }

    /// <summary>
    /// 开始新的回合，增加当前回合数，设置清除获胜玩家
    /// </summary>
    public void StartRound()
    {
        ++currentRound;
        winner = null;
        currentGameState = GameState.Start;
        //OnGameRoundStartEvent.Invoke();
    }

    /// <summary>
    /// 判断是否结束回合，并设置获胜者
    /// </summary>
    /// <returns>是否结束回合</returns>
    public bool IsEndOfTheRound()
    {
        currentGameState = GameState.Playing;
        haveWinner = false;
        temPlayer = null;
        for (int i = 0; i < AllPlayerManager.Instance.Count; i++)
            if (AllPlayerManager.Instance[i].gameObject.activeSelf)
            {
                // 遍历获取第一个赢的人
                if (!haveWinner)
                {
                    temPlayer = AllPlayerManager.Instance[i];
                    haveWinner = true;
                }
                // 第二个赢得的人没有队伍或与第一个人队伍不同，也说明没结束
                else if (!AllPlayerManager.Instance[i].IsTeammate(temPlayer))
                    return false;
            }
        //如果为空，说明平局（全死了，没有获胜玩家）
        winner = temPlayer;
        currentGameState = GameState.End;
        //OnGameRoundEndEvent.Invoke();
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
        return winner == null;
    }

    /// <summary>
    /// 判断是否是团队获胜，否则是个人获胜，就是判断玩家ID对应的团队ID
    /// </summary>
    /// <returns>是否团队获胜</returns>
    public bool IsTeamWon()
    {
        return winner.Team != null;
    }

    /// <summary>
    /// 更新获胜玩家们数据
    /// </summary>
    public void UpdateWonData()
    {
        //平局不更新
        if (IsDraw())
            return;

        // 个人获胜，只加个人即可
        if (!IsTeamWon())
        {
            playerWonTimes[winner]++;
            return;
        }

        // 团队获胜， 加团队所有人
        for (int i = 0; i < AllPlayerManager.Instance.Count; i++)
            if (winner.Team == AllPlayerManager.Instance[i].Team)
                playerWonTimes[AllPlayerManager.Instance[i]]++;

    }

    /// <summary>
    /// 获取获胜名字，团队加‘TEAM’，个人加‘PLAYER’
    /// </summary>
    /// <returns>返回获胜玩家（团队）字符串</returns>
    private string GetWinnerName()
    {
        string message;
        if (IsTeamWon())
            message = winner.Team.ColoredTeamName + " TEAM";
        else
            message = winner.ColoredPlayerName + " PLAYER";
        return message;
    }

    /// <summary>
    /// 获取回合或总的游戏结束信息
    /// </summary>
    /// <returns></returns>
    public string GetEndingMessage()
    {
        StringBuilder message;

        if (IsDraw())                                       // 平局，获取胜利者
        {
            message = new StringBuilder("DRAW!\n\n");
            return message.ToString();
        }

        if (IsEndOfTheGame())                               // 如果是最后结束，输出最后赢最多的玩家
            message = new StringBuilder(GetWinnerName() + " WINS THE GAME!");
        else
        {
            message = new StringBuilder(GetWinnerName() + " WINS THE ROUND!\n\n");
            foreach (var item in playerWonTimes)            // 获取所有玩家胜利信息
                message.AppendFormat("{0} : {1} WINS\n", item.Key.ColoredPlayerNameByTeam, item.Value);
        }

        return message.ToString();
    }

}