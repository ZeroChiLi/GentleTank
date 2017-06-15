using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Sensitive")]
public class SensitiveDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return Sensitive(controller);
    }

    // 0.5秒时间扫一圈
    private bool Sensitive(StateController controller)
    {
        if (LookDecision.LookAround(controller, Quaternion.Euler(0, 720 * Time.time,0), Color.yellow))
            return true;

        return false;
    }

}
