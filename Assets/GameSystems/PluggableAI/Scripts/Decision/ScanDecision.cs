using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 停止导航，自己转圈圈（扫描敌人）
    /// </summary>
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Scan")]
    public class ScanDecision : Decision
    {
        [Range(0, 1800)]
        public float rotatePerSecond = 120f;        //每秒旋转角度，正为顺时针，负为逆时针
        [Range(0, 60)]
        public float searchDuration = 3f;       //旋转时间

        public override bool Decide(StateController controller)
        {
            controller.navMeshAgent.isStopped = true;
            controller.transform.Rotate(0, rotatePerSecond * Time.deltaTime, 0);

            return CountDownTimer.IsCountDownEnd(controller.statePrefs, "ScanDecisionCD", searchDuration, Time.deltaTime,true);
        }
    }
}