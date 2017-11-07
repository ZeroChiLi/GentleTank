using UnityEngine;

namespace GameSystem.AI
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Listen")]
    public class ListenDecision : Decision
    {
        public string message;

        public override bool Decide(StateController controller)
        {
            if (controller.statePrefs.Contains(CommonCode.BroadcastMessage)/* && controller.statePrefs[CommonCode.BroadcastMessage] as string == message*/)
            {
                Debug.Log(controller.playerManager.PlayerName + " Get Someone Call " + controller.statePrefs[CommonCode.BroadcastMessage] as string);
                return true;
            }
            return false;
            //return (controller.statePrefs.Contains(CommonCode.BroadcastMessage) 
            //    && controller.statePrefs[CommonCode.BroadcastMessage] as string == message);
        }
    }
}