using System.Collections.Generic;
using UnityEngine;

namespace Widget.ChargeArea
{
    public class AllChargeAreaManager : MonoBehaviour
    {
        public PointList chargeAreaPointList;                       // 充电区产生的位置
        public GameObject chargeAreaPerfab;                         // 充电区预设

        private List<ChargeAreaManager> chargeAreaList = new List<ChargeAreaManager>(); // 所有充电区
        private GameState lastGameState = GameState.None;           // 上一次回合状态，通过这个来触发充电区状态变化

        /// <summary>
        /// 创建充电区们
        /// </summary>
        private void Awake()
        {
            CreateChargeAreas();
        }

        /// <summary>
        /// 通过点列表创建充电区们
        /// </summary>
        private void CreateChargeAreas()
        {
            for (int i = 0; i < chargeAreaPointList.Count; i++)
                chargeAreaList.Add(Instantiate(chargeAreaPerfab, chargeAreaPointList[i].position, chargeAreaPointList[i].Rotation, transform).GetComponent<ChargeAreaManager>());
        }

        /// <summary>
        /// 更新充电区状态
        /// </summary>
        private void Update()
        {
            ChangeChargeAreaStateWithGameRoundState();
        }

        /// <summary>
        /// 通过游戏的回合状态改变充电区是否激活状态
        /// </summary>
        private void ChangeChargeAreaStateWithGameRoundState()
        {
            if (lastGameState != GameState.Playing && GameRecord.Instance.CurrentGameState == GameState.Playing)
            {
                lastGameState = GameState.Playing;
                OpenChargeAreas();
            }
            else if (lastGameState == GameState.Playing && GameRecord.Instance.CurrentGameState != GameState.Playing)
            {
                lastGameState = GameState.None;
                ShutChargeAreas();
            }
        }

        /// <summary>
        /// 开启所有充电区
        /// </summary>
        private void OpenChargeAreas()
        {
            for (int i = 0; i < chargeAreaList.Count; i++)
                chargeAreaList[i].enabled = true;
        }

        /// <summary>
        /// 关闭所有充电区，并重置
        /// </summary>
        private void ShutChargeAreas()
        {
            for (int i = 0; i < chargeAreaList.Count; i++)
            {
                chargeAreaList[i].ResetChargeArea();
                chargeAreaList[i].enabled = false;
            }
        }

    }
}