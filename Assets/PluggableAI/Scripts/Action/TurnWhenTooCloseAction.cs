using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/TurnWhenTooClose")]
public class TurnWhenTooCloseAction : Action
{
    //如果当前距离在停止距离内，旋转到目标
    public override void Act(StateController controller)
    {
        if (controller.chaseTarget == null)
            return;
        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
        {
            Vector3 direction = controller.chaseTarget.position - controller.transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            controller.rigidbodySelf.rotation = Quaternion.RotateTowards(controller.transform.rotation, targetRotation, controller.navMeshAgent.angularSpeed * Time.deltaTime);
        }
    }

}
