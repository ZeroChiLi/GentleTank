using UnityEngine;

namespace GameSystem.AI
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/BackOff")]
    public class BackOff : Action
    {
        [Range(0, 100)]
        public float keepDistance = 5f;         //保持的最小距离

        //后退到指定距离
        public override void Act(StateController controller)
        {
            if (controller.chaseTarget == null)
                return;

            if ((controller.chaseTarget.position - controller.transform.position).magnitude < keepDistance)
                controller.rigidbodySelf.position += -1 * controller.transform.forward.normalized * controller.navMeshAgent.speed * Time.deltaTime;
        }
    }
}
