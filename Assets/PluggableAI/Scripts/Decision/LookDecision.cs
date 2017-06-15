using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public override bool Decide(StateController controller) { return Look(controller); }

    public Color debugColor;

    [Range(0, 360)]
    public float angle = 90f;                       //检测前方角度范围
    [Range(0, 100)]
    public float distance = 25f;                    //检测距离
    public float rotatePerSecond = 90f;             //每秒旋转角度

    //放射线检测
    private bool Look(StateController controller)
    {
        if (LookAround(controller, Quaternion.Euler(0, -angle / 2 + Mathf.Repeat(rotatePerSecond * Time.time, angle), 0), distance, debugColor))
            return true;
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
