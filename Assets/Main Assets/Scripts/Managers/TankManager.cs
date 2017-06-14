using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

namespace Complete
{
    [Serializable]
    public class TankManager
    {
        public GameObject tankPerfab;                           // 坦克预设
        public bool isAI;                                       // 是否是AI
        public Color playerColor;                               // 渲染颜色

        [HideInInspector] public Transform spawnPoint;          // 出生点
        [HideInInspector] public int playerNumber;              // 玩家编号
        [HideInInspector] public string coloredPlayerText;      // 表示玩家颜色的HTML格式颜色
        [HideInInspector] public GameObject instance;           // 玩家实例
        [HideInInspector] public int winsTime;                  // 玩家回合获胜次数
		[HideInInspector] public List<Transform> wayPointList;  // AI巡逻点

        private TankMovement movement;                          // 移动脚本
        private TankShooting shooting;                          // 攻击脚本
        private GameObject hpCanvas;                            // 显示玩家信息UI（血量）
		private StateController stateController;				// AI状态控制器
        private NavMeshAgent navMeshAgent;                      // AI导航

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
                    stateController.SetupAI(true, wayPointList);
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
                    movement.SetPlayerNumber(playerNumber);
                }
                else
                    Debug.LogError("If You Want To Control Tank,Need 'TankMovement' Script Component.");
            }

            shooting.enabled = enable;
            shooting.SetPlayerNumber(playerNumber);
            hpCanvas.SetActive(enable);
        }

        // 为所有带Mesh Render的子组件染色，包括自己的名字UI
        private void RenderPlayerColor()
        {
            //玩家名字，并加上颜色
            coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerNumber + "</color>";

            // 获取所有网眼渲染，并设置颜色。
            MeshRenderer[] renderers = instance.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
                renderers[i].material.color = playerColor;
        }

        // 重置（位置，角度，Active）
        public void Reset()
        {
            instance.transform.position = spawnPoint.position;
            instance.transform.rotation = spawnPoint.rotation;

            //先设置False，因为如果获胜了的玩家本身就是true，重置就会调用OnEnable函数。
            instance.SetActive (false);
            instance.SetActive (true);
        }
    }
}