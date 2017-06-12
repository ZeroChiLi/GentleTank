using System;
using UnityEngine;
using System.Collections.Generic;

namespace Complete
{
    [Serializable]
    public class TankManager
    {
        public GameObject tankPerfab;                           // 坦克预设
        public bool isAI;                                       // 是否是AI
        public Color playerColor;                               // 渲染颜色
        public Transform spawnPoint;                            // 出生点

        [HideInInspector] public int playerNumber;              // 玩家编号
        [HideInInspector] public string coloredPlayerText;      // 表示玩家颜色的HTML格式颜色
        [HideInInspector] public GameObject instance;           // 玩家实例
        [HideInInspector] public int winsTime;                  // 玩家回合获胜次数
		[HideInInspector] public List<Transform> wayPointList;  // AI巡逻点

        private TankMovement movement;                          // 移动脚本
        private TankShooting shooting;                          // 攻击脚本
        private GameObject canvasGameObject;                    // 显示玩家信息UI（血量）
		private StateController stateController;				// AI状态控制器

        // 配置坦克
		public void SetupTank(bool isAI, List<Transform> wayPointList)
        {
            if(isAI)
            {
                //是AI就添加巡逻点
                stateController = instance.GetComponent<StateController>();
                stateController.SetupAI(true, wayPointList);
            }
            else
            {
                //不是AI就设置控制输入
                movement = instance.GetComponent<TankMovement> ();
                movement.m_PlayerNumber = playerNumber;
            }

            shooting = instance.GetComponent<TankShooting> ();
            shooting.m_PlayerNumber = playerNumber;

            canvasGameObject = instance.GetComponentInChildren<Canvas> ().gameObject;

            coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerNumber + "</color>";

            // 获取所有网眼渲染，并设置颜色。
            MeshRenderer[] renderers = instance.GetComponentsInChildren<MeshRenderer> ();
            for (int i = 0; i < renderers.Length; i++)
                renderers[i].material.color = playerColor;
        }

        // 锁定控制权（玩家移动控制权、AI状态控制权、攻击权）
        public void DisableControl()
        {
			if (movement != null)
            movement.enabled = false;

			if (stateController != null)
				stateController.enabled = false;

            shooting.enabled = false;

            canvasGameObject.SetActive(false);
        }

        // 解锁控制权
        public void EnableControl()
        {
			if (movement != null)
            movement.enabled = true;

			if (stateController != null)
				stateController.enabled = true;

            shooting.enabled = true;

            canvasGameObject.SetActive (true);
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