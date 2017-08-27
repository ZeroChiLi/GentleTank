using System.Collections;
using UnityEngine;

namespace Item.Ammo
{
    public class ArrowAmmo : AmmoBase
    {
        public ObjectPool arrowHitPool;                     // 箭头命中池
        public float delayInactive = 0.5f;                  // 延时消失

        private HealthManager targetHealth;                 // 目标血量

        protected override IEnumerator OnCollision(Collider other)
        {
            arrowHitPool.GetNextObject(true, transform);
            targetHealth = other.GetComponent<HealthManager>();
            if (targetHealth != null)
                targetHealth.SetHealthAmount(-damage);
            yield break;
        }

        /// <summary>
        /// 要被摧毁该弓箭前，先模拟插在目标上，一段时间后消失
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected override IEnumerator OnCrashed(Collider other)
        {
            if (otherAmmo != null)      // 如果碰到别的弹药，那就直接消失吧
            {
                yield return base.OnCrashed(other);
                yield break;
            }
            ammoCollider.enabled = false;
            ammoRb.Sleep();
            ammoRb.isKinematic = true;

            yield return new WaitForSeconds(delayInactive);

            ammoRb.isKinematic = false;
            ammoCollider.enabled = true;
            yield return base.OnCrashed(other);
        }
    }
}