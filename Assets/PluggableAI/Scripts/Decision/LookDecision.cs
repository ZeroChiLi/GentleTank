using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    [ColorUsage(false)]
    public Color debugColor = Color.green;          //调试颜色
    [Range(0, 360)]
    public float angle = 90f;                       //检测前方角度范围
    [Range(1, 50)]
    public int accuracy = 3;                        //检测精度
    [Range(0, 100)]
    public float distance = 25f;                    //检测距离
    [Range(0, 1800)]
    public float rotatePerSecond = 90f;             //每秒旋转角度

    //放射线检测
    public override bool Decide(StateController controller)
    {
        float subAngle = angle / accuracy;          //每条射线需要检测的角度范围
        for (int i = 0; i < accuracy; i++)
            if (LookAround(controller, Quaternion.Euler(0, -angle / 2 + i * subAngle + Mathf.Repeat(rotatePerSecond * Time.time, subAngle), 0), distance, debugColor))
                return true;
        return false;
    }

    //射出射线检测是否有Player
    static public bool LookAround(StateController controller, Quaternion eulerAnger, float distance, Color DebugColor)
    {
        Debug.DrawRay(controller.eyes.position, eulerAnger * controller.eyes.forward.normalized * distance, DebugColor);

        RaycastHit hit;
        if (Physics.Raycast(controller.eyes.position, eulerAnger * controller.eyes.forward, out hit, distance) && hit.collider.CompareTag("Player") && !controller.IsTeamMate(hit.collider))
        {
            controller.chaseTarget = hit.transform;
            return true;
        }
        return false;
    }

}
