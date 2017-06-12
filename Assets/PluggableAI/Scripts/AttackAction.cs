using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action 
{
    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.attackRange, Color.red);
        
        if (Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.attackRange)
            && hit.collider.CompareTag("Player"))
        {
            //过了时间间隔，可以发炮
            if (controller.CheckIfCountDownElapsed(controller.enemyStats.attackRate))
            {
                //射击的力度（长度），开始攻击的距离
                controller.tankShooting.Fire(controller.enemyStats.attackForce, controller.enemyStats.attackRate);
            }
        }
    }

}
