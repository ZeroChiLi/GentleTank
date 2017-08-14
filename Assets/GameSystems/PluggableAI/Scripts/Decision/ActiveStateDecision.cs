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
            if (!controller.instancePrefs.Contains("ChaseTarget"))
                return false;

            if (((Transform)controller.instancePrefs["ChaseTarget"]).gameObject.activeSelf)
                return true;

            controller.instancePrefs.Remove("ChaseTarget");     // 死掉了，删掉键值
            return false;
        }

    }
}