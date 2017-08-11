using UnityEngine;
using UnityEngine.UI;

namespace Item.Tank
{
    public class TankHealth : MonoBehaviour
    {
        public float startingHealth = 100f;                 // 起始血量
        public Slider slider;                               // 血量滑动条
        public Image fillImage;                             // 代表血量的图片
        public Color fullHealthColor = Color.green;         // 满血颜色
        public Color zeroHealthColor = Color.red;           // 没血颜色
        public ObjectPool tankExplosionPool;                // 坦克爆炸特效池
        public ObjectPool tankBustedPool;                   // 坦克残骸池
        public float feelPainTime = 4f;                     // 感到伤痛持续时间（被攻击）

        private PlayerManager playerManager;                // 玩家信息
        private float currentHealth;                        // 当前血量
        private bool isFeelPain = false;                    // 是否感受到伤害
        private float timeElapsed;                          // 计时器
        private bool isDead;                                // 是否死掉

        public float CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = Mathf.Clamp(value, 0, startingHealth); }
        }
        public bool IsFeelPain { get { return isFeelPain; } }


        /// <summary>
        /// 获取玩家信息
        /// </summary>
        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        /// <summary>
        /// 激活时初始化
        /// </summary>
        private void OnEnable()
        {
            slider.maxValue = startingHealth;
            CurrentHealth = startingHealth;
            isFeelPain = false;
            isDead = false;
            SetHealthUI();
        }

        /// <summary>
        /// 关闭时重置
        /// </summary>
        private void OnDisable()
        {
            CurrentHealth = 0;
            SetHealthUI();
        }

        /// <summary>
        /// 更新受伤感受时间
        /// </summary>
        private void Update()
        {
            if (isFeelPain)
            {
                timeElapsed -= Time.deltaTime;
                if (timeElapsed <= 0)
                    isFeelPain = false;
            }
        }

        /// <summary>
        /// 受伤害
        /// </summary>
        /// <param name="amount">伤害值</param>
        public void TakeDamage(float amount)
        {
            isFeelPain = true;
            timeElapsed = feelPainTime;
            CurrentHealth -= amount;
            SetHealthUI();
            if (CurrentHealth <= 0f && !isDead)
                OnDeath();
        }

        /// <summary>
        /// 治愈加血
        /// </summary>
        /// <param name="amount">加血量</param>
        public void GainHeal(float amount)
        {
            CurrentHealth += amount;
            SetHealthUI();
        }

        /// <summary>
        /// 更新血条UI
        /// </summary>
        private void SetHealthUI()
        {
            slider.value = CurrentHealth;
            fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, CurrentHealth / startingHealth);
        }

        /// <summary>
        /// 死亡事件
        /// </summary>
        private void OnDeath()
        {
            isDead = true;
            tankExplosionPool.GetNextObject(transform: gameObject.transform);   // 爆炸特效
            tankBustedPool.GetNextObject().GetComponent<BustedTankMananger>().SetupBustedTank(transform, playerManager.RepresentColor);                 // 死后残骸
            gameObject.SetActive(false);
        }
    }
}