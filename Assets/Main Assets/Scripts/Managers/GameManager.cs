using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Complete
{
    public class GameManager : MonoBehaviour
    {
        public int m_NumRoundsToWin = 5;            // 赢得游戏需要赢的回合数
        public float m_StartDelay = 3f;             // 开始延时时间
        public float m_EndDelay = 3f;               // 结束延时时间
        public CameraControl m_CameraControl;       // 相机控制脚本
        public CameraFollowTarget followCamera;     // 跟踪相机，用于小地图
        public Text m_MessageText;                  // UI文本（玩家获胜等）
        public TankManager[] m_Tanks;               // 坦克管理器
        public List<Transform> wayPointsForAI;      // AI的巡逻点列表

        private int m_RoundNumber;                  // 当前回合数
        private WaitForSeconds m_StartWait;         // 开始回合延时
        private WaitForSeconds m_EndWait;           // 结束回合延时
        private TankManager m_RoundWinner;          // 当前回合获胜玩家
        private TankManager m_GameWinner;           // 最终获胜玩家

        private void Awake()
        {
            m_StartWait = new WaitForSeconds(m_StartDelay);
            m_EndWait = new WaitForSeconds(m_EndDelay);
        }

        private void Start()
        {
            SpawnAllTanks();
            SetCameraTargets();

            followCamera.SetTarget(m_Tanks[0].m_Instance);     //设置小地图跟随目标为第一个玩家

            // 开始游戏循环（检测获胜者，重新回合，结束游戏等）
            StartCoroutine(GameLoop());
        }

        // 产生所有坦克，包括玩家和AI
        private void SpawnAllTanks()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].m_Instance =
                    Instantiate(m_Tanks[i].tankPerfab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
                m_Tanks[i].m_PlayerNumber = i + 1;

                m_Tanks[i].SetupTank(m_Tanks[i].isAI, wayPointsForAI);
            }
        }

        // 给相机添加所有坦克
        private void SetCameraTargets()
        {
            Transform[] targets = new Transform[m_Tanks.Length];

            for (int i = 0; i < targets.Length; i++)
                targets[i] = m_Tanks[i].m_Instance.transform;

            m_CameraControl.m_Targets = targets;
        }

        // 游戏的协程
        private IEnumerator GameLoop()
        {
            yield return StartCoroutine(RoundStarting());           //回合开始，有一段延时

            yield return StartCoroutine(RoundPlaying());            //回合中，多于一个坦克存活时一直在这里死循环

            yield return StartCoroutine(RoundEnding());             //回合结束

            // 如果结束了游戏，重新加载场景，否则进行下一回合
            if (m_GameWinner != null)
                SceneManager.LoadScene(0);
            else
                StartCoroutine(GameLoop());
        }

        // 回合开始
        private IEnumerator RoundStarting()
        {
            ResetAllTanks();                                // 重置所有坦克
            DisableTankControl();                           // 并且锁定他们的控制权

            m_CameraControl.SetStartPositionAndSize();      // 重置相机

            ++m_RoundNumber;                                // 回合数增加                
            m_MessageText.text = "ROUND " + m_RoundNumber;

            yield return m_StartWait;                       // 延时一段时间再开始
        }

        // 回合中
        private IEnumerator RoundPlaying()
        {
            EnableTankControl();                            // 解锁玩家控制权

            m_MessageText.text = string.Empty;              // 清空显示信息

            while (!OneTankLeft())                          // 只剩一个坦克才结束该协程
                yield return null;
        }

        // 回合结束
        private IEnumerator RoundEnding()
        {
            DisableTankControl();                           // 锁定玩家控制权

            m_RoundWinner = GetRoundWinner();               // 获取回合胜利的玩家

            if (m_RoundWinner != null)                      // 不为空就给胜出玩家加获胜次数
                m_RoundWinner.m_Wins++;

            m_GameWinner = GetGameWinner();                 // 获取最终获胜玩家

            string message = EndMessage();                  // 获取结束信息并显示之
            m_MessageText.text = message;

            yield return m_EndWait;
        }

        // 返回是否小于等于一个坦克存活（0个说明是同归了）
        private bool OneTankLeft()
        {
            int numTanksLeft = 0;

            for (int i = 0; i < m_Tanks.Length; i++)
                if (m_Tanks[i].m_Instance.activeSelf)
                    numTanksLeft++;

            return numTanksLeft <= 1;
        }

        // 获取获胜的玩家，为空就是平局
        private TankManager GetRoundWinner()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
                if (m_Tanks[i].m_Instance.activeSelf)
                    return m_Tanks[i];

            return null;
        }

        // 获取最终胜利的玩家
        private TankManager GetGameWinner()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
                if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                    return m_Tanks[i];

            return null;
        }

        // 获取回合或总的游戏结束信息
        private string EndMessage()
        {
            string message = "DRAW!";                       // 默认平局

            // 添加获胜玩家的带颜色的玩家名字字符串
            if (m_RoundWinner != null)
                message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

            message += "\n\n\n\n";

            // 添加所有玩家获胜次数
            for (int i = 0; i < m_Tanks.Length; i++)
                message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";

            // 添加最后获胜玩家
            if (m_GameWinner != null)
                message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

            return message;
        }

        // 重置所有坦克
        private void ResetAllTanks()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
                m_Tanks[i].Reset();
        }

        // 锁定所有玩家控制权
        private void EnableTankControl()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
                m_Tanks[i].EnableControl();
        }

        // 解锁所有玩家控制权
        private void DisableTankControl()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
                m_Tanks[i].DisableControl();
        }
    }
}