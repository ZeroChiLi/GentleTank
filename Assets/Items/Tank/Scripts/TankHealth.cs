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

        [HideInInspector]
        public bool getHurt = false;                        // 是否受伤

        private float currentHealth;                        // 当前血量
        private bool dead;                                  // 是否死掉

        public float CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = Mathf.Clamp(value, 0, startingHealth); }
        }

        /// <summary>
        /// 激活时初始化
        /// </summary>
        private void OnEnable()
        {
            slider.maxValue = startingHealth;
            CurrentHealth = startingHealth;
            getHurt = false;
            dead = false;
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
        /// 受伤害
        /// </summary>
        /// <param name="amount">伤害值</param>
        public void TakeDamage(float amount)
        {
            getHurt = true;
            CurrentHealth -= amount;
            SetHealthUI();
            if (CurrentHealth <= 0f && !dead)
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
            dead = true;

            //获取爆炸特效，并显示之
            tankExplosionPool.GetNextObject(transform: gameObject.transform);

            gameObject.SetActive(false);
        }
    }
}