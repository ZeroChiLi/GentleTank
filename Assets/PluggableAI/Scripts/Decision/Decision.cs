using UnityEngine;

//决定下一个状态
public abstract class Decision : ScriptableObject 
{
    public abstract bool Decide(StateController controller);
}
