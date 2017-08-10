using UnityEngine;

namespace GameSystem.AI
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/ActiveState")]
    public class ActiveStateDecision : Decision
    {
        //追踪的对象是否还活着（active）
        public override bool Decide(StateController controller)
        {
            return controller.chaseTarget.gameObject.activeSelf;
        }

    }
}