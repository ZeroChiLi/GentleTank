using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 追踪的对象是否还活着（activeSelf）
    /// </summary>
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/ActiveState")]
    public class ActiveStateDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            if (!controller.instancePrefs.Contains(CommonCode.ChaseEnemy))
                return false;

            if (((Transform)controller.instancePrefs[CommonCode.ChaseEnemy]).gameObject.activeSelf)
                return true;

            controller.instancePrefs.Remove(CommonCode.ChaseEnemy);     // 死掉了，删掉键值
            return false;
        }

    }
}