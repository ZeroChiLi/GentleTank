using System.Collections;
using UnityEngine;

namespace Item.Ammo
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public abstract class AmmoBase : MonoBehaviour
    {
        public PlayerManager launcher;      // 发射者
        public float damage = 50f;          // 伤害值
        [SerializeField]
        private int durability = 50;        // 子弹耐久度（碰到别的子弹，会根据别子弹的耐久值减去自己耐久值）

        public int Durability { get { return durability; } }

        private int currentDruability;      // 当前子弹耐久度
        private Rigidbody rigidbodySelf;    // 自己的刚体
        private AmmoBase otherAmmo;         // 临时别的子弹

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
        /// 激活时重置耐久度
        /// </summary>
        private void OnEnable()
        {
            currentDruability = durability;
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
            if (!gameObject.activeInHierarchy || (launcher != null && launcher == other.GetComponent<PlayerManager>()))
                return;
            StartCoroutine(OnCollision(other));
            if (DruabilityLowerThanZero(other))
                StartCoroutine(OnCrashed());
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
        protected abstract IEnumerator OnCollision(Collider other);

        /// <summary>
        /// 碰到非低级弹药时（大于等于自己等级或者不是弹药'AmmoBase'，自身弹药破碎）响应
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator OnCrashed()
        {
            if (!gameObject.activeInHierarchy)
                yield return null;
            gameObject.SetActive(false);
        }
    }
}