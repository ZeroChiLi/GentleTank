using UnityEngine;

namespace GameSystem.AI
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Hurt")]
    public class HurtDecision : Decision
    {
        //是否受到伤害
        public override bool Decide(StateController controller)
        {
            return controller.GetHurt();
        }
    }
}