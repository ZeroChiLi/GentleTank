using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopChase")]
public class StopChaseDecision : Decision
{
    [Range(0,100)]
    public float distance = 30f;                //停止追逐的最远距离

    //超过追逐距离
    public override bool Decide(StateController controller)
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
