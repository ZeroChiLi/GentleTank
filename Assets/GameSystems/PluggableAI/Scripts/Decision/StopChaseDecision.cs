using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 超过追逐距离,改变下一个巡逻点（用来避免AI卡死）
    /// </summary>
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/StopChase")]
    public class StopChaseDecision : Decision
    {
        [Range(0, 100)]
        public float distance = 30f;                //停止追逐的最远距离

        public override bool Decide(StateController controller)
        {
            if (!controller.instancePrefs.Contains("ChaseTarget"))
            {
                controller.UpdateNextWayPoint(true);
                return true;
            }
            if (!GameMathf.TwoPosInRange(controller.transform.position, ((Transform)controller.instancePrefs["ChaseTarget"]).position, distance))
            {
                controller.instancePrefs.Remove("ChaseTarget");
                controller.UpdateNextWayPoint(true);
                return true;
            }
            return false;
        }
    }
}