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
        public Color m_PlayerColor;                             // 渲染颜色
        public Transform m_SpawnPoint;                          // 出生点

        [HideInInspector] public int m_PlayerNumber;            // 玩家编号
        [HideInInspector] public string m_ColoredPlayerText;    // 表示玩家颜色的HTML格式颜色
        [HideInInspector] public GameObject m_Instance;         // 玩家实例
        [HideInInspector] public int m_Wins;                    // 玩家回合获胜次数
		[HideInInspector] public List<Transform> m_WayPointList;// AI巡逻点

        private TankMovement m_Movement;                        // 移动脚本
        private TankShooting m_Shooting;                        // 攻击脚本
        private GameObject m_CanvasGameObject;                  // 显示玩家信息UI（血量）
		private StateController m_StateController;				// AI状态控制器

        // 配置坦克
		public void SetupTank(bool isAI, List<Transform> wayPointList)
        {
            if(isAI)
            {
                //是AI就添加巡逻点
                m_StateController = m_Instance.GetComponent<StateController>();
                m_StateController.SetupAI(true, wayPointList);
            }
            else
            {
                //不是AI就设置控制输入
                m_Movement = m_Instance.GetComponent<TankMovement> ();
                m_Movement.m_PlayerNumber = m_PlayerNumber;
            }

            m_Shooting = m_Instance.GetComponent<TankShooting> ();
            m_Shooting.m_PlayerNumber = m_PlayerNumber;

            m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas> ().gameObject;

            m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

            // 获取所有网眼渲染，并设置颜色。
            MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer> ();
            for (int i = 0; i < renderers.Length; i++)
                renderers[i].material.color = m_PlayerColor;
        }

        // 锁定控制权（玩家移动控制权、AI状态控制权、攻击权）
        public void DisableControl()
        {
			if (m_Movement != null)
            m_Movement.enabled = false;

			if (m_StateController != null)
				m_StateController.enabled = false;

            m_Shooting.enabled = false;

            m_CanvasGameObject.SetActive(false);
        }

        // 解锁控制权
        public void EnableControl()
        {
			if (m_Movement != null)
            m_Movement.enabled = true;

			if (m_StateController != null)
				m_StateController.enabled = true;

            m_Shooting.enabled = true;

            m_CanvasGameObject.SetActive (true);
        }

        // 重置（位置，角度，Active）
        public void Reset()
        {
            m_Instance.transform.position = m_SpawnPoint.position;
            m_Instance.transform.rotation = m_SpawnPoint.rotation;

            //先设置False，因为如果获胜了的玩家本身就是true，重置就会调用OnEnable函数。
            m_Instance.SetActive (false);
            m_Instance.SetActive (true);
        }
    }
}