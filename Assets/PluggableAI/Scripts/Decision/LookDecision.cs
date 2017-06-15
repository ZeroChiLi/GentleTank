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
        //一条向前的射线
        if (LookAround(controller, Quaternion.identity, Color.green))
            return true;

        //多一个精确度就多两条对称的射线,每条射线夹角是总角度除与精度
        float subAngle = (controller.defaultStats.lookAngle / 2) / controller.defaultStats.lookAccurate;
        for (int i = 0; i < controller.defaultStats.lookAccurate; i++)
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
        Debug.DrawRay(controller.eyes.position, eulerAnger * controller.eyes.forward.normalized * controller.defaultStats.lookRange, DebugColor);

        RaycastHit hit;
        if (Physics.Raycast(controller.eyes.position, eulerAnger * controller.eyes.forward, out hit, controller.defaultStats.lookRange) && hit.collider.CompareTag("Player") && hit.collider != controller.colliderSelf)
        {
            controller.chaseTarget = hit.transform;
            return true;
        }
        return false;
    }

}
