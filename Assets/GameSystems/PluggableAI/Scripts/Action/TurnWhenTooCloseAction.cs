using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 如果当前距离在停止距离内，自动旋转到目标（避免导航抵达目标后没有面向目标）
    /// </summary>
    [CreateAssetMenu(menuName = "PluggableAI/Actions/TurnWhenTooClose")]
    public class TurnWhenTooCloseAction : Action
    {
        public override void Act(StateController controller)
        {
            if (!controller.instancePrefs.Contains("ChaseTarget"))
                return;
            if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
            {
                Vector3 direction = controller.instancePrefs.GetValue<Transform>("ChaseTarget").position - controller.transform.position;
                direction.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                controller.rigidbodySelf.rotation = Quaternion.RotateTowards(controller.transform.rotation, targetRotation, controller.navMeshAgent.angularSpeed * Time.deltaTime);
            }
        }
    }
}
