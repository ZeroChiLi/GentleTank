using UnityEngine;
using UnityEngine.UI;

public class OnlineTankHealth : Photon.MonoBehaviour
{
    public ObjectPool tankExplosionPool;                // 坦克爆炸特效池
    public float startingHealth = 100f;                 // 起始血量
    public Slider slider;                               // 血量滑动条
    public Image fillImage;                             // 代表血量的图片
    public Color fullHealthColor = Color.green;         // 满血颜色
    public Color zeroHealthColor = Color.red;           // 没血颜色

    [HideInInspector]
    public bool getHurt = false;      // 是否受伤

    private float currentHealth;                        // 当前血量
    private bool isDead;                                // 是否死掉

    /// <summary>
    /// 当前血量
    /// </summary>
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
        isDead = false;
        UpdateHealthUI();
    }

    /// <summary>
    /// 失效时清空信息
    /// </summary>
    private void OnDisable()
    {
        CurrentHealth = 0;
        UpdateHealthUI();
    }

    /// <summary>
    /// 受伤害
    /// </summary>
    /// <param name="amount">伤害值</param>
    /// <returns>返回当前血量</returns>
    public float TakeDamage(float amount)
    {
        getHurt = true;
        CurrentHealth -= amount;
        UpdateHealthUI();
        if (CurrentHealth <= 0f && !isDead)
            OnDeath();
        return CurrentHealth;
    }

    /// <summary>
    /// 增加血量
    /// </summary>
    /// <param name="amount">增加量</param>
    /// <returns>当前血量</returns>
    public float GainHeal(float amount)
    {
        CurrentHealth += amount;
        UpdateHealthUI();
        return CurrentHealth;
    }

    /// <summary>
    /// 更新血量UI
    /// </summary>
    private void UpdateHealthUI()
    {
        slider.value = CurrentHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, CurrentHealth / startingHealth);
    }

    /// <summary>
    /// 死掉时调用
    /// </summary>
    private void OnDeath()
    {
        isDead = true;

        //获取爆炸特效，并显示之
        tankExplosionPool.GetNextObject(transform: gameObject.transform);

        gameObject.SetActive(false);
    }

    /// <summary>
    /// 同步HP
    /// </summary>
    /// <param name="stream">信息流</param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(currentHealth);
            stream.SendNext(isDead);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
            UpdateHealthUI();
            if ((bool)stream.ReceiveNext())
                OnDeath();
        }
    }
}
