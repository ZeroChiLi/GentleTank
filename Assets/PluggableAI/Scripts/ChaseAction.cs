using System;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    //追踪目标
    private void Chase(StateController controller)
    {
        controller.navMeshAgent.destination = controller.chaseTarget.position;
        controller.navMeshAgent.isStopped = false;
        //如果太近，旋转到目标
        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
        {
            Vector3 direction = controller.chaseTarget.position - controller.transform.position ;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            controller.transform.rotation = Quaternion.RotateTowards(controller.transform.rotation, targetRotation, controller.enemyStats.searchingTurnSpeed * Time.deltaTime);
        }
    }

}
