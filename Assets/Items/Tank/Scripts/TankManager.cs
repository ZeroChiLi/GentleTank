using GameSystem.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Item.Tank
{
    public class TankManager : PlayerManager
    {
        public GameObject tankRenderers;                        // 坦克渲染部件
        public MeshRenderer playerIconMesh;                     // 玩家图标材质
        public Text playerNameText;                             // 玩家名字UI
        [HideInInspector]
        public TankMovement tankMovement;                       // 移动
        [HideInInspector]
        public TankShooting tankShooting;                       // 攻击
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
            tankMovement = GetComponent<TankMovement>();
            tankShooting = GetComponent<TankShooting>();
            tankHealth = GetComponent<TankHealth>();
            //playerInfoUI = GetComponent<PlayerInfoUI>();

            stateController = GetComponent<StateController>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// 初始化坦克,设置坦克Perfabs、获取玩家所在团队、获取实例的必要组件、激活控制权、渲染坦克颜色。
        /// </summary>
        public void Init()
        {
            SetupUIAndInput();                              // 配置玩家名、外部输入
            SetControlEnable(true);                         // 激活相应的控制权
        }

        /// <summary>
        /// 配置信息UI、移动、攻击输入
        /// </summary>
        public void SetupUIAndInput()
        {
            playerNameText.text = PlayerName;
            ChangeColor.SelfAndChildrens(tankRenderers, RepresentColor);         // 坦克颜色
            if (Team != null)
            {
                playerNameText.color = Team.TeamColor;
                playerIconMesh.material.color = Team.TeamColor;     // 图标颜色
            }
            tankMovement.SetupPlayerInput(PlayerID);                // 配置坦克移动输入
            tankShooting.SetShortcutName("Fire" + PlayerID);        // 配置坦克攻击输入
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

            tankShooting.enabled = enable;
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
        public void ResetSpawnPoint(Point spawnPoint)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.Rotation;

            //先设置False，因为如果获胜了的玩家本身就是true，重置就会调用OnEnable函数。
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}