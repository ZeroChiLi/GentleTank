using System.Collections;
using UnityEngine;

namespace Item.Ammo
{
    [RequireComponent(typeof(Collider))]
    public class SpringBoxingAmmo : AmmoBase
    {
        /// <summary>
        /// 初始化记录位置信息
        /// </summary>
        private void Start()
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