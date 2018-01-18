using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 如果当前距离在停止距离内，自动旋转到目标（避免导航抵达目标后没有面向目标）
    /// </summary>
    [CreateAssetMenu(menuName = "GameSystem/PluggableAI/Actions/TurnWhenTooClose")]
    public class TurnWhenTooCloseAction : Action
    {
        [Range(0, 30f)]
        public float tolerance = 15f;      // 旋转角度的容差

        public override void Act(StateController controller)
        {
            if (!controller.instancePrefs.Contains(CommonCode.ChaseEnemy) || controller.navMeshAgent.pathPending)
                return;
            if (controller.navMeshAgent.remainingDistance < controller.navMeshAgent.stoppingDistance)
            {
                Vector3 direction = ((Transform)controller.instancePrefs[CommonCode.ChaseEnemy]).position - controller.transform.position;
                direction.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                //if (targetRotation.AlmostEquals(controller.transform.rotation,tolerance))
                //    return;
                controller.rigidbodySelf.rotation = Quaternion.RotateTowards(controller.transform.rotation, targetRotation, controller.navMeshAgent.angularSpeed * Time.deltaTime);
            }
        }
    }
}
