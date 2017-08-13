using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 相对目标，后退一直到超过指定距离
    /// </summary>
    [CreateAssetMenu(menuName = "PluggableAI/Actions/BackOff")]
    public class BackOff : Action
    {
        [Range(0, 100)]
        public float keepDistance = 5f;         //保持的最小距离

        public override void Act(StateController controller)
        {
            if (!controller.instancePrefs.Contains("ChaseTarget"))
                return;
            if (GameMathf.TwoPosInRange(controller.instancePrefs.GetValue<Transform>("ChaseTarget").position, controller.transform.position, keepDistance))
                controller.rigidbodySelf.position += -1 * controller.transform.forward.normalized * controller.navMeshAgent.speed * Time.deltaTime;
        }
    }
}
