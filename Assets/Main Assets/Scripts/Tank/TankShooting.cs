using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int playerNumber = 1;                // 玩家编号
    public ObjectPool shellPool;                // 炮弹池
    public Transform shellSpawn;                // 发射子弹的位置
    public Slider aimSlider;                    // 发射时显示黄色箭头
    public AudioSource shootingAudio;           // 当前射击音效
    public AudioClip chargingClip;              // 射击力度距离变化声音
    public AudioClip fireClip;                  // 射击时声音
    public float fireInterval = 0.8f;           // 发射间隔
    public float minLaunchForce = 15f;          // 最小发射力度
    public float maxLaunchForce = 30f;          // 最大发射力度
    public float maxChargeTime = 0.75f;         // 最大发射蓄力时间

    private bool isAI;                          // 是否是AI
    private string fireButton;                  // 发射子弹按钮是名字
    private float currentLaunchForce;           // 当前发射力度
    private float chargeSpeed;                  // 力度变化速度（最小到最大力度 / 最大蓄力时间）
    private bool fired = true;                  // 是否发射了
    private float nextFireTime;                 // 下一发最早时间

    private void Start()
    {
        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
    }

    private void Update()
    {
        if (!CanFire())
            return;
        if (!isAI)
            CheckForceToFire();
    }

    //根据按键时长来发射炮弹
    private void CheckForceToFire()
    {
        // 一直按着到超过最大力度，自动发射
        if (currentLaunchForce >= maxLaunchForce && !fired)
        {
            currentLaunchForce = maxLaunchForce;
            Fire(currentLaunchForce, 1);
        }
        // 如果开始按下攻击键，开始射击力度，开始射击变化音效
        else if (Input.GetButtonDown(fireButton))
        {
            fired = false;
            currentLaunchForce = minLaunchForce;

            shootingAudio.clip = chargingClip;
            shootingAudio.Play();

            aimSlider.value = minLaunchForce;
        }
        // 按着攻击键中，增强射击力度
        else if (Input.GetButton(fireButton) && !fired)
        {
            currentLaunchForce += chargeSpeed * Time.deltaTime;
            aimSlider.value = currentLaunchForce;
        }
        // 松开攻击键，Fire In The Hole!!
        else if (Input.GetButtonUp(fireButton) && !fired)
        {
            Fire(currentLaunchForce, 1);
        }
    }

    //设置编号
    public void SetPlayerNumber(int number,bool isAI = false)
    {
        //在Player Setting中设置
        playerNumber = number;
        fireButton = "Fire" + playerNumber;
        this.isAI = isAI;
    }

    //发射炮弹
    public void Fire(float launchForce, float fireRate)
    {
        if (!CanFire())
            return;

        //获取炮弹，并发射
        shellPool.GetNextObjectActive(shellSpawn).GetComponent<Rigidbody>().velocity = currentLaunchForce * shellSpawn.forward;

        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;

        nextFireTime = Time.time + fireInterval;
    }

    //是否可以攻击
    private bool CanFire()
    {
        if (Time.time > nextFireTime)
            return true;
        return false;
    }

}
