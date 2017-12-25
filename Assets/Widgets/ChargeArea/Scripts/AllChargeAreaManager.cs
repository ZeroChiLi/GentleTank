using System.Collections.Generic;
using UnityEngine;

namespace Widget.ChargeArea
{
    public class AllChargeAreaManager : MonoBehaviour
    {
        public Points chargeAreaPoints;                       // 充电区产生的位置
        public GameObject chargeAreaPerfab;                         // 充电区预设
        public ObjectPool fillAreaObjectPool;                       // 填充扇区池

        private List<ChargeAreaManager> chargeAreaList = new List<ChargeAreaManager>(); // 所有充电区
        private GameState lastGameState = GameState.None;           // 上一次回合状态，通过这个来触发充电区状态变化

        /// <summary>
        /// 创建充电区们
        /// </summary>
        private void Awake()
        {
            CreateChargeAreas();
            fillAreaObjectPool.CreateObjectPool(this.gameObject);
        }

        private void Start()
        {
            // 注册回合监听
            GameRound.Instance.OnGameRoundStartEvent.AddListener(() => { OpenChargeAreas(); });
            GameRound.Instance.OnGameRoundEndEvent.AddListener(() => { ShutChargeAreas(); });
        }

        /// <summary>
        /// 通过点列表创建充电区们
        /// </summary>
        private void CreateChargeAreas()
        {
            for (int i = 0; i < chargeAreaPoints.Count; i++)
                chargeAreaList.Add(Instantiate(chargeAreaPerfab, chargeAreaPoints[i].position, chargeAreaPoints[i].rotation, transform).GetComponent<ChargeAreaManager>());
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