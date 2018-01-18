using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 随机抵达下一个巡逻点
    /// </summary>
    [CreateAssetMenu(menuName = "GameSystem/PluggableAI/Actions/RandomlyPatrol")]
    public class PatrolRandomlyAction : Action
    {
        public override void Act(StateController controller)
        {
            controller.navMeshAgent.enabled = true;
            controller.navMeshAgent.destination = controller.NextWaypoint.position;
            controller.navMeshAgent.isStopped = false;  //保持运动状态

            //navMeshAgent调用setDestination 后，会有一个计算路径的时间，计算过程中pathPending为true. 
            //当前距离小于到抵达目标的一定距离，且已经计算完下一个目标的距离
            if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
                controller.UpdateNextWayPoint(true);
        }
    }
}
