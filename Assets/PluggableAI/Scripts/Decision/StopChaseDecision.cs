using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/StopChase")]
public class StopChaseDecision : Decision 
{
    public override bool Decide(StateController controller)
    {
        return StopChase(controller);
    }
    
    //超过追逐距离
    private bool StopChase(StateController controller)
    {
        if (controller.chaseTarget == null)
            return true;
        if ((controller.transform.position - controller.chaseTarget.position).magnitude > controller.defaultStats.chaseMaxRange)
        {
            controller.chaseTarget = null;
            //controller.navMeshAgent.isStopped = true;
            return true;
        }
        return false;
    }
}
