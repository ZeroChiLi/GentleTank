using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Hurt")]
public class HurtDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return Hurt(controller);
    }

    private bool Hurt(StateController controller)
    {
        if(controller.GetHurt())
        {
            controller.SetHurt(false);
            return true;
        }
        return false;
    }

}
