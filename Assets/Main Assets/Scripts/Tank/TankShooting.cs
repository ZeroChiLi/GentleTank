using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public enum ShootState
    {
        None,Normal,Charge,Fire
    }

    public ObjectPool shellPool;                // 炮弹池
    public Transform shellSpawn;                // 发射子弹的位置
    public Slider aimSlider;                    // 发射时显示黄色箭头
    public AudioSource shootingAudio;           // 当前射击音效
    public AudioClip chargingClip;              // 射击力度距离变化声音
    public AudioClip fireClip;                  // 射击时声音
    public float fireRate = 0.8f;               // 发射间隔
    public float minLaunchForce = 15f;          // 最小发射力度
    public float maxLaunchForce = 30f;          // 最大发射力度
    public float maxChargeTime = 0.75f;         // 最大发射蓄力时间
    public float maxDamage = 100f;              // 最大伤害
    public bool usingInputButton = true;        // 是否使用标准输入

    private ShootState shootState = ShootState.None;    // 当前射击状态
    private TankInformation tankInfo;           // 玩家信息
    private float currentLaunchForce;           // 当前发射力度
    private float chargeSpeed;                  // 力度变化速度（最小到最大力度 / 最大蓄力时间）
    private float nextFireTime;                 // 下一发最早时间

    private string fireButton = "Fire0";        // 发射子弹按钮是名字

    /// <summary>
    /// 获取坦克信息组件，计算力量变化率
    /// </summary>
    private void Start()
    {
        tankInfo = GetComponent<TankInformation>();
        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
    }

    /// 配置玩家攻击输入属性
    /// </summary>
    /// <param name="id">玩家ID</param>
    public void SetupPlayerInput(int id)
    {
        if (id < 0)
            return;
        fireButton = "Fire" + id;
    }

    /// <summary>
    /// 更新射击力度
    /// </summary>
    private void Update()
    {
        if (usingInputButton)
            StateChangeByInput();
        if (!CanFire())
            return;
        if (!tankInfo.playerAI)         //不是AI才更新
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
            case ShootState.Normal:
                Ready();
                break;
            case ShootState.Charge:
                Charging();
                break;
            case ShootState.Fire:
                Fire();
                break;
        }
    }

    /// <summary>
    /// 是否可以攻击
    /// </summary>
    /// <returns></returns>
    public bool CanFire()
    {
        if (Time.time > nextFireTime)
            return true;
        return false;
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
        currentLaunchForce += chargeSpeed * Time.deltaTime;
        aimSlider.value = currentLaunchForce;
    }

    /// <summary>
    /// 默认发射炮弹
    /// </summary>
    public void Fire()
    {
        Fire(currentLaunchForce, fireRate, maxDamage);
    }

    /// <summary>
    /// 发射炮弹，自定义参数变量
    /// </summary>
    /// <param name="launchForce">发射力度</param>
    /// <param name="fireRate">发射后冷却时间</param>
    /// <param name="fireDamage">伤害值</param>
    public void Fire(float launchForce, float fireRate, float fireDamage)
    {
        if (!CanFire())
            return;

        //获取炮弹，并发射
        GameObject shell = shellPool.GetNextObject(transform: shellSpawn);
        shell.GetComponent<Rigidbody>().velocity = launchForce * shellSpawn.forward;
        shell.GetComponent<Shell>().maxDamage = fireDamage;

        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;

        nextFireTime = Time.time + fireRate;
    }

    /// <summary>
    /// 状态随标准输入改变而改变
    /// </summary>
    private void StateChangeByInput()
    {
        if (Input.GetButtonDown(fireButton))
            shootState = ShootState.Normal;
        else if (Input.GetButton(fireButton))
            shootState = ShootState.Charge;
        else if (Input.GetButtonUp(fireButton))
            shootState = ShootState.Fire;
        else
            shootState = ShootState.None;
    }

    /// <summary>
    /// 外部改变射击状态
    /// </summary>
    /// <param name="shootState"></param>
    public void ChangeState(ShootState shootState)
    {
        this.shootState = shootState;
    }
}