using System.Collections;
using UnityEngine;

namespace Item.Ammo
{
    public class BoxingAmmo : AmmoBase
    {
        public ObjectPool effectPool;                   // 特效池

        private HealthManager targetHealth;             // 目标血量

        /// <summary>
        /// 弹簧拳是金刚不坏拳，不会坏的
        /// </summary>
        protected void Start()
        {
            IsIndestructible = true;
        }
        protected override IEnumerator OnCollision(Collider other)
        {
            effectPool.GetNextObject(true,transform);
            targetHealth = other.GetComponent<HealthManager>();
            if (targetHealth != null)
                targetHealth.SetHealthAmount(-damage);
            yield break;
        }
    }
}