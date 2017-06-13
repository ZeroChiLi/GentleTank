using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        controller.navMeshAgent.destination = controller.wayPointList[controller.nextWayPoint].position;
        controller.navMeshAgent.isStopped = false;  //保持运动状态？？

        //navMeshAgent调用setDestination 后，会有一个计算路径的时间，计算过程中pathPending为true. 
        //当前距离小于到抵达目标的一定距离，且已经计算完下一个目标的距离
        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
        {
            //循环列表获取下一个要到的点，Scene中三个绿色点，在GameManager中配置
            //controller.nextWayPotnt = (controller.nextWayPotnt + 1) % controller.wayPointList.Count;

            //随机选择下一个巡逻点，重复也不怕，因为上面这个判断会依然成立，所以会重新选择
            controller.nextWayPoint = GetNextRandomWayPoint(controller.wayPointList.Count,controller.nextWayPoint);
        }
    }

    private int GetNextRandomWayPoint(int max,int except)
    {
        int randomPoint;
        do
        {
            randomPoint = Random.Range(0, max);
        } while (randomPoint == except);
        return randomPoint;
    }

}
