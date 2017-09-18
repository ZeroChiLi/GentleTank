using UnityEngine;

[System.Serializable]
public class AmmoProperty
{
    public float damage = 20f;          // 伤害值
    public bool isIndestructible;       // 是否无法摧毁的（不受耐久影响）
    public int durability = 50;         // 子弹耐久度（碰到别的子弹，会根据别子弹的耐久值减去自己耐久值）
}

[CreateAssetMenu(menuName = "Module/AmmoModule/Default")]
public class AmmoModule : ModuleBase
{
    public AmmoProperty ammoProperty;
}
