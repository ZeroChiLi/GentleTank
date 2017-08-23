using System.Collections;
using UnityEngine;

namespace Item.Ammo
{
    public abstract class AmmoBase : MonoBehaviour
    {
        public PlayerManager launcher;      // 发射者
        public int level = 50;              // 子弹等级（碰到低等级的子弹时，不会消失）
        public float damage = 50f;          // 伤害值

        private AmmoBase ohterAmmo;         // 临时别的子弹

        /// <summary>
        /// 碰撞物体时响应
        /// </summary>
        /// <param name="other">碰撞的其他物体</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (!gameObject.activeInHierarchy)
                return;
            StartCoroutine(OnCollision(collision));
            if (!IsLowerLevelAmmo(collision))
                gameObject.SetActive(false);
        }

        /// <summary>
        /// 是否是比自己低级的弹药，否则是大于等于自己等级的弹药，或者不是弹药
        /// </summary>
        /// <param name="other">另一个碰撞体</param>
        /// <returns>是否是比自己低级的弹药</returns>
        protected bool IsLowerLevelAmmo(Collision collision)
        {
            ohterAmmo = collision.gameObject.GetComponent<AmmoBase>();
            if (ohterAmmo == null)
                return false;
            return ohterAmmo.level < level;
        }

        /// <summary>
        /// 弹药碰撞物体时响应
        /// </summary>
        /// <param name="other">碰撞到的别的物体</param>
        /// <returns></returns>
        protected abstract IEnumerator OnCollision(Collision collision);
    }
}