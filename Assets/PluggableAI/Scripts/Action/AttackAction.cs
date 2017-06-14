using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action
{
    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        var enemyStats = controller.enemyStats;

        Vector3 startLeft = controller.eyes.position - controller.eyes.right.normalized * enemyStats.lookSphereCastRadius / 2;
        Vector3 startRight = controller.eyes.position + controller.eyes.right.normalized * enemyStats.lookSphereCastRadius / 2;

        DebugDraw(controller, startLeft, startRight, Quaternion.Euler(0, 0, 0));
        if (LookAround(controller, controller.eyes.forward) && controller.CheckIfCountDownElapsed(controller.enemyStats.attackRate))
        {
            controller.Fire();
            return;
        }

        float attackAngle = enemyStats.attackAngle / 2;
        float subAngle = attackAngle / enemyStats.attackAccurate;
        //一个精确度就多两条对称的射线
        for (int i = 0; i < enemyStats.attackAccurate; i++)
        {
            //两条射线
            Quaternion eulerAngerLeft = Quaternion.Euler(0, -1 * subAngle * (i + 1), 0);
            Quaternion eulerAngerRight = Quaternion.Euler(0, subAngle * (i + 1), 0);
            DebugDraw(controller, startLeft, startRight, eulerAngerLeft);
            DebugDraw(controller, startLeft, startRight, eulerAngerRight);

            if (LookAround(controller, eulerAngerLeft * controller.eyes.forward) || LookAround(controller, eulerAngerRight * controller.eyes.forward))
            {
                controller.Fire();
                return;
            }
        }

    }


    //射出一球体的射线检测是否有Player
    private bool LookAround(StateController controller, Vector3 lookForward)
    {
        RaycastHit hit;
        if (Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius, lookForward, out hit, controller.enemyStats.attackMaxRange))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
            return false;
        }
        else
            return false;
    }

    //调试信息：绘制两条射线代表扫描的范围，两线之内都会检测到
    private void DebugDraw(StateController controller, Vector3 left, Vector3 right, Quaternion eulerAnger)
    {
        Debug.DrawRay(left, eulerAnger * controller.eyes.forward.normalized * controller.enemyStats.lookRange, Color.red);
        Debug.DrawRay(right, eulerAnger * controller.eyes.forward.normalized * controller.enemyStats.lookRange, Color.red);
    }


}
