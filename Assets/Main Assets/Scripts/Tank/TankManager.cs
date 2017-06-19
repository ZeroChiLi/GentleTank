using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class TankManager
{
    public string playerName;                               // 玩家名称
    public bool isAI;                                       // 是否是AI
    public GameObject tankPerfab;                           // 坦克预设
    [ColorUsage(false)]
    public Color playerColor = Color.white;                 // 渲染颜色

    public int PlayerID { get { return playerID; } }                        // 获取玩家ID
    public GameObject Instance { get { return instance; } }                 // 获取坦克的实例
    public string ColoredPlayerName { get { return coloredPlayerName; } }   // 获取带颜色的玩家名

    private int playerID;                                   // 玩家ID
    private GameObject instance;                            // 玩家实例
    private string coloredPlayerName;                       // 带颜色的玩家名
    private Point spawnPoint;                               // 出生点
    private AllTeamsManager allTeamsManager;                // 所有团队管理
    private Team playerTeam;                                // 玩家所在团队

    private TankMovement movement;                          // 移动脚本
    private TankShooting shooting;                          // 攻击脚本
    private GameObject hpCanvas;                            // 显示玩家信息UI（血量）

    private StateController stateController;                // AI状态控制器
    private NavMeshAgent navMeshAgent;                      // AI导航

    /// <summary>
    /// 设置玩家ID
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    public void SetPlayerID(int playerID)
    {
        this.playerID = playerID;
    }

    /// <summary>
    /// 初始化坦克
    /// </summary>
    /// <param name="instance">坦克Perfab</param>
    /// <param name="spawnPoint">出生点</param>
    /// <param name="allTeamsManager">所有团队管理器</param>
    public void InitTank(GameObject instance,Point spawnPoint, AllTeamsManager allTeamsManager)
    {
        this.instance = instance;
        this.spawnPoint = spawnPoint;
        this.allTeamsManager = allTeamsManager;
        playerTeam = allTeamsManager.GetTeamByPlayerID(PlayerID);
    }

    /// <summary>
    /// 配置坦克,包括获取实例的必要组件、激活控制权、渲染坦克颜色。
    /// </summary>
    public void SetupTank()
    {
        SetupComponent();                               // 获取私有组件
        SetControlEnable(true);                         // 激活相应的控制权
        RenderTankColor();                              // 渲染颜色
    }

    /// <summary>
    /// 获取所有要用到的私有组件
    /// </summary>
    private void SetupComponent()
    {
        movement = instance.GetComponent<TankMovement>();
        shooting = instance.GetComponent<TankShooting>();
        hpCanvas = instance.GetComponentInChildren<Canvas>().gameObject;
        stateController = instance.GetComponent<StateController>();
        navMeshAgent = instance.GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// 为所有带'NeedRenderByPlayerColor'的子组件染色，包括自己的名字UI，包括团队灯光
    /// </summary>
    private void RenderTankColor()
    {
        TankColor tankColor = instance.gameObject.GetComponent<TankColor>();
        tankColor.RenderColorByComponent<NeedRenderByPlayerColor>(playerColor);
        coloredPlayerName = playerName;
        if (playerTeam != null)
        {
            coloredPlayerName = "<color=#" + ColorUtility.ToHtmlStringRGB(playerTeam.TeamColor) + ">" + playerName + "</color>";
            //团队灯光
            tankColor.GetComponentInChildren<Light>().color = playerTeam.TeamColor;
        }
        //玩家UI彩色字
        tankColor.RenderPlayerInfo(ColoredPlayerName);
    }

    /// <summary>
    /// 重置出生点以及激活状态
    /// </summary>
    /// <param name="spawnPoint">出生点</param>
    public void Reset(Point spawnPoint)
    {
        this.spawnPoint = spawnPoint;
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = Quaternion.Euler(spawnPoint.rotation);

        //先设置False，因为如果获胜了的玩家本身就是true，重置就会调用OnEnable函数。
        instance.SetActive(false);
        instance.SetActive(true);
    }

    /// <summary>
    /// 设置控制权激活状态
    /// </summary>
    /// <param name="enable">是否激活</param>
    public void SetControlEnable(bool enable)
    {
        if (isAI)
            SetAIControlEnable(enable);
        else
            SetPlayerControlEnable(enable);

        shooting.enabled = enable;
        shooting.SetPlayerNumber(PlayerID);
        hpCanvas.SetActive(enable);
    }

    /// <summary>
    /// AI操控，设置状态控制器,激活 StateController ，关闭 TankMovement
    /// </summary>
    /// <param name="enable">是否激活</param>
    private void SetAIControlEnable(bool enable)
    {
        if (movement != null)
            movement.enabled = false;
        if (stateController != null)
        {
            stateController.enabled = enable;
            stateController.SetupAI(PlayerID, allTeamsManager);
        }
        else
            Debug.LogError("If This Tank Is AI,You Need 'StateController' Script Compontent");
    }

    /// <summary>
    /// 人为操控，设置控制输入，激活TankMovement，关闭 StateController、导航
    /// </summary>
    /// <param name="enable">是否激活</param>
    private void SetPlayerControlEnable(bool enable)
    {
        if (stateController != null)
            stateController.enabled = false;
        if (navMeshAgent != null)
            navMeshAgent.enabled = false;
        if (movement != null)
        {
            movement.enabled = enable;
            movement.SetPlayerNumber(PlayerID);
        }
        else
            Debug.LogError("If You Want To Control Tank,Need 'TankMovement' Script Component.");
    }

}
