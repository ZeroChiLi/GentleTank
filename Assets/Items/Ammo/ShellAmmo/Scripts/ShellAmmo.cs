using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item.Ammo
{
    public class ShellAmmo : AmmoBase
    {
        public ObjectPool shellExplosionPool;               // 爆炸特性池
        public float explosionForce = 100f;                 // 爆炸中心的能量
        public float explosionRadius = 5f;                  // 爆炸半径

        private HealthManager targetHealth;                 // 目标血量
        private List<HealthManager> validTargets = new List<HealthManager>();          // 临时有效玩家列表        

        protected override void OnCollision(Collider other)
        {
        }

        protected override void OnCrashed(Collider other)
        {
            // 从爆炸池中获取对象，并设置位置，显示之
            shellExplosionPool.GetNextObject(true, transform);

            // 获取爆炸范围内所有碰撞体
            ComponentUtility.GetUniquelyComponentInParent(Physics.OverlapSphere(transform.position, explosionRadius), ref validTargets);

            for (int i = 0; i < validTargets.Count; i++)
                TakeDamage(validTargets[i]);

            gameObject.SetActive(false);
        }

        /// <summary>
        /// 获取目标的血条，计算扣血量并给给扣血。
        /// </summary>
        /// <param name="targetHealth">目标的血条</param>
        protected void TakeDamage(HealthManager targetHealth)
        {
            // 计算目标距离爆炸中心比例值（0 ~ 1,0为中），越靠近伤害越大，线性的
            targetHealth.SetHealthAmount(-1 * Mathf.Max(0f, GameMathf.Persents(explosionRadius, 0, (targetHealth.transform.position - transform.position).magnitude) * damage), launcher);
        }


    }
}