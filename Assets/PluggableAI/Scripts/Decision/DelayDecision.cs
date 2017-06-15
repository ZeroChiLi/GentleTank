using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Delay")]
public class DelayDecision : Decision
{
    //判断是否超过延时时间
    public override bool Decide(StateController controller) { return controller.CheckIfCountDownElapsed(delayTime); }

    public float delayTime = 1f;

}
