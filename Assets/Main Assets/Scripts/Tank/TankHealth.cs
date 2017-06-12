using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankHealth : MonoBehaviour
    {
        public float startingHealth = 100f;               // 起始血量
        public Slider slider;                             // 血量滑动条
        public Image fillImage;                           // 代表血量的图片
        public Color fullHealthColor = Color.green;       // 满血颜色
        public Color zeroHealthColor = Color.red;         // 没血颜色
        public GameObject explosionPrefab;                // 爆炸特性

        private AudioSource explosionAudio;               // 爆炸声音
        private ParticleSystem explosionParticles;        // 玩家爆炸粒子
        private float currentHealth;                      // 当前血量
        private bool dead;                                // 是否死掉

        private void Awake()
        {
            explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
            explosionParticles.gameObject.SetActive(false);
            explosionAudio = explosionParticles.GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            currentHealth = startingHealth;
            dead = false;

            SetHealthUI();
        }

        // 受伤害
        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            SetHealthUI();
            if (currentHealth <= 0f && !dead)
                OnDeath();
        }

        // 修改血条长度，颜色
        private void SetHealthUI()
        {
            slider.value = currentHealth;

            fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / startingHealth);
        }

        // 死掉了
        private void OnDeath()
        {
            dead = true;

            explosionParticles.transform.position = transform.position;
            explosionParticles.gameObject.SetActive(true);
            explosionParticles.Play();

            explosionAudio.Play();

            gameObject.SetActive(false);
        }
    }
}