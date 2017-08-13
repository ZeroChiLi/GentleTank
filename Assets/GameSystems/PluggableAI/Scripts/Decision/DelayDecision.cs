using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 判断是否超过延时时间
    /// </summary>
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Delay")]
    public class DelayDecision : Decision
    {
        [Range(0, 60)]
        public float delayTime = 1f;        //延时时间

        public override bool Decide(StateController controller)
        {
            return CountDownTimer.IsCountDownEnd(controller.statePrefs, "DelayDecisionCD", delayTime, Time.deltaTime, true);
        }
    }
}