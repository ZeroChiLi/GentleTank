using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class TankManager
{
    public int playerID;                                    // 玩家编号
    public bool isAI;                                       // 是否是AI
    public GameObject tankPerfab;                           // 坦克预设
    [ColorUsage(false)]
    public Color playerColor = Color.white;                 // 渲染颜色

    public GameObject Instance { get { return instance; } }         // 获取坦克的实例
    public string PlayerName { get { return coloredPlayerName; } }  // 获取带颜色的玩家名
    public int WinTimes { get { return winTimes; } }                // 获取玩家获胜次数

    private GameObject instance;                            // 玩家实例
    private string coloredPlayerName;                       // 带颜色的玩家名
    private int winTimes;                                   // 玩家回合获胜次数
    private Point spawnPoint;                               // 出生点
    private Color teamColor;

    private TankMovement movement;                          // 移动脚本
    private TankShooting shooting;                          // 攻击脚本
    private GameObject hpCanvas;                            // 显示玩家信息UI（血量）

    private StateController stateController;                // AI状态控制器
    private NavMeshAgent navMeshAgent;                      // AI导航


    // 初始化坦克
    public void InitTank(GameObject instance, int winTimes, Point spawnPoint,Color teamColor)
    {
        this.instance = instance;
        this.winTimes = winTimes;
        this.spawnPoint = spawnPoint;
        this.teamColor = teamColor;
    }

    // 配置坦克
    public void SetupTank()
    {
        SetupComponent();                                   // 获取私有组件
        SetControlEnable(true);                             // 激活相应的控制权
        RenderPlayerColor();                                // 渲染颜色
    }

    // 配置所有要用到的私有组件
    private void SetupComponent()
    {
        movement = instance.GetComponent<TankMovement>();
        shooting = instance.GetComponent<TankShooting>();
        hpCanvas = instance.GetComponentInChildren<Canvas>().gameObject;
        stateController = instance.GetComponent<StateController>();
        navMeshAgent = instance.GetComponent<NavMeshAgent>();
    }

    // 设置控制权激活状态
    public void SetControlEnable(bool enable)
    {
        // AI操控，设置状态控制器,激活 StateController ，关闭 TankMovement
        if (isAI)
        {
            if (movement != null)
                movement.enabled = false;
            if (stateController != null)
            {
                stateController.enabled = enable;
                stateController.SetupAI(playerID);
            }
            else
                Debug.LogError("If This Tank Is AI,You Need 'StateController' Script Compontent");
        }
        // 人为操控，设置控制输入，激活TankMovement，关闭 StateController、导航
        else
        {
            if (stateController != null)
                stateController.enabled = false;
            if (navMeshAgent != null)
                navMeshAgent.enabled = false;
            if (movement != null)
            {
                movement.enabled = enable;
                movement.SetPlayerNumber(playerID);
            }
            else
                Debug.LogError("If You Want To Control Tank,Need 'TankMovement' Script Component.");
        }

        shooting.enabled = enable;
        shooting.SetPlayerNumber(playerID);
        hpCanvas.SetActive(enable);
    }

    // 为所有带Mesh Render的子组件染色，包括自己的名字UI，包括团队灯光
    private void RenderPlayerColor()
    {
        //玩家名字，并加上颜色
        coloredPlayerName = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerID + "</color>";

        // 获取所有网眼渲染，并设置颜色。
        MeshRenderer[] renderers = instance.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = playerColor;

        instance.GetComponentInChildren<Light>().color = teamColor;

    }

    // 重置（位置，角度，Active）
    public void Reset(Point spawnPoint)
    {
        this.spawnPoint = spawnPoint;
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = Quaternion.Euler(spawnPoint.rotation);

        //先设置False，因为如果获胜了的玩家本身就是true，重置就会调用OnEnable函数。
        instance.SetActive(false);
        instance.SetActive(true);
    }

    // 获胜
    public void Win()
    {
        ++winTimes;
    }
    
}
