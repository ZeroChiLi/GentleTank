using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public float moveSpeed = 1;                 //移动速度
    public float lookRange = 40f;               //检测敌人距离
    public float lookSphereCastRadius = 1f;     //射出检测射线的射线半径
    public float lookAngle = 90;                //射出射线角度
    public int lookAccurate = 3;                //查找精度配合角度使用，0为只有一条向前的射线，

    public float attackMinRange = 5f;           //攻击最小距离
    public float attackMaxRange = 40f;          //攻击最大距离
    public float attackRate = 1f;               //攻击周期
    public float attackForce = 15f;             //攻击发射力度
    public int attackDamage = 50;               //攻击伤害

    public float searchDuration = 4f;           //查找持续时间
    public float searchingTurnSpeed = 120f;     //查找时旋转速度

    public float chaseMaxRange = 80f;           //最大追逐距离

}