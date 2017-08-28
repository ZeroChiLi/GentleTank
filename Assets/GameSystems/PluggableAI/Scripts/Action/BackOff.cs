using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 相对目标，后退一直到超过指定距离
    /// </summary>
    [CreateAssetMenu(menuName = "PluggableAI/Actions/BackOff")]
    public class BackOff : Action
    {
        [Range(0, 10f)]
        public float tolerance = 1f;      // 相对自己停止距离的容差

        public override void Act(StateController controller)
        {
            if (!controller.instancePrefs.Contains("ChaseTarget") || controller.navMeshAgent.pathPending)
                return;
            if (GameMathf.TwoPosInRange(((Transform)controller.instancePrefs["ChaseTarget"]).position, controller.transform.position, controller.navMeshAgent.stoppingDistance - tolerance))
                controller.rigidbodySelf.position += -1 * controller.transform.forward.normalized * controller.navMeshAgent.speed * Time.deltaTime;
        }
    }
}
