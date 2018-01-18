using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 追踪目标
    /// </summary>
    [CreateAssetMenu(menuName = "GameSystem/PluggableAI/Actions/Chase")]
    public class ChaseAction : Action
    {
        public override void Act(StateController controller)
        {
            if (!controller.instancePrefs.Contains(CommonCode.ChaseEnemy))
                return;

            controller.navMeshAgent.destination = ((Transform)controller.instancePrefs[CommonCode.ChaseEnemy]).position;
            controller.navMeshAgent.isStopped = false;
        }
    }
}
