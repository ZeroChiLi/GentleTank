using GameSystem.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Item.Tank
{
    public class TankManager : PlayerManager
    {
        public string colorMaterialName = "TankColour";         // 坦克渲染名字
        public MeshRenderer playerIconMesh;                     // 玩家图标材质
        public BoxCollider boxCollider;                         // 碰撞体
        public Slider aimSlider;                                // 瞄准滑动条
        public Text playerNameText;                             // 玩家名字UI
        public SignImageManager signImage;                      // 符号图标
        public ObjectPool signalExpandPool;                     // 信号扩展对象池
        [HideInInspector]
        public MoveManager tankMovement;                       // 移动
        [HideInInspector]
        public TankAttack tankAttack;                           // 攻击
        [HideInInspector]
        public TankHealth tankHealth;                           // 血量
        [HideInInspector]
        public StateController stateController;                 // AI状态控制器
        [HideInInspector]
        public NavMeshAgent navMeshAgent;                       // AI导航

        /// <summary>
        /// 获取所有要用到的私有组件
        /// </summary>
        private void Awake()
        {
            tankMovement = GetComponent<MoveManager>();
            tankAttack = GetComponent<TankAttack>();
            tankHealth = GetComponent<TankHealth>();
            stateController = GetComponent<StateController>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// 初始化坦克,设置坦克Perfabs、获取玩家所在团队、获取实例的必要组件、激活控制权、渲染坦克颜色。
        /// </summary>
        public void Init(Points waypoints)
        {
            SetupUIAndInput(waypoints);                              // 配置玩家名、外部输入
            SetControlEnable(true);                         // 激活相应的控制权
        }

        /// <summary>
        /// 配置信息UI、移动、攻击输入
        /// </summary>
        public void SetupUIAndInput(Points waypoints)
        {
            playerNameText.text = PlayerName;
            ColorTool.ChangeSelfAndChildrens(gameObject, RepresentColor, colorMaterialName);         // 坦克颜色
            if (Team != null)
            {
                playerNameText.color = Team.TeamColor;
                playerIconMesh.material.color = Team.TeamColor;     // 图标颜色
            }
            tankMovement.SetupPlayerInput(PlayerID);                // 配置坦克移动输入
            tankAttack.SetShortcutName("Fire" + PlayerID);        // 配置坦克攻击输入
            stateController.SetWaypoints(waypoints);
        }

        /// <summary>
        /// 设置控制权激活状态
        /// </summary>
        /// <param name="enable">是否激活</param>
        public void SetControlEnable(bool enable)
        {
            if (IsAI)
                SetAIControlEnable(enable);
            else
                SetPlayerControlEnable(enable);

            tankAttack.enabled = enable;
            tankHealth.enabled = enable;
        }

        /// <summary>
        /// AI操控，设置状态控制器,激活 StateController ，关闭 TankMovement
        /// </summary>
        /// <param name="enable">是否激活</param>
        private void SetAIControlEnable(bool enable)
        {
            if (tankMovement != null)
                tankMovement.enabled = false;
            if (stateController != null)
                stateController.SetupAI(enable);
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
            if (tankMovement != null)
                tankMovement.enabled = enable;
            else
                Debug.LogError("If You Want To Control Tank,Need 'TankMovement' Script Component.");
        }

        /// <summary>
        /// 重置出生点以及激活状态
        /// </summary>
        /// <param name="spawnPoint">出生点</param>
        public void ResetToSpawnPoint(Point spawnPoint)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;

            //先设置False，因为如果获胜了的玩家本身就是true，重置就会调用OnEnable函数。
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 显示信号扩展
        /// </summary>
        public void PlaySignalExpand(float radius,float time)
        {
            SignalExpand signal = signalExpandPool.GetNextObject(true).GetComponent<SignalExpand>();
            signal.transform.position = transform.position;
            signal.Play(Vector3.one, Vector3.one * radius, time,Team == null? Color.white : Team.TeamColor);
        }
    }
}