using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PointList spawnPointList;                // 坦克出生点
    public AllTanksManager allTanks;                // 所有坦克管理器
    public AllTeamsManager allTeams;                // 所有团队管理器

    public int numRoundsToWin = 5;                  // 赢得游戏需要赢的回合数
    public float startDelay = 3f;                   // 开始延时时间
    public float endDelay = 3f;                     // 结束延时时间
    public Text messageText;                        // UI文本（玩家获胜等）
    public CameraControl cameraControl;             // 相机控制组件
    public MinimapManager minimapManager;           // 小地图管理器
    public SpawnAllPopUpArrow spawnAllPopUpArrow;   // 用来显示玩家箭头

    private WaitForSeconds startWait;               // 开始回合延时
    private WaitForSeconds endWait;                 // 结束回合延时

    private void Awake()
    {
        allTanks.SetupInstance();
        allTeams.SetupInstance();
        GameRecord.Instance = new GameRecord(numRoundsToWin); // 创建一个游戏纪录实例
        startWait = new WaitForSeconds(startDelay); // 游戏回合开始延时
        endWait = new WaitForSeconds(endDelay);     // 游戏回合结束延时
    }

    /// <summary>
    /// 初始化游戏记录实例、产生所有坦克、设置相机目标、小地图初始化、开始游戏循环
    /// </summary>
    private void Start()
    {
        SetupGame();                                // 配置游戏

        GameRecord.Instance.StartGame();            // 开始游戏循环（检测获胜者，重新回合，结束游戏等）
        StartCoroutine(GameLoop());
    }

    /// <summary>
    /// 产生所有坦克（包括玩家和AI）、设置镜头所有追踪目标、小地图初始化
    /// </summary>
    private void SetupGame()
    {
        GameObject tanks = new GameObject("Tanks");
        for (int i = 0; i < AllTanksManager.Instance.Count; i++)
            AllTanksManager.Instance[i].InitTank(Instantiate(AllTanksManager.Instance[i].tankPerfab, tanks.transform));

        cameraControl.targets = AllTanksManager.Instance.GetTanksTransform();
        cameraControl.SetStartPositionAndSize();

        minimapManager.SetupPlayerIconDic();
        minimapManager.SetTarget(AllTanksManager.Instance[0].Instance.transform);
    }

    /// <summary>
    /// 重置所有坦克出生点
    /// </summary>
    private void ResetAllTanksSpawnPoint()
    {
        spawnPointList.EnableAllPoints();                     // 初始化出生点
        for (int i = 0; i < AllTanksManager.Instance.Count; i++)
        {
            //获取有效随机出生点，且每个坦克位置不一样
            Point spawnPoint = spawnPointList.GetRandomPoint(false, true);
            if (spawnPoint == null)
                continue;
            AllTanksManager.Instance[i].Reset(spawnPoint);
        }
    }

    /// <summary>
    /// 设置所有玩家控制权
    /// </summary>
    /// <param name="enable">激活状态</param>
    private void SetTanksControlEnable(bool enable)
    {
        for (int i = 0; i < AllTanksManager.Instance.Count; i++)
            AllTanksManager.Instance[i].SetControlEnable(enable);
    }

    /// <summary>
    /// 游戏的循环协程
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());           //回合开始，有一段延时

        yield return StartCoroutine(RoundPlaying());            //回合中

        yield return StartCoroutine(RoundEnding());             //回合结束

        // 如果结束了游戏，重新加载场景，否则进行下一回合
        if (GameRecord.Instance.IsEndOfTheGame())
            SceneManager.LoadScene(0);
        else
            StartCoroutine(GameLoop());
    }

    /// <summary>
    /// 回合开始
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoundStarting()
    {
        SetTanksControlEnable(false);                   // 锁定坦克们的控制权
        ResetAllTanksSpawnPoint();                      // 重置所有坦克位置
        spawnAllPopUpArrow.Spawn();      // 显示玩家箭头
        GameRecord.Instance.StartRound();

        messageText.text = "ROUND " + GameRecord.Instance.CurrentRound;

        yield return startWait;                         // 延时一段时间再开始
    }

    /// <summary>
    /// 回合中
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoundPlaying()
    {
        SetTanksControlEnable(true);                    // 解锁玩家控制权

        messageText.text = string.Empty;                // 清空显示信息

        while (!GameRecord.Instance.IsEndOfTheRound())           // 回合没结束就继续
            yield return null;
    }

    /// <summary>
    /// 回合结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoundEnding()
    {
        SetTanksControlEnable(false);                               // 锁定玩家控制权

        GameRecord.Instance.UpdateWonData();                        // 更新获胜次数

        messageText.text = GameRecord.Instance.GetEndingMessage();  // 获取结束信息并显示之

        yield return endWait;
    }


}