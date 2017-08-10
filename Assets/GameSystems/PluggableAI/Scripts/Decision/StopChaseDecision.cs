using UnityEngine;

namespace GameSystem.AI
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/StopChase")]
    public class StopChaseDecision : Decision
    {
        [Range(0, 100)]
        public float distance = 30f;                //停止追逐的最远距离

        //超过追逐距离,改变下一个巡逻点（用来避免AI卡死）
        public override bool Decide(StateController controller)
        {
            if (controller.chaseTarget == null)
            {
                controller.GetNewNextWayPoint(true);
                return true;
            }
            if ((controller.transform.position - controller.chaseTarget.position).magnitude > distance)
            {
                controller.chaseTarget = null;
                controller.GetNewNextWayPoint(true);
                return true;
            }
            return false;
        }
    }
}