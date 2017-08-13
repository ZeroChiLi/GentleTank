using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 检测敌人
    /// </summary>
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
    public class LookDecision : Decision
    {
        public Color debugColor = Color.green;          //调试颜色
        [Range(0, 360)]
        public float angle = 90f;                       //检测前方角度范围
        [Range(1, 50)]
        public int accuracy = 3;                        //检测精度
        [Range(0, 100)]
        public float distance = 25f;                    //检测距离
        [Range(0, 1800)]
        public float rotatePerSecond = 90f;             //每秒旋转角度

        public override bool Decide(StateController controller)
        {
            float subAngle = angle / accuracy;          //每条射线需要检测的角度范围
            for (int i = 0; i < accuracy; i++)
                if (LookAround(controller, Quaternion.Euler(0, -angle / 2 + i * subAngle + Mathf.Repeat(rotatePerSecond * Time.time, subAngle), 0), distance, debugColor))
                    return true;
            return false;
        }

        /// <summary>
        /// 查找敌人，并存到controller.instancePrefs 的 "ChaseTarget" 中，值为Transform。
        /// </summary>
        /// <param name="controller">检测者</param>
        /// <param name="eulerAnger">检测角度范围</param>
        /// <param name="distance">检测半径</param>
        /// <param name="DebugColor">调试颜色</param>
        /// <returns>是否检测到敌人</returns>
        static public bool LookAround(StateController controller, Quaternion eulerAnger, float distance, Color DebugColor)
        {
            Debug.DrawRay(controller.eyes.position, eulerAnger * controller.eyes.forward.normalized * distance, DebugColor);

            RaycastHit hit;
            if (Physics.Raycast(controller.eyes.position, eulerAnger * controller.eyes.forward, out hit, distance, LayerMask.GetMask("Level")) && hit.collider.CompareTag("Player") && !controller.IsTeamMate(hit.collider) && !controller.IsMyself(hit.collider))
            {
                controller.instancePrefs.AddOrModifyValue("ChaseTarget", hit.transform);
                return true;
            }
            return false;
        }
    }
}
