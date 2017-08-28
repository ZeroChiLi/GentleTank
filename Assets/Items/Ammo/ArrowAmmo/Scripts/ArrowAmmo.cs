using System.Collections;
using UnityEngine;

namespace Item.Ammo
{
    public class ArrowAmmo : AmmoBase
    {
        public ObjectPool arrowHitPool;                     // 箭头命中池
        public float inactiveDelay = 1f;                    // 延时消失时间

        private HealthManager targetHealth;                 // 目标血量

        protected override void OnCollision(Collider other)
        {
            arrowHitPool.GetNextObject(true, transform);
            targetHealth = other.GetComponent<HealthManager>();
            if (targetHealth != null)
                targetHealth.SetHealthAmount(-damage);
        }

        /// <summary>
        /// 要被摧毁该弓箭前，如果碰到别的弹药直接消失，否则一段时间后消失
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected override void OnCrashed(Collider other)
        {
            if (otherAmmo != null)
                base.OnCrashed(other);
            else
                StartCoroutine(DelayInactive(other));
            return;
        }

        /// <summary>
        /// 延时消失弓箭
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private IEnumerator DelayInactive(Collider other)
        {
            ammoCollider.enabled = false;
            ammoRb.Sleep();
            ammoRb.isKinematic = true;

            yield return new WaitForSeconds(inactiveDelay);

            ammoRb.isKinematic = false;
            ammoCollider.enabled = true;
            base.OnCrashed(other);
        }
    }
}