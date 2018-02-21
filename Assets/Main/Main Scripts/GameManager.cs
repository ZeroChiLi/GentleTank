using Item.Tank;
using Widget;
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

    public Points spawnPoints1;
    public Points spawnPoints2;
    public Points wayPoints;
    public AllPlayerManager allPlayerManager;       // 所有玩家
    public int numRoundsToWin = 5;                  // 赢得游戏需要赢的回合数
    public float startDelay = 3f;                   // 开始延时时间
    public float endDelay = 3f;                     // 结束延时时间
    public float changeCamDelay = 2f;               // 转换镜头延时时间
    public Text messageText;                        // UI文本（玩家获胜等）
    public GameEvent gameEvent;

    public TankManager MyTank { get { return myTank; } }

    private List<TankManager> tankList;             // 所有玩家坦克
    private TankManager myTank;                     // 自己的坦克
    private WaitForSeconds startWait;               // 开始回合延时
    private WaitForSeconds endWait;                 // 结束回合延时
    private WaitForSeconds changeCamWait;           // 转换镜头延时

    private void Awake()
    {
        Instance = this;
        tankList = new List<TankManager>();
        startWait = new WaitForSeconds(startDelay);         // 游戏回合开始延时
        endWait = new WaitForSeconds(endDelay);             // 游戏回合结束延时
        changeCamWait = new WaitForSeconds(changeCamDelay); // 镜头转换延时
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
        myTank = CreateMasterTank();
        allPlayerManager.SetupInstance();
        AllPlayerManager.Instance.CreatePlayerGameObjects(new GameObject("Tanks").transform, myTank);
        tankList.Add(myTank);
        myTank.Init(wayPoints);
        for (int i = 1; i < AllPlayerManager.Instance.Count; i++)
        {
            tankList.Add(AllPlayerManager.Instance[i].GetComponent<TankManager>());
            tankList[i].Init(wayPoints);
        }

        if (VirtualInput.GetButton("Attack") != null)
            ((ChargeButtonInput)VirtualInput.GetButton("Attack")).Setup(myTank.tankAttack, myTank.tankAttack.coolDownTime, myTank.tankAttack.minLaunchForce, myTank.tankAttack.maxLaunchForce, myTank.tankAttack.ChargeRate);

        MainCameraRig.Instance.Setup(myTank.transform, AllPlayerManager.Instance.GetAllPlayerTransform());
    }

    /// <summary>
    /// 创建玩家坦克
    /// </summary>
    private TankManager CreateMasterTank()
    {
        GameObject tank = Instantiate(MasterManager.Instance.StandardPrefab);
        MasterManager.Instance.SelectedTank.CreateTank(tank.transform);

        TankManager manager = tank.GetComponent<TankManager>();
        MasterManager.Instance.SelectedTank.InitTankComponents(manager);

        MasterData data = MasterManager.Instance.data;
        manager.Information = new PlayerInformation(0, data.masterName, data.isAI, data.representColor, data.team);
        manager.stateController.defaultStats = data.aiState;

        TankHealth health = tank.GetComponent<TankHealth>();
        health.OnDeathEvent += MyTankBorkenEvent;
        gameEvent.onMyPlayerCreatedEvent.Invoke();

        return manager;
    }

    /// <summary>
    /// 自己的坦克坏了，转换镜头
    /// </summary>
    private void MyTankBorkenEvent(HealthManager health, PlayerManager killer)
    {
        gameEvent.onMyPlayerDeadEvent.Invoke();
        if (killer == null)
            MainCameraRig.Instance.currentType = MainCameraRig.Type.MultiTargets;
        else
            StartCoroutine(MyTankDeathCameraBlend(health.transform, killer.transform));
    }

    /// <summary>
    /// 主角死后先把镜头给杀主角的玩家，再转到多目标镜头
    /// </summary>
    /// <param name="master"></param>
    /// <param name="killer"></param>
    /// <returns></returns>
    private IEnumerator MyTankDeathCameraBlend(Transform master, Transform killer)
    {
        MainCameraRig.Instance.oneTarget = killer;
        MainCameraRig.Instance.currentType = MainCameraRig.Type.OneTarget;
        yield return changeCamWait;
        MainCameraRig.Instance.currentType = MainCameraRig.Type.MultiTargets;
        MainCameraRig.Instance.oneTarget = master;
    }

    /// <summary>
    /// 重置所有坦克出生点
    /// </summary>
    private void ResetAllTanksToSpawnPoint()
    {
        int t1 = 0, t2 = 0;
        for (int i = 0; i < tankList.Count; i++)
        {
            if (tankList[i].Team.TeamID == (GameRound.Instance.CurrentRound % 2))
                tankList[i].ResetToSpawnPoint(spawnPoints1.GetWorldSpacePoint(spawnPoints1[t1++]));
            else
                tankList[i].ResetToSpawnPoint(spawnPoints2.GetWorldSpacePoint(spawnPoints2[t2++]));
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
            AllSceneManager.LoadScene(AllSceneManager.GameSceneType.SoloScene);
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

        yield return changeCamWait;                     // 延时一段时间转换成单独镜头
        if (myTank != null && !myTank.IsAI)
            MainCameraRig.Instance.currentType = MainCameraRig.Type.OneTarget;

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
        AllSceneManager.LoadScene(AllSceneManager.GameSceneType.MainMenuScene);
    }
}