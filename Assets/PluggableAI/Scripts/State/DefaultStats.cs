using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/EnemyStats")]
public class DefaultStats : ScriptableObject
{
    public float navSpeed = 3.5f;               //移动速度
    public float navAngularSpeed = 120f;        //旋转速度
    public float navAcceleration = 8f;          //加速度
    public float navStopDistance = 10f;         //抵达停止距离
    public float navRadius = 2f;                //导航物体半径
    public float navHeight = 10f;               //导航物体高度

    public float lookRange = 40f;               //检测敌人距离
    public float lookAngle = 90f;               //射出射线角度
    public int lookAccurate = 3;                //查找精度配合角度使用，0为只有一条向前的射线，

    public float attackMinRange = 5f;           //攻击最小距离
    public float attackMaxRange = 40f;          //攻击最大距离
    public float attackAngle = 45f;             //同检测lookAngle
    public int attackAccurate = 1;              //同检测lookAccurate
    public float attackRate = 1f;               //攻击周期
    public float attackForce = 15f;             //攻击发射力度
    public int attackDamage = 50;               //攻击伤害

    public float searchDuration = 4f;           //查找持续时间

    public float chaseMaxRange = 80f;           //最大追逐距离

}