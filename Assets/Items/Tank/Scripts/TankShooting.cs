using Item.Shell;
using UnityEngine;
using UnityEngine.UI;
using CrossPlatformInput;

namespace Item.Tank
{
    public class TankShooting : AttackManager
    {
        public enum ShootState
        {
            None, Ready, Charge, Fire
        }

        public ObjectPool shellPool;                // 炮弹池
        public Transform shellSpawn;                // 发射子弹的位置
        public Slider aimSlider;                    // 发射时显示黄色箭头
        public AudioSource shootingAudio;           // 当前射击音效
        public AudioClip chargingClip;              // 射击力度距离变化声音
        public AudioClip fireClip;                  // 射击时声音
        public float minLaunchForce = 15f;          // 最小发射力度
        public float maxLaunchForce = 30f;          // 最大发射力度
        public float maxChargeTime = 0.75f;         // 最大发射蓄力时间
        public float maxDamage = 100f;              // 最大伤害
        public bool usingInputButton = true;        // 是否使用标准输入

        public float ChargeRate { get { return chargeRate; } }

        private ShootState shootState = ShootState.None;    // 当前射击状态
        private PlayerManager playerManager;        // 玩家信息
        private float currentLaunchForce;           // 当前发射力度
        private float chargeRate;                   // 力度变化速度（最小到最大力度 / 最大蓄力时间）

        /// <summary>
        /// 获取坦克信息组件，计算力量变化率
        /// </summary>
        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            currentLaunchForce = minLaunchForce;
            aimSlider.value = minLaunchForce;
            chargeRate = (maxLaunchForce - minLaunchForce) / maxChargeTime;
        }

        /// <summary>
        /// 更新射击力度
        /// </summary>
        private void Update()
        {
            if (playerManager.IsAI || IsCoolDown)
                return;
            if (usingInputButton)
                StateChangeByInput();
            if (shootState == ShootState.None && playerManager.IsMine)  // 优先选择键盘输入，若无，则再判断虚拟按钮输入
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
            if (currentLaunchForce > maxLaunchForce)
                currentLaunchForce = maxLaunchForce;
            switch (shootState)
            {
                case ShootState.Ready:
                    Ready();
                    break;
                case ShootState.Charge:
                    Charging();
                    break;
                case ShootState.Fire:
                    Attack();
                    break;
            }
        }

        /// <summary>
        /// 如果开始按下攻击键，开始射击蓄力，开始射击变化音效
        /// </summary>
        public void Ready()
        {
            currentLaunchForce = minLaunchForce;
            aimSlider.value = minLaunchForce;

            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }

        /// <summary>
        /// 蓄力，变化通过Time.deltaTime
        /// </summary>
        public void Charging()
        {
            currentLaunchForce += chargeRate * Time.deltaTime;
            aimSlider.value = currentLaunchForce;
        }

        /// <summary>
        /// 攻击实际效果
        /// </summary>
        protected override void OnAttack(params object[] values)
        {
            if (values == null || values.Length == 0)
                Fire(currentLaunchForce, coolDownTime, maxDamage);
            else if (values.Length == 3)
                Fire((float)values[0], (float)values[1], (float)values[2]);
        }

        /// <summary>
        /// 发射炮弹，自定义参数变量
        /// </summary>
        /// <param name="launchForce">发射力度</param>
        /// <param name="coolDownTime">发射后冷却时间</param>
        /// <param name="fireDamage">伤害值</param>
        private void Fire(float launchForce, float coolDownTime, float fireDamage)
        {
            //获取炮弹，并发射
            GameObject shell = shellPool.GetNextObject(transform: shellSpawn);
            shell.GetComponent<Rigidbody>().velocity = launchForce * shellSpawn.forward;
            shell.GetComponent<ShellManager>().maxDamage = fireDamage;

            shootingAudio.clip = fireClip;
            shootingAudio.Play();

            currentLaunchForce = minLaunchForce;
            aimSlider.value = minLaunchForce;

            this.coolDownTime = coolDownTime;
        }

        /// <summary>
        /// 状态随标准输入改变而改变
        /// </summary>
        private void StateChangeByInput()
        {
            if (Input.GetButtonDown(shortcutName))
                shootState = ShootState.Ready;
            else if (Input.GetButton(shortcutName))
                shootState = ShootState.Charge;
            else if (Input.GetButtonUp(shortcutName))
                shootState = ShootState.Fire;
            else
                shootState = ShootState.None;
        }

        /// <summary>
        /// 通过虚拟按钮控制
        /// </summary>
        public void StateChangeByChargeButton()
        {
            switch (VirtualInput.GetButtonState("TankShooting"))
            {
                case ButtonState.Down:
                    shootState = ShootState.Ready;
                    break;
                case ButtonState.Pressed:
                    shootState = ShootState.Charge;
                    break;
                case ButtonState.Up:
                    shootState = ShootState.Fire;
                    break;
                default:
                    shootState = ShootState.None;
                    break;
            }
        }

    }
}