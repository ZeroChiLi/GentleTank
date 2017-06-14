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

        //一条向前的射线
        if (LookDecision.LookAround(controller, Quaternion.Euler(0, 0, 0),Color.red))
        {
            controller.Fire();
            return;
        }

        float subAngle = (enemyStats.lookAngle / 2) / enemyStats.lookAccurate;
        for (int i = 0; i < enemyStats.attackAccurate; i++)
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
