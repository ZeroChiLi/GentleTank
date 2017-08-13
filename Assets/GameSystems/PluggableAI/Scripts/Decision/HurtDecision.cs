using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 是否受到伤害
    /// </summary>
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Hurt")]
    public class HurtDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            return controller.IsFeelPain();
        }
    }
}