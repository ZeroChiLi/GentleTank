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
        var defaultStats = controller.defaultStats;

        //一条向前的射线
        if (LookDecision.LookAround(controller, Quaternion.Euler(0, 0, 0),Color.red))
        {
            controller.Fire();
            return;
        }

        float subAngle = (defaultStats.attackAngle / 2) / defaultStats.attackAccurate;
        for (int i = 0; i < defaultStats.attackAccurate; i++)
        {
            if (LookDecision.LookAround(controller, Quaternion.Euler(0, -1 * subAngle * (i + 1), 0), Color.red) 
                || LookDecision.LookAround(controller, Quaternion.Euler(0, subAngle * (i + 1), 0), Color.red))
            {
                controller.Fire();
                return;
            }
        }
    }
    
}
