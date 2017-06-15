using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public override bool Decide(StateController controller) { return Look(controller); }

    [Range(0, 360)]
    public float angle = 90f;
    [Range(0, 180)]
    public float accurate = 6f;
    [Range(0, 100)]
    public float distance = 25f;

    //放射线检测
    private bool Look(StateController controller)
    {
        //一条向前的射线
        if (LookAround(controller, Quaternion.identity,distance, Color.green))
            return true;

        //多一个精确度就多两条对称的射线,每条射线夹角是总角度除与精度
        float subAngle = (angle / 2) / accurate;
        for (int i = 0; i < accurate; i++)
        {
            if (LookAround(controller, Quaternion.Euler(0, -1 * subAngle * (i + 1), 0), distance, Color.green)
                || LookAround(controller, Quaternion.Euler(0, subAngle * (i + 1), 0), distance, Color.green))
                return true;
        }

        return false;
    }

    //射出射线检测是否有Player
    static public bool LookAround(StateController controller, Quaternion eulerAnger,float distance, Color DebugColor)
    {
        Debug.DrawRay(controller.eyes.position, eulerAnger * controller.eyes.forward.normalized * distance, DebugColor);

        RaycastHit hit;
        if (Physics.Raycast(controller.eyes.position, eulerAnger * controller.eyes.forward, out hit, distance) && hit.collider.CompareTag("Player") && hit.collider != controller.colliderSelf)
        {
            controller.chaseTarget = hit.transform;
            return true;
        }
        return false;
    }

}
