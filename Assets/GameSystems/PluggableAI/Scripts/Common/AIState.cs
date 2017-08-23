using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// AI状态信息设置
    /// </summary>
    [CreateAssetMenu(menuName = "PluggableAI/AI State")]
    public class AIState : ScriptableObject
    {
        //导航控制
        public MinMaxValue navSpeed = new MinMaxValue(0f, 10f, 3f, 4f);                //移动速度
        public MinMaxValue navAngularSpeed = new MinMaxValue(-360f, 360f, 110f, 130f);  //旋转速度
        public MinMaxValue navAcceleration = new MinMaxValue(0f, 50f, 6f, 10f);         //加速度
        public MinMaxValue navStopDistance = new MinMaxValue(0f, 20f, 6f, 12f);         //抵达停止距离

        //攻击控制
        public MinMaxValue attackRate = new MinMaxValue(0f, 5f, 0.8f, 1.2f);            //攻击周期
        public MinMaxValue attackForce = new MinMaxValue(0f, 30f, 13f, 17f);            //攻击力度
        public MinMaxValue attackDamage = new MinMaxValue(0f, 200f, 45f, 55f);          //攻击伤害
    }
}