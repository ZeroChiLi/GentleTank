using GameSystem.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Item.Tank
{
    public class TankManager : MonoBehaviour
    {
        [HideInInspector]
        public PlayerManager playerManager;                     // 玩家信息
        [HideInInspector]
        public PlayerInfoUI playerInfoUI;                       // UI信息
        [HideInInspector]
        public TankMovement tankMovement;                       // 移动
        [HideInInspector]
        public TankShooting tankShooting;                       // 攻击
        [HideInInspector]
        public HealthManager tankHealth;                        // 血量
        [HideInInspector]
        public StateController stateController;                 // AI状态控制器
        [HideInInspector]
        public NavMeshAgent navMeshAgent;                       // AI导航

        /// <summary>
        /// 初始化坦克,设置坦克Perfabs、获取玩家所在团队、获取实例的必要组件、激活控制权、渲染坦克颜色。
        /// </summary>
        public void Init()
        {
            SetupComponent();                               // 获取私有组件
            SetupUIAndInput();                              // 配置玩家名、外部输入
            SetControlEnable(true);                         // 激活相应的控制权
        }

        /// <summary>
        /// 获取所有要用到的私有组件
        /// </summary>
        private void SetupComponent()
        {
            playerManager = GetComponent<PlayerManager>();
            tankMovement = GetComponent<TankMovement>();
            tankShooting = GetComponent<TankShooting>();
            tankHealth = GetComponent<HealthManager>();
            playerInfoUI = GetComponent<PlayerInfoUI>();

            stateController = GetComponent<StateController>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// 配置信息UI、移动、攻击输入
        /// </summary>
        public void SetupUIAndInput()
        {
            playerInfoUI.SetupNameAndColor();   // 配置玩家信息UI的名字和颜色
            ChangeColor.SelfAndChildrens(gameObject, playerManager.RepresentColor);         // 坦克颜色
            tankMovement.SetupPlayerInput(playerManager.PlayerID);              // 配置坦克移动输入
            tankShooting.SetShortcutName("Fire" + playerManager.PlayerID);      // 配置坦克攻击输入
        }

        /// <summary>
        /// 设置控制权激活状态
        /// </summary>
        /// <param name="enable">是否激活</param>
        public void SetControlEnable(bool enable)
        {
            if (playerManager.IsAI)
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