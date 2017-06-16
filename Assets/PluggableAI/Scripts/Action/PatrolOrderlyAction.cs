using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/OrderlyPatrol")]
public class PatrolOrderlyAction : Action
{
    public override void Act(StateController controller)
    {
        controller.navMeshAgent.destination = controller.NextWayPoint.position;
        controller.navMeshAgent.isStopped = false;  

        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
            controller.GetNewNextWayPoint(false);
    }

}
