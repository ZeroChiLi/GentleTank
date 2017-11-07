using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 相对目标，后退一直到超过指定距离
    /// </summary>
    [CreateAssetMenu(menuName = "PluggableAI/Actions/BackOff")]
    public class BackOff : Action
    {
        [Range(0, 10f)]
        public float tolerance = 1f;      // 相对自己停止距离的容差

        public override void Act(StateController controller)
        {
            // 有目标、没有正在计算路程、眼前捕获到敌人（避免拐角处两个卡死）、两者距离在停留范围内
            if (controller.instancePrefs.Contains(CommonCode.ChaseEnemy) &&
                !controller.navMeshAgent.pathPending &&
                (bool)controller.statePrefs[CommonCode.CatchEnemy] == true &&
                GameMathf.TwoPosInRange(((Transform)controller.instancePrefs[CommonCode.ChaseEnemy]).position, controller.transform.position, controller.navMeshAgent.stoppingDistance - tolerance))
                controller.rigidbodySelf.position += -1 * controller.transform.forward.normalized * controller.navMeshAgent.speed * Time.deltaTime;
        }
    }
}
