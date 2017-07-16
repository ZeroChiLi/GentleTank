using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float startingHealth = 100f;                 // 起始血量
    public Slider slider;                               // 血量滑动条
    public Image fillImage;                             // 代表血量的图片
    public Color fullHealthColor = Color.green;         // 满血颜色
    public Color zeroHealthColor = Color.red;           // 没血颜色
    public ObjectPool tankExplosionPool;                // 坦克爆炸特效池

    [HideInInspector]
    public bool getHurt = false;      // 是否受伤

    private float currentHealth;                        // 当前血量
    private bool dead;                                  // 是否死掉

    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

    private void OnEnable()
    {
        slider.maxValue = startingHealth;
        CurrentHealth = startingHealth;
        getHurt = false;
        dead = false;
        SetHealthUI();
    }

    // 受伤害
    public void TakeDamage(float amount)
    {
        getHurt = true;
        CurrentHealth -= amount;
        SetHealthUI();
        if (CurrentHealth <= 0f && !dead)
            OnDeath();
    }

    // 加血
    public void GainHeal(float amount)
    {
        CurrentHealth += amount;
        SetHealthUI();
    }

    // 修改血条长度，颜色
    private void SetHealthUI()
    {
        slider.value = CurrentHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, CurrentHealth / startingHealth);
    }

    // 死掉了
    private void OnDeath()
    {
        dead = true;

        //获取爆炸特效，并显示之
        tankExplosionPool.GetNextObject(gameObject.transform);

        gameObject.SetActive(false);
    }
}
