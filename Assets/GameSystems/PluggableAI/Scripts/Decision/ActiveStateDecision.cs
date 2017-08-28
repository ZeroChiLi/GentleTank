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
            if (!controller.instancePrefs.Contains("ChaseEnemy"))
                return false;

            if (((Transform)controller.instancePrefs["ChaseEnemy"]).gameObject.activeSelf)
                return true;

            controller.instancePrefs.Remove("ChaseEnemy");     // 死掉了，删掉键值
            return false;
        }

    }
}