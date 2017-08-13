using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 决定下一个状态
    /// </summary>
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(StateController controller);
    }
}