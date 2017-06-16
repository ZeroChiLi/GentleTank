using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    //追踪目标
    public override void Act(StateController controller)
    {
        if (controller.chaseTarget == null)
            return;

        controller.navMeshAgent.destination = controller.chaseTarget.position;
        controller.navMeshAgent.isStopped = false;
    }

}
