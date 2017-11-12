using System.Collections;
using UnityEngine;

namespace Item.Ammo
{
    public class ArrowAmmo : AmmoBase
    {
        public ObjectPool arrowHitPool;                     // 箭头命中池
        public float inactiveDelay = 1f;                    // 延时消失时间

        private HealthManager targetHealth;                 // 目标血量
        private Transform preParent;

        private new void OnEnable()
        {
            base.OnEnable();
            if (preParent != null)
            {
                transform.SetParent(preParent);
                preParent = null;
            }
        }

        private new void OnDisable()
        {
            base.OnDisable();
            gameObject.SetActive(false);
        }

        protected override void OnCollision(Collider other)
        {
            arrowHitPool.GetNextObject(true, transform);
            targetHealth = other.GetComponentInParent<HealthManager>();
            if (targetHealth != null)
                targetHealth.SetHealthAmount(-damage, launcher);

            // 要被摧毁该弓箭前，如果碰到别的弹药直接消失，否则一段时间后消失
            if (otherAmmo != null)
                gameObject.SetActive(false);
            else
                StartCoroutine(DelayInactive(other));
            return;
        }

        /// <summary>
        /// 延时消失弓箭
        /// </summary>
        private IEnumerator DelayInactive(Collider other)
        {
            ammoCollider.enabled = false;
            ammoRb.Sleep();
            ammoRb.isKinematic = true;
            if (other.gameObject.activeInHierarchy)
            {
                preParent = transform.parent;
                transform.SetParent(other.transform);
            }

            yield return new WaitForSeconds(inactiveDelay);

            if (preParent != null)
            {
                transform.SetParent(preParent);
                preParent = null;
            }
            gameObject.SetActive(false);
            ammoRb.isKinematic = false;
            ammoCollider.enabled = true;
        }

    }
}