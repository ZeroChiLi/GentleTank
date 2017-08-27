using System.Collections;
using UnityEngine;

namespace Item.Ammo
{
    public class ArrowAmmo : AmmoBase
    {
        public ObjectPool arrowHitPool;                     // 箭头命中池

        private HealthManager targetHealth;                 // 目标血量

        protected override IEnumerator OnCollision(Collider other)
        {
            arrowHitPool.GetNextObject(true, transform);
            targetHealth = other.GetComponent<HealthManager>();
            if (targetHealth != null)
                targetHealth.SetHealthAmount(-damage);
            yield break;
        }
    }
}