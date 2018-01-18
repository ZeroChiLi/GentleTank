using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 判断是否超过延时时间
    /// </summary>
    [CreateAssetMenu(menuName = "GameSystem/PluggableAI/Decisions/Delay")]
    public class DelayDecision : Decision
    {
        [Range(0, 60)]
        public float delayTime = 1f;        //延时时间

        public override bool Decide(StateController controller)
        {
            controller.statePrefs.AddIfNotContains(CommonCode.DelayDecisionCD, new CountDownTimer(delayTime, true));

            // 更新倒计时
            return ((CountDownTimer)controller.statePrefs[CommonCode.DelayDecisionCD]).IsTimeUp;
        }
    }
}