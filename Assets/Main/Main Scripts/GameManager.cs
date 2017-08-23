using Item.Tank;
using Widget.ArrowPopUp;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Widget.Minimap;
using System.Collections.Generic;
using CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    public GameManager Instance { get; private set; }

    public PointList spawnPointList;                // 坦克出生点
    public AllPlayerManager allPlayerManager;       // 所有玩家
    public int numRoundsToWin = 5;                  // 赢得游戏需要赢的回合数
    public float startDelay = 3f;                   // 开始延时时间
    public float endDelay = 3f;                     // 结束延时时间
    public float changeCamDelay = 2f;               // 转换镜头延时时间
    public Text messageText;                        // UI文本（玩家获胜等）
    public AllCameraRigManager allCameraRig;        // 所有镜头控制器
    public MinimapWitchCamera minimap;              // 小地图管理
    public AllArrowPopUpManager spawnAllPopUpArrow; // 用来显示玩家箭头

    public TankManager MyTank { get { return myTank; } }

    private List<TankManager> tankList;             // 所有玩家坦克
    private TankManager myTank;                     // 自己的坦克
    private WaitForSeconds startWait;               // 开始回合延时
    private WaitForSeconds changeCamWait;           // 转换镜头延时
    private WaitForSeconds endWait;                 // 结束回合延时

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

        new GameRound(numRoundsToWin);             // 创建一个游戏纪录实例
        GameRound.Instance.StartGame();            // 开始游戏循环（检测获胜者，重新回合，结束游戏等）
        StartCoroutine(GameLoop());
    }

    /// <summary>
    /// 产生所有坦克（包括玩家和AI）、设置镜头所有追踪目标、小地图初始化
    /// </summary>
    private void SetupGame()
    {
        allPlayerManager.SetupInstance();
        AllPlayerManager.Instance.CreatePlayerGameObjects(new GameObject("Tanks").transform);
        for (int i = 0; i < AllPlayerManager.Instance.Count; i++)
        {
            tankList.Add(AllPlayerManager.Instance[i].GetComponent<TankManager>());
            tankList[i].Init();
            if (AllPlayerManager.Instance[i] == AllPlayerManager.Instance.MyPlayer)
                myTank = tankList[i];
        }

        allCameraRig.Init(myTank.transform, AllPlayerManager.Instance.GetAllPlayerTransform());

        if (myTank != null)
        {
            minimap.SetTarget(myTank.transform);
            minimap.SetMinimapActive(true);
            ((ChargeButtonInput)VirtualInput.GetButton("TankShooting")).Setup(myTank.tankShooting.coolDownTime, myTank.tankShooting.minLaunchForce, myTank.tankShooting.maxLaunchForce, myTank.tankShooting.ChargeRate);
        }
    }

    /// <summary>
    /// 重置所有坦克出生点
    /// </summary>
    private void ResetAllTanksSpawnPoint()
    {
        spawnPointList.EnableAllPoints();                     // 初始化出生点
        for (int i = 0; i < tankList.Count; i++)
        {
            //获取有效随机出生点，且每个坦克位置不一样
            Point spawnPoint = spawnPointList.GetRandomPoint(false, true);
            if (spawnPoint == null)
                continue;
            tankList[i].ResetSpawnPoint(spawnPoint);
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
    /// 自己的坦克坏了，转换镜头
    /// </summary>
    private void OnMyTankBroken()
    {
        if (myTank == null || myTank.isActiveAndEnabled)
            return;
        minimap.SetMinimapActive(false);
        allCameraRig.TurnToMultiCam();
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
        spawnAllPopUpArrow.Spawn();                     // 显示玩家箭头
        minimap.SetMinimapActive(true);
        GameRound.Instance.StartRound();

        messageText.text = "ROUND " + GameRound.Instance.CurrentRound;

        yield return changeCamWait;                     // 延时一段时间转换成单独镜头
        allCameraRig.TurnToAutoCam();
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

        while (!GameRound.Instance.IsEndOfTheRound())   // 回合没结束就继续
        {
            OnMyTankBroken();
            yield return null;
        }
    }

    /// <summary>
    /// 回合结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoundEnding()
    {
        allCameraRig.TurnToMultiCam();

        SetTanksControlEnable(false);                   // 锁定玩家控制权

        GameRound.Instance.UpdateWonData();             // 更新获胜次数

        messageText.text = GameRound.Instance.GetEndingMessage();  // 获取结束信息并显示之

        yield return endWait;
    }

    /// <summary>
    /// 回到主菜单
    /// </summary>
    public void BackToMainScene()
    {
        AllSceneManager.LoadScene(GameScene.MainMenuScene);
    }
}