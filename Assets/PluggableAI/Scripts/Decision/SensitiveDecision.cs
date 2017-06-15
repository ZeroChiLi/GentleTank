using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Sensitive")]
public class SensitiveDecision : Decision
{
    public override bool Decide(StateController controller) { return Sensitive(controller); }

    [Range(0, 100)]
    public float distance = 25f;                //扫描距离
    public float rotateSpeed = 720f;            //每秒旋转角度

    private bool Sensitive(StateController controller)
    {
        if (LookDecision.LookAround(controller, Quaternion.Euler(0, rotateSpeed * Time.time, 0), distance, Color.yellow))
            return true;
        return false;
    }

}
