using System;
using UnityEngine;

[System.Serializable]
public class AmmoProperty : ModuleProperty
{
    public float damage = 20f;          // 伤害值
    public bool isIndestructible;       // 是否无法摧毁的（不受耐久影响）
    public int durability = 50;         // 子弹耐久度（碰到别的子弹，会根据别子弹的耐久值减去自己耐久值）

    public override string GetPropertiesString()
    {
        return string.Format("描述：{0}\n伤害：{1}\n耐久：{2}", description, damage, isIndestructible ? "INF" : durability.ToString());
    }
}

[CreateAssetMenu(menuName = "Module/AmmoModule/Default")]
public class AmmoModule : ModuleBase
{
    public AmmoProperty property;

    public override string GetProperties()
    {
        return property.ToString();
    }
}
