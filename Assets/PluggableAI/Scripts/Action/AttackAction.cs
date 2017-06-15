using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action
{
    public override void Act(StateController controller) { Attack(controller); }

    [Range(0,360)]
    public float angle = 15f;
    [Range(0, 180)]
    public float accurate = 2f;
    [Range(0, 100)]
    public float distance = 25f;

    private void Attack(StateController controller)
    {
        var defaultStats = controller.defaultStats;

        //一条向前的射线
        if (LookDecision.LookAround(controller, Quaternion.Euler(0, 0, 0),distance, Color.red))
        {
            controller.Fire();
            return;
        }

        float subAngle = (angle / 2) / accurate;
        for (int i = 0; i < accurate; i++)
        {
            if (LookDecision.LookAround(controller, Quaternion.Euler(0, -1 * subAngle * (i + 1), 0), distance, Color.red)
                || LookDecision.LookAround(controller, Quaternion.Euler(0, subAngle * (i + 1), 0), distance, Color.red))
            {
                controller.Fire();
                return;
            }
        }
    }

}
