using Item.Tank;
using System.Collections;
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
        public ObjectPool signalExpandPool;                     // 信号扩展对象池

        public override void Act(StateController controller)
        {
            controller.statePrefs.AddIfNotContains(CommonCode.BroadcastActionCD, new CountDownTimer(0, false));

            if (!((CountDownTimer)controller.statePrefs[CommonCode.BroadcastActionCD]).IsTimeUp || string.IsNullOrEmpty(messages) || AllPlayerManager.Instance == null || controller.Team == null)
                return;
            (controller.statePrefs[CommonCode.BroadcastActionCD] as CountDownTimer).Reset(period, true);
            TankManager tank = controller.playerManager as TankManager;
            tank.signImage.ShowForSecond(SignImageManager.SignType.Exclamation, 2f, controller.playerManager.RepresentColor);
            PlaySignalExpand(controller, radius, period);
        }

        /// <summary>
        /// 显示信号扩展
        /// </summary>
        public void PlaySignalExpand(StateController controller, float radius, float time)
        {
            SignalExpand signal = signalExpandPool.GetNextObject(true).GetComponent<SignalExpand>();
            signal.transform.position = controller.transform.position;
            signal.Play(Vector3.one, Vector3.one * radius * 2f, time, controller.playerManager.Team == null ? Color.white : controller.playerManager.Team.TeamColor);
            controller.StartCoroutine(FindAndSend(controller, signal));
        }

        /// <summary>
        /// 查找半径范围内所有符合条件的对象，并发送信息
        /// </summary>
        private IEnumerator FindAndSend(StateController controller, SignalExpand signal)
        {
            while (!signal.Timer.IsTimeUp)
            {
                for (int i = 0; i < AllPlayerManager.Instance.Count; i++)
                {
                    if (AllPlayerManager.Instance[i] != controller.playerManager
                        && AllPlayerManager.Instance[i].gameObject.activeInHierarchy
                        && GameMathf.TwoPosInRange(controller.transform.position, AllPlayerManager.Instance[i].transform.position, signal.CurrentPercent * radius))
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
                yield return null;
            }

        }

    }
}