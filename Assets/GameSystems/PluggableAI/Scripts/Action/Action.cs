using UnityEngine;

namespace GameSystem.AI
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(StateController controller);
    }
}