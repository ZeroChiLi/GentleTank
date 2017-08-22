using UnityEngine;
using UnityEditor;
using System.Collections;

namespace GameSystem.AI
{
    [CustomEditor(typeof(AIStats))]
    public class AIStatsEditor : Editor
    {
        private AIStats stats;

        private bool[] showTrigger = new bool[8];

        /// <summary>
        /// 初始化stats
        /// </summary>
        private void OnEnable()
        {
            //全部初始化为true
            ArrayList.Repeat(true, showTrigger.Length).CopyTo(showTrigger);
            stats = target as AIStats;
        }

        /// <summary>
        /// 显示导航控制和攻击控制
        /// </summary>
        public override void OnInspectorGUI()
        {
            NavSetting();
            AttackSetting();
        }

        /// <summary>
        /// 导航控制
        /// </summary>
        private void NavSetting()
        {
            ShowMinMaxBox(1, ref stats.navSpeed, "Move Speed");             //移动速度
            ShowMinMaxBox(2, ref stats.navAngularSpeed, "Angular Speed");   //旋转速度
            ShowMinMaxBox(3, ref stats.navAcceleration, "Acceleration");    //加速度
            ShowMinMaxBox(4, ref stats.navStopDistance, "Stop Distance");   //抵达停止距离
        }

        /// <summary>
        /// 攻击控制
        /// </summary>
        private void AttackSetting()
        {
            ShowMinMaxBox(5, ref stats.attackRate, "Attack Rate");          //攻击周期
            ShowMinMaxBox(6, ref stats.attackForce, "Attack Force");        //攻击力度
            ShowMinMaxBox(7, ref stats.attackDamage, "Attack Damage");      //攻击伤害
        }

        /// <summary>
        /// 显示控制范围组件
        /// </summary>
        /// <param name="order">开关顺序</param>
        /// <param name="minMaxValue">浮动范围数</param>
        /// <param name="title">组件名</param>
        private void ShowMinMaxBox(int order, ref MinMaxValue minMaxValue, string title)
        {
            if (!ShowUp(order, title))
                return;
            EditorGUILayout.MinMaxSlider("Range : ", ref minMaxValue.minValue, ref minMaxValue.maxValue, minMaxValue.minLimit, minMaxValue.maxLimit);
            minMaxValue.minValue = Mathf.Clamp(EditorGUILayout.FloatField("Min Value :", minMaxValue.minValue), minMaxValue.minLimit, minMaxValue.maxValue);
            minMaxValue.maxValue = Mathf.Clamp(EditorGUILayout.FloatField("Max Value :", minMaxValue.maxValue), minMaxValue.minValue, minMaxValue.maxLimit);
        }

        /// <summary>
        /// 显示菜单列表
        /// </summary>
        /// <param name="order">标签序号</param>
        /// <param name="title">标签标题</param>
        /// <returns>返回是否显示</returns>
        private bool ShowUp(int order, string title)
        {
            EditorGUI.indentLevel = 0;
            showTrigger[order] = EditorGUILayout.Foldout(showTrigger[order], title);
            EditorGUI.indentLevel = 1;
            return showTrigger[order];
        }
    }
}
