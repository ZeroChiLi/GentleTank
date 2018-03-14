using CrossPlatformInput;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Item.Tank
{
    public enum AttackState
    {
        None, Ready, Charge, Luanch
    }

    public abstract class TankAttack : AttackManager
    {
        public AudioClip chargingClip;              // 射击力度距离变化声音
        public AudioClip fireClip;                  // 射击时声音
        public Slider forceSlider;                  // 发射时显示黄色箭头
        public float minLaunchForce = 15f;          // 最小发射力度
        public float maxLaunchForce = 30f;          // 最大发射力度
        public float maxChargeTime = 0.75f;         // 最大发射蓄力时间
        public float damage = 50;                   // 伤害值
        public bool usingInputButton = true;        // 是否使用标准输入

        public float ChargeRate { get { return chargeRate; } }
        public float ForceSliderLength { get { return forceSlider.maxValue - forceSlider.minValue; } }

        protected PlayerManager playerManager;         // 玩家信息
        protected AudioSource attackAudio;             // 当前射击音效
        protected AttackState shootState = AttackState.None;    // 当前射击状态
        protected float chargeRate;                   // 力度变化速度（最小到最大力度 / 最大蓄力时间）

        /// <summary>
        /// 获取基本组件（PlayerManager、AudioSource）
        /// </summary>
        protected void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            attackAudio = GetComponent<AudioSource>();
        }

        /// <summary>
        /// 激活时重置信息
        /// </summary>
        protected void OnEnable()
        {
            ResetSliderValue(minLaunchForce, maxLaunchForce, maxChargeTime);
        }

        /// <summary>
        /// 失活时重置信息
        /// </summary>
        protected void OnDisable()
        {
            ResetSliderValue(minLaunchForce, maxLaunchForce, maxChargeTime);
        }

        /// <summary>
        /// 在激活和失活时重置信息
        /// </summary>
        public void ResetSliderValue(float min, float max, float time)
        {
            if (forceSlider)
            {
                forceSlider.minValue = minLaunchForce = min;
                forceSlider.maxValue = maxLaunchForce = max;
                forceSlider.value = min;
            }
            maxChargeTime = time;
            chargeRate = (max - min) / time;
        }

        /// <summary>
        /// 更新射击力度
        /// </summary>
        protected void Update()
        {
            if (playerManager == null || playerManager.IsAI || !IsTimeUp)
                return;
            if (usingInputButton)
                StateChangeByInput();
            // 优先选择键盘输入，若无，则再判断虚拟按钮输入
            if (shootState == AttackState.None && AllPlayerManager.Instance && playerManager == AllPlayerManager.Instance.MyPlayer)
                StateChangeByChargeButton();
            if (!playerManager.IsAI)         //不是AI才更新
                ChargeToFire();
        }

        /// <summary>
        /// 根据按键时长来发射炮弹
        /// </summary>
        private void ChargeToFire()
        {
            // 一直按着到超过最大力度，自动发射
            if (forceSlider.value > maxLaunchForce)
                forceSlider.value = maxLaunchForce;
            switch (shootState)
            {
                case AttackState.Ready:
                    Ready();
                    break;
                case AttackState.Charge:
                    Charging();
                    break;
                case AttackState.Luanch:
                    Attack();
                    break;
            }
        }

        /// <summary>
        /// 如果开始按下攻击键，开始射击蓄力，开始射击变化音效
        /// </summary>
        public void Ready()
        {
            forceSlider.value = minLaunchForce;
            forceSlider.value = minLaunchForce;

            if (attackAudio != null && chargingClip != null)
            {
                attackAudio.clip = chargingClip;
                attackAudio.Play();
            }
        }

        /// <summary>
        /// 蓄力，变化通过Time.deltaTime
        /// </summary>
        public void Charging()
        {
            forceSlider.value += chargeRate * Time.deltaTime;
            forceSlider.value = forceSlider.value;
        }

        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override bool Attack(params object[] values)
        {
            if (!base.Attack(values))
                return false;

            if (attackAudio != null && fireClip != null)
            {
                attackAudio.clip = fireClip;
                attackAudio.Play();
            }

            forceSlider.value = minLaunchForce;
            forceSlider.value = minLaunchForce;
            return true;
        }

        /// <summary>
        /// 状态随标准输入改变而改变
        /// </summary>
        protected void StateChangeByInput()
        {
            if (Input.GetButtonDown(shortcutName))
                shootState = AttackState.Ready;
            else if (Input.GetButton(shortcutName))
                shootState = AttackState.Charge;
            else if (Input.GetButtonUp(shortcutName))
                shootState = AttackState.Luanch;
            else
                shootState = AttackState.None;
        }

        /// <summary>
        /// 通过虚拟按钮控制
        /// </summary>
        public void StateChangeByChargeButton()
        {
            switch (VirtualInput.GetButtonState("Attack"))
            {
                case ButtonState.Down:
                    shootState = AttackState.Ready;
                    break;
                case ButtonState.Pressed:
                    shootState = AttackState.Charge;
                    break;
                case ButtonState.Up:
                    shootState = AttackState.Luanch;
                    break;
                default:
                    shootState = AttackState.None;
                    break;
            }
        }
    }
}