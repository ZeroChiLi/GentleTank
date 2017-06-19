using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PointList spawnPointList;                // 坦克出生点
    public AllTanksManager allTanksManager;         // 所有坦克管理器
    public AllTeamsManager allTeamsManager;         // 所有团队管理器

    public int numRoundsToWin = 5;                  // 赢得游戏需要赢的回合数
    public float startDelay = 3f;                   // 开始延时时间
    public float endDelay = 3f;                     // 结束延时时间
    public CameraControl cameraControl;             // 相机控制脚本
    public MinimapManager minimapManager;           // 跟踪相机，用于小地图
    public Text messageText;                        // UI文本（玩家获胜等）

    private WaitForSeconds startWait;               // 开始回合延时
    private WaitForSeconds endWait;                 // 结束回合延时
    private GameRecord gameRecord;                  // 游戏记录，回合数，玩家获胜数

    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);
        gameRecord = new GameRecord(numRoundsToWin, allTanksManager, allTeamsManager);
        spawnPointList.EnableAllPoints();

        SpawnAllTanks();
        SetupCameraAndMinimap();

        // 开始游戏循环（检测获胜者，重新回合，结束游戏等）
        gameRecord.StartGame();
        StartCoroutine(GameLoop());
    }

    // 产生所有坦克，包括玩家和AI
    private void SpawnAllTanks()
    {
        GameObject tanks = new GameObject("Tanks");
        for (int i = 0; i < allTanksManager.Length; i++)
        {
            //获取有效随机出生点，且每个坦克位置不一样
            Point spawnPoint = spawnPointList.GetRandomPoint(false);
            if (spawnPoint == null)
                continue;

            allTanksManager[i].InitTank(Instantiate(allTanksManager[i].tankPerfab, spawnPoint.position, Quaternion.Euler(spawnPoint.rotation),tanks.transform), allTeamsManager);
            allTanksManager[i].SetupTank();
        }
    }

    // 给主相机添加所有坦克，小地图相机添加追踪目标
    private void SetupCameraAndMinimap()
    {
        cameraControl.targets = allTanksManager.GetTanksTransform();
        minimapManager.SetupPlayerIconDic(allTanksManager,allTeamsManager);
        minimapManager.SetTarget(allTanksManager[0].Instance.transform);
    }

    // 游戏的循环协程
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());           //回合开始，有一段延时

        yield return StartCoroutine(RoundPlaying());            //回合中，多于一个坦克存活时一直在这里死循环

        yield return StartCoroutine(RoundEnding());             //回合结束

        // 如果结束了游戏，重新加载场景，否则进行下一回合
        if (gameRecord.IsEndOfTheGame())
            SceneManager.LoadScene(0);
        else
            StartCoroutine(GameLoop());
    }

    // 回合开始
    private IEnumerator RoundStarting()
    {
        ResetAllTanks();                                // 重置所有坦克
        SetTanksControlEnable(false);                   // 并且锁定他们的控制权

        cameraControl.SetStartPositionAndSize();        // 重置相机

        gameRecord.StartRound();
        messageText.text = "ROUND " + gameRecord.CurrentRound;

        yield return startWait;                         // 延时一段时间再开始
    }

    // 回合中
    private IEnumerator RoundPlaying()
    {
        SetTanksControlEnable(true);                    // 解锁玩家控制权

        messageText.text = string.Empty;                // 清空显示信息

        while (!gameRecord.IsEndOfTheRound())           // 回合没结束就继续
            yield return null;
    }

    // 回合结束
    private IEnumerator RoundEnding()
    {
        SetTanksControlEnable(false);                   // 锁定玩家控制权

        gameRecord.UpdateWonData();                     // 更新获胜次数

        messageText.text = EndMessage();                // 获取结束信息并显示之

        yield return endWait;
    }

    // 获取回合或总的游戏结束信息
    private string EndMessage()
    {
        string message = "DRAW!";                       // 默认平局

        if (!gameRecord.IsDraw())                       // 不是平局，获取胜利者
            message = gameRecord.GetWinnerName() + " WINS THE ROUND!";

        message += "\n\n";

        foreach (var item in gameRecord.playerWonTimes) // 获取所有玩家胜利信息
            message += allTanksManager.GetTankByID(item.Key).ColoredPlayerName + " : " + item.Value + "WINS\n";

        if (gameRecord.IsEndOfTheGame())                // 如果是最后结束，输出最后赢最多的玩家
            message = gameRecord.GetWinnerName() + " WINS THE GAME!";
        return message;
    }

    // 重置所有坦克
    private void ResetAllTanks()
    {
        spawnPointList.EnableAllPoints();                     // 初始化出生点
        for (int i = 0; i < allTanksManager.Length; i++)
        {
            //获取有效随机出生点，且每个坦克位置不一样
            Point spawnPoint = spawnPointList.GetRandomPoint(false, true);
            if (spawnPoint == null)
                continue;
            allTanksManager[i].Reset(spawnPoint);
        }
    }

    // 设置所有玩家控制权
    private void SetTanksControlEnable(bool enable)
    {
        for (int i = 0; i < allTanksManager.Length; i++)
            allTanksManager[i].SetControlEnable(enable);
    }
}
