using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 动作（在指定状态，循环执行的动作）
    /// </summary>
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(StateController controller);
    }
}