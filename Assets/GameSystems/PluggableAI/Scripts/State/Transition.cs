
namespace GameSystem.AI
{
    /// <summary>
    /// 状态转换。通过决定，选中两种状态中其中一个
    /// </summary>
    [System.Serializable]
    public class Transition
    {
        public Decision decision;
        public State trueState;
        public State falseState;
    }
}