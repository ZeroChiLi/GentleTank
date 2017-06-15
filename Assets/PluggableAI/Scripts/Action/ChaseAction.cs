using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    public override void Act(StateController controller) { Chase(controller); }

    //追踪目标
    private void Chase(StateController controller)
    {
        if (controller.chaseTarget == null)
            return;

        controller.navMeshAgent.destination = controller.chaseTarget.position;
        controller.navMeshAgent.isStopped = false;
    }

}
