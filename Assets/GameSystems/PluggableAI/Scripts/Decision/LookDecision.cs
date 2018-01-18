using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 检测敌人
    /// </summary>
    [CreateAssetMenu(menuName = "GameSystem/PluggableAI/Decisions/Look")]
    public class LookDecision : Decision
    {
        public Color debugColor = Color.green;          // 调试颜色
        public bool fromEyes = true;                    // 是否从眼睛放出检测射线，否则是自身
        [Range(0, 360)]
        public float angle = 90f;                       // 检测前方角度范围
        [Range(1, 50)]
        public int accuracy = 3;                        // 检测精度
        [Range(0, 100)]
        public float distance = 25f;                    // 检测距离
        [Range(0, 1800)]
        public float rotatePerSecond = 90f;             // 每秒旋转角度

        public override bool Decide(StateController controller)
        {
            float subAngle = angle / accuracy;          // 每条射线需要检测的角度范围
            for (int i = 0; i < accuracy; i++)
                if (controller.FindEnemy(Quaternion.Euler(0, -angle / 2 + i * subAngle + Mathf.Repeat(rotatePerSecond * Time.time, subAngle), 0), distance, debugColor, fromEyes))
                    return true;
            return false;
        }
    }
}
