using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/BackOffMinAttackRange")]
public class BackOffMinAttackRangeAction : Action
{
    public override void Act(StateController controller)
    {
        BackOffMinAttackRange(controller);
    }

    //后退到最小攻击距离以外
    private void BackOffMinAttackRange(StateController controller)
    {
        if (controller.chaseTarget == null)
            return;
        Vector3 direction = controller.chaseTarget.position - controller.transform.position;
        if (direction.magnitude < controller.defaultStats.attackMinRange)
            controller.aiRigidbody.position += -1 * controller.transform.forward.normalized * controller.defaultStats.navSpeed / 1.5f * Time.deltaTime;
    }

}
