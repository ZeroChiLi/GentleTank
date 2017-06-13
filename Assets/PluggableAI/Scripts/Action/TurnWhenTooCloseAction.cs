using System;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/TurnWhenTooClose")]
public class TurnWhenTooCloseAction : Action
{
    public override void Act(StateController controller)
    {
        TurnWhenTooClose(controller);
    }

    //如果当前距离在停止距离内，旋转到目标
    private void TurnWhenTooClose(StateController controller)
    {
        if (controller.chaseTarget == null)
            return;
        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
        {
            Vector3 direction = controller.chaseTarget.position - controller.transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            controller.aiRigidbody.rotation = Quaternion.RotateTowards(controller.transform.rotation, targetRotation, controller.enemyStats.searchingTurnSpeed * Time.deltaTime);
        }
    }

}
