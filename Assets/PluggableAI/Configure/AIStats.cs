using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/AI Stats")]
public class AIStats : ScriptableObject
{
    [Header ("NavMeshAgent Setting")]
    public float navSpeed = 3.5f;               //移动速度
    public float navAngularSpeed = 120f;        //旋转速度
    public float navAcceleration = 8f;          //加速度
    public float navStopDistance = 10f;         //抵达停止距离
    public float navRadius = 2f;                //导航物体半径
    public float navHeight = 10f;               //导航物体高度

    [Header("Attack Setting")]
    public float attackRate = 1f;               //攻击周期
    public float attackForce = 15f;             //攻击发射力度
    public int attackDamage = 50;               //攻击伤害
}