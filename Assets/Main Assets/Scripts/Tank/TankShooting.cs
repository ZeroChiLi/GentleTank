using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankShooting : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // 玩家编号
        public Rigidbody m_Shell;                   // 子弹刚体
        public Transform m_FireTransform;           // 发射子弹的位置
        public Slider m_AimSlider;                  // 发射时显示黄色箭头
        public AudioSource m_ShootingAudio;         // 当前射击音效
        public AudioClip m_ChargingClip;            // 射击力度距离变化声音
        public AudioClip m_FireClip;                // 射击时声音
        public float fireInterval = 0.8f;             // 发射间隔
        public float m_MinLaunchForce = 15f;        // 最小发射力度
        public float m_MaxLaunchForce = 30f;        // 最大发射力度
        public float m_MaxChargeTime = 0.75f;       // 最大发射蓄力时间


        private string m_FireButton;                // 发射子弹按钮是名字
        private float m_CurrentLaunchForce;         // 当前发射力度
        private float m_ChargeSpeed;                // 力度变化速度（最小到最大力度 / 最大蓄力时间）
        private bool m_Fired = true;                // 是否发射了
        private float nextFireTime;                 // 下一发最早时间

        private void OnEnable()
        {
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_AimSlider.value = m_MinLaunchForce;
        }

        private void Start()
        {
            //在Player Setting中设置
            m_FireButton = "Fire" + m_PlayerNumber;

            m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        }

        private void Update()
        {
            if (!CanFire())
                return;

            // 一直按着到超过最大力度，自动发射
            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            {
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire(m_CurrentLaunchForce, 1);
            }
            // 如果开始按下攻击键，开始射击力度，开始射击变化音效
            else if (Input.GetButtonDown(m_FireButton))
            {
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;

                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play();

                m_AimSlider.value = m_MinLaunchForce;
            }
            // 按着攻击键中，增强射击力度
            else if (Input.GetButton(m_FireButton) && !m_Fired)
            {
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                m_AimSlider.value = m_CurrentLaunchForce;
            }
            // 松开攻击键，Fire In The Hole!!
            else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
            {
                Fire(m_CurrentLaunchForce, 1);
                m_AimSlider.value = m_MinLaunchForce;
            }
        }

        //发射子弹
        public void Fire(float launchForce, float fireRate)
        {
            if (!CanFire())
                return;

            nextFireTime = Time.time + fireInterval;

            //创建子弹并获取刚体
            Rigidbody shellInstance =
                Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();

            m_CurrentLaunchForce = m_MinLaunchForce;
        }

        //是否可以攻击
        private bool CanFire()
        {
            if (Time.time > nextFireTime)
                return true;
            return false;
        }

    }
}