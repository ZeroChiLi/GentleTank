using Item.Tank;
using UnityEngine;

namespace GameSystem.AI
{
    [CreateAssetMenu(menuName = "GameSystem/PluggableAI/Decisions/Listen")]
    public class ListenDecision : Decision
    {
        public string message;

        public override bool Decide(StateController controller)
        {
            if (controller.statePrefs.Contains(CommonCode.BroadcastMessage) && controller.statePrefs[CommonCode.BroadcastMessage] as string == message)
            {
                (controller.playerManager as TankManager).signImage.ShowForSecond(SignImageManager.SignType.Question, 2f, controller.playerManager.RepresentColor);
                return true;
            }
            return false;
        }
    }
}