using Item.Tank;
using UnityEngine;

namespace GameSystem.AI
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/Broadcast")]
    public class BroadcastAction : Action
    {
        public enum AcceptType { All, Teammates, Enemy }

        public float radius = 30f;
        public AcceptType acceptType;
        [Range(0.1f, 10f)]
        public float period = 3f;
        public string messages;

        public override void Act(StateController controller)
        {
            controller.statePrefs.AddIfNotContains(CommonCode.BroadcastActionCD, new CountDownTimer(period, true));

            if (!((CountDownTimer)controller.statePrefs[CommonCode.BroadcastActionCD]).IsTimeUp || string.IsNullOrEmpty(messages) || AllPlayerManager.Instance == null || controller.Team == null)
                return;
            TankManager tank = controller.playerManager as TankManager;
            tank.signImage.ShowForSecond(SignImageManager.SignType.Exclamation,2f, controller.playerManager.RepresentColor);
            tank.PlaySignalExpand(radius * 2f, period);
            for (int i = 0; i < AllPlayerManager.Instance.Count; i++)
            {
                if (AllPlayerManager.Instance[i] != controller.playerManager
                    && AllPlayerManager.Instance[i].gameObject.activeInHierarchy
                    && GameMathf.TwoPosInRange(controller.transform.position, AllPlayerManager.Instance[i].transform.position, radius))
                {
                    TankManager target = AllPlayerManager.Instance[i] as TankManager;
                    if (target == null)
                        continue;
                    switch (acceptType)
                    {
                        case AcceptType.All:
                            break;
                        case AcceptType.Teammates:
                            if (AllPlayerManager.Instance[i].Team != controller.Team)
                                continue;
                            break;
                        case AcceptType.Enemy:
                            if (AllPlayerManager.Instance[i].Team == controller.Team)
                                continue;
                            break;
                    }
                    target.stateController.statePrefs.AddIfNotContains(CommonCode.BroadcastMessage, messages);
                }
            }
        }
    }
}