using UnityEngine;
using UnityEngine.UI;

public abstract class HealthManager : MonoBehaviour
{
    public float minHealth = 0f;                        // 血量最小值
    public float maxHealth = 100f;                      // 血量最大值
    public bool canFeelPain = true;                     // 是否感受被攻击
    public float feelPainTime = 4f;                     // 感到伤痛持续时间（被攻击）
    public Slider healthSlider;                         // 血量滑动条
    public Image sliderFillImage;                       // 代表血量的图片
    public Color fullHealthColor = Color.green;         // 满血颜色
    public Color zeroHealthColor = Color.red;           // 没血颜色

    public delegate void DeathEventHandle(HealthManager health, PlayerManager killer);
    public event DeathEventHandle OnDeathEvent;         // 提供外部的死亡时事件

    public float MinHealth { get { return minHealth; } }                // 血量最小值
    public float MaxHealth { get { return maxHealth; } }                // 血量最大值
    public float CurrentHealth                                          // 当前血量，限制在最小和最大值范围内
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, minHealth, maxHealth); }
    }
    public bool IsFeelPain                                              // 是否感受到伤痛
    {
        get { return canFeelPain && isFeelPain; }
        set { isFeelPain = value; }
    }
    public bool IsDead { get { return CurrentHealth <= MinHealth; } }   // 是否死掉了（当前血量小于等于最小值）

    protected float currentHealth;                      // 当前血量
    protected bool isFeelPain = false;                  // 是否感受到伤害
    protected int lastPainFrame;

    private float timeElapsed;                          // 计时器

    protected void Start()
    {
        Init();
    }

    /// <summary>
    /// 默认初始化
    /// </summary>
    public void Init()
    {
        isFeelPain = false;
        healthSlider.minValue = minHealth;
        healthSlider.maxValue = maxHealth;
        CurrentHealth = maxHealth;
        UpdateSlider();
    }

    /// <summary>
    /// 更新滑动条
    /// </summary>
    virtual public void UpdateSlider()
    {
        if (healthSlider != null)
            healthSlider.value = CurrentHealth;
        if (sliderFillImage != null)
            sliderFillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, (CurrentHealth - minHealth) / (maxHealth - minHealth));
    }

    /// <summary>
    /// 通过Time.delta更新伤痛感受时间
    /// </summary>
    protected void UpdateFeelPainByDeltaTime()
    {
        if (!IsFeelPain)
            return;
        timeElapsed -= Time.deltaTime;
        if (timeElapsed <= 0)
            isFeelPain = false;
    }

    /// <summary>
    /// 设置血量变化
    /// </summary>
    /// <param name="amount">变化值</param>
    /// <param name="from">哪个玩家要改的（谁打的）</param>
    public void SetHealthAmount(float amount, PlayerManager from = null)
    {
        if (amount == 0 || lastPainFrame == Time.frameCount)        // 每一帧最多只接收一次伤害
            return;
        if (amount < 0)
        {
            isFeelPain = true;
            timeElapsed = feelPainTime;
            lastPainFrame = Time.frameCount;
        }
        CurrentHealth += amount;
        UpdateSlider();
        OnHealthChanged(from);
        if (IsDead)
        {
            OnDead(from);
            if (OnDeathEvent != null)
                OnDeathEvent.Invoke(this,from);
        }
    }

    /// <summary>
    /// 血量变化时调用
    /// </summary>
    abstract protected void OnHealthChanged(PlayerManager from = null);

    /// <summary>
    /// 玩家死掉时调用
    /// </summary>
    abstract protected void OnDead(PlayerManager from = null);

}
