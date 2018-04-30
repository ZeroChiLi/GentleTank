using Item.Tank;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CrossPlatformInput;
using CameraRig;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct GameEvent
    {
        public UnityEvent onBeforeRoundStartEvent;
        public UnityEvent onAfterRoundStartEvent;
        public UnityEvent onBeforeRoundEndEvent;
        public UnityEvent onAfterRoundEndEvent;
        public UnityEvent onMyPlayerCreatedEvent;
        public UnityEvent onMyPlayerDeadEvent;
    }
    static public GameManager Instance { get; private set; }

    public Points spawnPoints;
    public Points wayPoints;
    //public AllPlayerManager allPlayerManager;       // 所有玩家
    public int numRoundsToWin = 5;                  // 赢得游戏需要赢的回合数
    public float startDelay = 3f;                   // 开始延时时间
    public float endDelay = 3f;                     // 结束延时时间
    public Text messageText;                        // UI文本（玩家获胜等）
    public GameEvent gameEvent;

    private List<TankManager> tankList;             // 所有玩家坦克
    private WaitForSeconds startWait;               // 开始回合延时
    private WaitForSeconds endWait;                 // 结束回合延时

    private void Awake()
    {
        Instance = this;
        tankList = new List<TankManager>();
        startWait = new WaitForSeconds(startDelay);         // 游戏回合开始延时
        endWait = new WaitForSeconds(endDelay);             // 游戏回合结束延时
    }

    /// <summary>
    /// 初始化游戏记录实例、产生所有坦克、设置相机目标、小地图初始化、开始游戏循环
    /// </summary>
    private void Start()
    {
        SetupGame();                                // 配置游戏

        GameRound.Instance.maxRound = numRoundsToWin;             // 设置局数
        GameRound.Instance.StartGame();            // 开始游戏循环（检测获胜者，重新回合，结束游戏等）
        StartCoroutine(GameLoop());
    }

    /// <summary>
    /// 产生所有坦克（包括玩家和AI）、设置镜头所有追踪目标、小地图初始化
    /// </summary>
    private void SetupGame()
    {
        //allPlayerManager.SetupInstance();
        AllPlayerManager.Instance.CreatePlayerGameObjects(new GameObject("Tanks").transform);
        for (int i = 0; i < AllPlayerManager.Instance.Count; i++)
            if (AllPlayerManager.Instance[i].IsJoin)
            {
                tankList.Add(AllPlayerManager.Instance[i].GetComponent<TankManager>());
                tankList[i].Init(wayPoints);
            }

        MainCameraRig.Instance.Setup(AllPlayerManager.Instance.GetAllPlayerTransform());
    }

    /// <summary>
    /// 重置所有坦克出生点
    /// </summary>
    private void ResetAllTanksToSpawnPoint()
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < spawnPoints.Count; i++)
            indexList.Add(i);
        for (int i = 0; i < tankList.Count; i++)
        {
            int index = Random.Range(0, indexList.Count);
            tankList[i].ResetToSpawnPoint(spawnPoints.GetWorldSpacePoint(spawnPoints[indexList[index]]));
            indexList.RemoveAt(index);
        }
    }

    /// <summary>
    /// 设置所有玩家控制权
    /// </summary>
    /// <param name="enable">激活状态</param>
    private void SetTanksControlEnable(bool enable)
    {
        for (int i = 0; i < tankList.Count; i++)
            tankList[i].SetControlEnable(enable);
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
        if (GameRound.Instance.IsEndOfTheGame())
            BackToMainScene();
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
        ResetAllTanksToSpawnPoint();                    // 重置所有坦克位置
        gameEvent.onBeforeRoundStartEvent.Invoke();
        GameRound.Instance.StartRound();

        messageText.text = "ROUND " + GameRound.Instance.CurrentRound;

        yield return startWait;                         // 延时一段时间再开始
        gameEvent.onAfterRoundStartEvent.Invoke();
    }

    /// <summary>
    /// 回合中
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoundPlaying()
    {
        SetTanksControlEnable(true);                    // 解锁玩家控制权

        messageText.text = string.Empty;                // 清空显示信息

        while (!GameRound.Instance.IsEndOfTheRound())   // 回合没结束就继续
            yield return null;
    }

    /// <summary>
    /// 回合结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoundEnding()
    {
        gameEvent.onBeforeRoundEndEvent.Invoke();
        MainCameraRig.Instance.currentType = MainCameraRig.Type.MultiTargets;

        SetTanksControlEnable(false);                   // 锁定玩家控制权

        GameRound.Instance.UpdateWonData();             // 更新获胜次数

        messageText.text = GameRound.Instance.GetEndingMessage();  // 获取结束信息并显示之

        yield return endWait;
        gameEvent.onAfterRoundEndEvent.Invoke();
    }

    /// <summary>
    /// 回到主菜单
    /// </summary>
    public void BackToMainScene()
    {
        StopAllCoroutines();
        AllSceneManager.LoadScene(AllSceneManager.GameSceneType.MainMenuScene);
    }
}