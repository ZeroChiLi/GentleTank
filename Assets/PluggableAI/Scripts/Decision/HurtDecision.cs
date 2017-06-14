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

    //是否受到伤害
    private bool Hurt(StateController controller)
    {
        return controller.GetHurt();
    }

}
