using UnityEngine;

namespace GameSystem.AI
{
    [CreateAssetMenu(menuName = "GameSystem/PluggableAI/Actions/BroadcastIfHurt")]
    public class BroadcastIfHurtAction : BroadcastAction
    {
        public override void Act(StateController controller)
        {
            if (controller.IsFeelPain())
                base.Act(controller);
        }
    }
}