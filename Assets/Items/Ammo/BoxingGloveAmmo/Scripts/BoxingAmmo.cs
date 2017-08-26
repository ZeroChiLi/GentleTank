using System.Collections;
using UnityEngine;

namespace Item.Ammo
{
    public class BoxingAmmo : AmmoBase
    {
        /// <summary>
        /// 弹簧拳是金刚不坏拳，不会坏的
        /// </summary>
        protected void Start()
        {
            IsIndestructible = true;
        }

        private HealthManager targetHealth;                 // 目标血量

        protected override IEnumerator OnCollision(Collider other)
        {
            targetHealth = other.GetComponent<HealthManager>();
            if (targetHealth != null)
                targetHealth.SetHealthAmount(-damage);
            yield break;
        }
    }
}