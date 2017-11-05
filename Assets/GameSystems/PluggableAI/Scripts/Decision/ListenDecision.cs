using UnityEngine;

namespace GameSystem.AI
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Listen")]
    public class ListenDecision : Decision
    {
        public string message;

        public override bool Decide(StateController controller)
        {
            return (controller.statePrefs.Contains("BroadcastMessage") 
                && controller.statePrefs["BroadcastMessage"] as string == message);
        }
    }
}