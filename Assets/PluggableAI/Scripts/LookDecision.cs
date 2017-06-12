using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/Patrol")]
public class LookDecision : Decision 
{
    public override bool Decide(StateController controller)
    {
        return Look(controller);
    }
    
    //放射线检测
    private bool Look(StateController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.lookRange, Color.green);

        //射出一球体的射线检测是否有Player
        if (Physics.SphereCast(controller.eyes.position,controller.enemyStats.lookSphereCastRadius,controller.eyes.forward,out hit,controller.enemyStats.lookRange) && hit.collider.CompareTag("Player"))
        {
            controller.chaseTarget = hit.transform;
            return true;
        }
        else
            return false;
    }
}
