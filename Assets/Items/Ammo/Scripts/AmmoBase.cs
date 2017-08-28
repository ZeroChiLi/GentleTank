using System.Collections;
using UnityEngine;

namespace Item.Ammo
{
    [RequireComponent(typeof(Collider))]
    public abstract class AmmoBase : MonoBehaviour
    {
        public PlayerManager launcher;      // 发射者
        public Rigidbody ammoRb;            // 自己的刚体
        public float damage = 50f;          // 伤害值
        [SerializeField]
        protected bool IsIndestructible;    // 是否无法摧毁的（不受耐久影响）
        [SerializeField]
        protected int durability = 50;      // 子弹耐久度（碰到别的子弹，会根据别子弹的耐久值减去自己耐久值）

        public int Durability { get { return durability; } }
        public int CurrentDurability { get { return currentDruability; } }

        protected Collider ammoCollider;    // 子弹的碰撞体
        protected int currentDruability;    // 当前子弹耐久度
        protected AmmoBase otherAmmo;       // 临时别的子弹

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
        /// 获取碰撞体，设置isTrigger，这样才能发生碰撞响应
        /// </summary>
        protected void Awake()
        {
            ammoCollider = GetComponent<Collider>();
            ammoCollider.isTrigger = true;
        }

        /// <summary>
        /// 激活时重置耐久度
        /// </summary>
        protected void OnEnable()
        {
            currentDruability = durability;
        }

        /// <summary>
        /// 失活时把刚体睡了
        /// </summary>
        protected void OnDisable()
        {
            if (ammoRb != null)
                ammoRb.Sleep();
        }

        /// <summary>
        /// 碰撞物体时响应
        /// </summary>
        /// <param name="other">碰撞的其他物体</param>
        private void OnTriggerEnter(Collider other)
        {
            // 如果已经失活了，或者碰到自己，或者标记无视弹药层、就跳过
            if (!gameObject.activeInHierarchy || (launcher != null && launcher == other.GetComponent<PlayerManager>()) || other.gameObject.layer == LayerMask.NameToLayer("IgnoreAmmo"))
                return;
            OnCollision(other);
            if (!IsIndestructible && DruabilityLowerThanZero(other))
                OnCrashed(other);
        }

        /// <summary>
        /// 计算当前耐久值，返回是否小于等于0
        /// </summary>
        /// <param name="other">另一个碰撞体</param>
        /// <returns>耐久值是否小于等于0</returns>
        protected bool DruabilityLowerThanZero(Collider other)
        {
            otherAmmo = other.GetComponent<AmmoBase>();
            if (otherAmmo == null)
            {
                currentDruability = 0;
                return true;
            }
            currentDruability -= otherAmmo.Durability;
            return currentDruability <= 0;
        }

        /// <summary>
        /// 弹药碰撞物体时响应
        /// </summary>
        /// <param name="other">碰撞到的别的物体</param>
        /// <returns></returns>
        protected abstract void OnCollision(Collider other);

        /// <summary>
        /// 耐久值小于等于0时响应
        /// </summary>
        /// <returns></returns>
        protected virtual void OnCrashed(Collider other)
        {
            gameObject.SetActive(false);
        }
    }
}