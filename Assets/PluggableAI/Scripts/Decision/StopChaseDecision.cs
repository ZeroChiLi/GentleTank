using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopChase")]
public class StopChaseDecision : Decision
{
    public override bool Decide(StateController controller) { return StopChase(controller); }

    [Range(0,100)]
    public float distance = 30f;                //停止追逐的最远距离

    //超过追逐距离
    private bool StopChase(StateController controller)
    {
        if (controller.chaseTarget == null)
            return true;
        if ((controller.transform.position - controller.chaseTarget.position).magnitude > distance)
        {
            controller.chaseTarget = null;
            return true;
        }
        return false;
    }
}
