using UnityEngine;

public abstract class AttackManager : MonoBehaviour 
{
    public string shortcutName = "Fire0";                           // 攻击键名称
    public float coolDownTime = 0.8f;                               // 冷却时间

    public bool IsCoolDown { get { return isCoolDown; } }           // 是否正在冷却

    protected bool isCoolDown;                                      // 是否正在冷却

    private float timeElapsed;                                      // 计时器

    /// <summary>
    /// 设置快捷键名称
    /// </summary>
    /// <param name="name">名称</param>
    public void SetShortcutName(string name)
    {
        shortcutName = name;
    }

    /// <summary>
    /// 通过Time.deltaTime更新冷却时间
    /// </summary>
    protected void UpdateCoolDownByDeltaTime()
    {
        if (!isCoolDown)
            return;
        timeElapsed -= Time.deltaTime;
        if (timeElapsed <= 0)
            isCoolDown = false;
    }

    /// <summary>
    /// 释放攻击，响应OnAttack事件
    /// </summary>
    /// <param name="values">参数列表</param>
    public void Attack(params object[] values)
    {
        if (isCoolDown)
            return;
        timeElapsed = coolDownTime;
        isCoolDown = true;
        OnAttack(values);
    }

    /// <summary>
    /// 真正的攻击效果
    /// </summary>
    /// <param name="values">参数列表</param>
    abstract protected void OnAttack(params object[] values);

}
