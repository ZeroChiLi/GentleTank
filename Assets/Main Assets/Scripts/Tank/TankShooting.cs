using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int playerNumber = 1;                // 玩家编号
    public Rigidbody shellRigidbody;            // 子弹刚体
    public Transform fireTransform;             // 发射子弹的位置
    public Slider aimSlider;                    // 发射时显示黄色箭头
    public AudioSource shootingAudio;           // 当前射击音效
    public AudioClip chargingClip;              // 射击力度距离变化声音
    public AudioClip fireClip;                  // 射击时声音
    public float fireInterval = 0.8f;           // 发射间隔
    public float minLaunchForce = 15f;          // 最小发射力度
    public float maxLaunchForce = 30f;          // 最大发射力度
    public float maxChargeTime = 0.75f;         // 最大发射蓄力时间


    private string fireButton;                  // 发射子弹按钮是名字
    private float currentLaunchForce;           // 当前发射力度
    private float chargeSpeed;                  // 力度变化速度（最小到最大力度 / 最大蓄力时间）
    private bool fired = true;                  // 是否发射了
    private float nextFireTime;                 // 下一发最早时间

    private void OnEnable()
    {

        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
    }

    private void Start()
    {
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
    }

    private void Update()
    {
        if (!CanFire())
            return;

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
    public void SetPlayerNumber(int number)
    {
        //在Player Setting中设置
        playerNumber = number;
        fireButton = "Fire" + playerNumber;
    }

    //发射子弹
    public void Fire(float launchForce, float fireRate)
    {
        if (!CanFire())
            return;

        nextFireTime = Time.time + fireInterval;

        //创建子弹并获取刚体
        Rigidbody shellInstance =
            Instantiate(shellRigidbody, fireTransform.position, fireTransform.rotation) as Rigidbody;

        shellInstance.velocity = currentLaunchForce * fireTransform.forward;

        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
    }

    //是否可以攻击
    private bool CanFire()
    {
        if (Time.time > nextFireTime)
            return true;
        return false;
    }

}
