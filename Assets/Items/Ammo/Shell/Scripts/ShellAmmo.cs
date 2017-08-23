using System.Collections;
using UnityEngine;

namespace Item.Ammo
{
    public class ShellAmmo : AmmoBase
    {
        public new Rigidbody rigidbody;                     // 自己的刚体
        public ObjectPool shellExplosionPool;               // 爆炸特性池
        public float explosionForce = 100f;                 // 爆炸中心的能量
        public float explosionRadius = 5f;                  // 爆炸半径

        // 把每次需要用到的临时变量拉出来
        private Collider[] colliders;                       // 碰撞物体们
        private Rigidbody targetRigidbody;                  // 目标刚体
        private HealthManager targetHealth;                 // 目标血量

        private void OnDisable()
        {
            rigidbody.Sleep();
        }

        protected override IEnumerator OnCollision(Collision other)
        {
            // 从爆炸池中获取对象，并设置位置，显示之
            shellExplosionPool.GetNextObject(transform: transform);

            // 获取爆炸范围内所有碰撞体
            colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            for (int i = 0; i < colliders.Length; i++)
            {
                targetRigidbody = colliders[i].GetComponent<Rigidbody>();
                //AddForce(colliders[i]);
                TakeDamage(colliders[i]);
            }
            yield return null;
        }

        /// <summary>
        /// 给一个爆炸力,对AI无效（NavMeshAgent导航的时候，下面这语句不会实现）
        /// </summary>
        /// <param name="collider"></param>
        private void AddForce(Collider collider)
        {
            if (!targetRigidbody)
                return;
            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        /// <summary>
        /// 获取目标的血条，计算扣血量并给给扣血。
        /// </summary>
        /// <param name="collider"></param>
        private void TakeDamage(Collider collider)
        {
            targetHealth = collider.GetComponent<HealthManager>();
            if (!targetHealth)
                return;
            targetHealth.SetHealthAmount(-1 * CalculateDamage(collider.transform.position));
        }

        /// <summary>
        /// 根据距离计算伤害值
        /// </summary>
        /// <param name="targetPosition">目标位置</param>
        /// <returns></returns>
        private float CalculateDamage(Vector3 targetPosition)
        {
            // 计算爆炸中心距离和自己的距离
            Vector3 explosionToTarget = targetPosition - transform.position;
            float explosionDistance = explosionToTarget.magnitude;

            // 转换成比例
            float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

            // 根据比例计算伤害
            return Mathf.Max(0f, relativeDistance * damage);
        }

    }
}