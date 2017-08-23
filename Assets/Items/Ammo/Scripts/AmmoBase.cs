using System.Collections;
using UnityEngine;

namespace Item.Ammo
{
    //[RequireComponent(typeof(Rigidbody),typeof(Collider))]
    public abstract class AmmoBase : MonoBehaviour
    {
        public PlayerManager launcher;      // 发射者
        public int level = 50;              // 子弹等级（碰到低等级的子弹时，不会消失）
        public float damage = 50f;          // 伤害值

        private Rigidbody rigidbodySelf;    // 自己的刚体
        private AmmoBase ohterAmmo;         // 临时别的子弹

        /// <summary>
        /// 初始化弹药
        /// </summary>
        /// <param name="launcher">发射者</param>
        /// <param name="damage">伤害值</param>
        public void Init(PlayerManager launcher,float damage)
        {
            this.launcher = launcher;
            this.damage = damage;
        }

        /// <summary>
        /// 获取刚体组件
        /// </summary>
        protected void Awake()
        {
            rigidbodySelf = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// 失活时把刚体睡了
        /// </summary>
        protected void OnDisable()
        {
            if (rigidbodySelf != null)
                rigidbodySelf.Sleep();
        }

        /// <summary>
        /// 碰撞物体时响应
        /// </summary>
        /// <param name="other">碰撞的其他物体</param>
        private void OnTriggerEnter(Collider other)
        {
            // 如果已经失活了，或者碰到自己，就跳过
            if (!gameObject.activeInHierarchy || launcher == other.GetComponent<PlayerManager>())
                return;
            StartCoroutine(OnCollision(other));
            if (!IsLowerLevelAmmo(other))
                gameObject.SetActive(false);
        }

        /// <summary>
        /// 是否是比自己低级的弹药，否则是大于等于自己等级的弹药，或者不是弹药
        /// </summary>
        /// <param name="other">另一个碰撞体</param>
        /// <returns>是否是比自己低级的弹药</returns>
        protected bool IsLowerLevelAmmo(Collider other)
        {
            ohterAmmo = other.GetComponent<AmmoBase>();
            if (ohterAmmo == null)
                return false;
            return ohterAmmo.level < level;
        }

        /// <summary>
        /// 弹药碰撞物体时响应
        /// </summary>
        /// <param name="other">碰撞到的别的物体</param>
        /// <returns></returns>
        protected abstract IEnumerator OnCollision(Collider other);
    }
}