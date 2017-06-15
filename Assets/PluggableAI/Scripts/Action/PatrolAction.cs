using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(StateController controller) { Patrol(controller); }

    private void Patrol(StateController controller)
    {
        controller.navMeshAgent.destination = controller.NextWayPouint.position;
        controller.navMeshAgent.isStopped = false;  //保持运动状态？？

        //navMeshAgent调用setDestination 后，会有一个计算路径的时间，计算过程中pathPending为true. 
        //当前距离小于到抵达目标的一定距离，且已经计算完下一个目标的距离
        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
            controller.GetNewRandomNextWayPoint();
    }

}
