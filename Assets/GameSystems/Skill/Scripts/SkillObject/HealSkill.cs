using System.Collections;
using UnityEngine;

namespace GameSystem.Skill
{
    [CreateAssetMenu(menuName = "Skill/Heal Skill")]
    public class HealSkill : SkillObject
    {
        public ObjectPool healPool;                     // 治愈特效池
        [Range(1, 100)]
        public int skillLevel = 5;                      // 技能等级（次数）
        [Range(1, 100)]
        public float healVolume = 20f;                  // 治愈血量
        [Range(0, 10f)]
        public float healRate = 0.5f;                   // 技能每次释放频率

        private WaitForSeconds healWaitTime;            // 每次治愈时间间隔一样
        private GameObject hitGameObject;               // 技能目标
        private HealthManager targetHealth;             // 目标血量

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            healWaitTime = new WaitForSeconds(healRate);
        }

        /// <summary>
        /// 技能效果
        /// </summary>
        /// <returns></returns>
        public override IEnumerator SkillEffect()
        {
            hitGameObject = AllSkillManager.Instance.aim.HitGameObject;
            targetHealth = hitGameObject.GetComponent<HealthManager>();
            for (int i = 0; i < skillLevel; i++)
                yield return HealPlayer(hitGameObject, targetHealth);
        }

        /// <summary>
        /// 治愈玩家
        /// </summary>
        /// <param name="transform">玩家的位置</param>
        /// <returns></returns>
        private IEnumerator HealPlayer(GameObject player, HealthManager playerHealth)
        {
            //如果死掉了或血条没有激活就直接结束治疗
            if (!player.activeInHierarchy || playerHealth.enabled == false)
                yield break;
            //显示治愈特效
            healPool.GetNextObject().transform.position = player.transform.position;
            //加血
            playerHealth.SetHealthAmount(healVolume);
            yield return healWaitTime;
        }

        /// <summary>
        /// 只有点击了玩家且包含血量组件才释放技能
        /// </summary>
        /// <returns>返回是否点击玩家</returns>
        public override bool ReleaseCondition()
        {
            if (AllSkillManager.Instance.aim.HitGameObject != null)
                Debug.Log(AllSkillManager.Instance.aim.HitGameObject);
            if (AllSkillManager.Instance.aim.HitGameObject.GetComponent<HealthManager>())
                Debug.Log("bbb");
            return AllSkillManager.Instance.aim.HitGameObject != null && AllSkillManager.Instance.aim.HitGameObject.GetComponent<HealthManager>() != null;
        }
    }
}