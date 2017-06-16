using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action
{
    public override void Act(StateController controller) { Attack(controller); }

    public Color debugColor;

    [Range(0, 360)]
    public float angle = 15f;                       //检测前方角度范围
    [Range(0, 100)]
    public float distance = 25f;                    //检测距离
    public float rotatePerSecond = 90f;             //每秒旋转角度

    //原理同LookDecision
    private void Attack(StateController controller)
    {
        //如果瞄得很准，射线射正前方就可以一次抓到目标
        if (LookDecision.LookAround(controller, Quaternion.identity, distance, debugColor))
            controller.Fire();
        else if (LookDecision.LookAround(controller, Quaternion.Euler(0, -angle / 2 + Mathf.Repeat(rotatePerSecond * Time.time, angle), 0), distance, debugColor))
            controller.Fire();
    }

}
