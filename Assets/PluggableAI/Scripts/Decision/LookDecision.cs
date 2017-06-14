using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return Look(controller);
    }

    //放射线检测
    private bool Look(StateController controller)
    {
        var enemyStats = controller.enemyStats;

        //一条向前的射线
        if (LookAround(controller, Quaternion.identity, Color.green))
            return true;

        //多一个精确度就多两条对称的射线,每条射线夹角是总角度除与精度
        float subAngle = (enemyStats.lookAngle / 2) / enemyStats.lookAccurate;
        for (int i = 0; i < enemyStats.lookAccurate; i++)
        {
            if (LookAround(controller, Quaternion.Euler(0, -1 * subAngle * (i + 1), 0), Color.green) 
                || LookAround(controller, Quaternion.Euler(0, subAngle * (i + 1), 0), Color.green))
                return true;
        }

        return false;
    }

    //射出射线检测是否有Player
    static public bool LookAround(StateController controller, Quaternion eulerAnger,Color DebugColor)
    {
        DebugDraw(controller,eulerAnger, DebugColor);
        
        RaycastHit hit;
        if (Physics.Raycast(controller.eyes.position, eulerAnger * controller.eyes.forward, out hit, controller.enemyStats.lookRange) && hit.collider.CompareTag("Player"))
        {
            controller.chaseTarget = hit.transform;
            return true;
        }
        return false;
    }

    //调试信息：绘制两条射线代表扫描的范围，两线之内都会检测到
    static public void DebugDraw(StateController controller, Quaternion eulerAnger, Color DebugColor)
    {
        Debug.DrawRay(controller.eyes.position, eulerAnger * controller.eyes.forward.normalized * controller.enemyStats.lookRange, DebugColor);
    }

}
