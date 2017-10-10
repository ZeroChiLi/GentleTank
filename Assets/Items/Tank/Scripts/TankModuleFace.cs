using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankModule/Face")]
public class TankModuleFace : TankModuleDecoration
{
    public enum TargetPos { HeadForwardUp,HeadForward}

    public TargetPos targetPos;

    public override Vector3 GetTargetAnchor(TankModule module)
    {
        TankModuleHead head = module as TankModuleHead;
        if (head == null)
        {
            Debug.LogError("TankModuleFace.GetTargetAnchor() Parameter Should Be 'TankModuleHead'.");
            return Vector3.zero;
        }
        switch (targetPos)
        {
            case TargetPos.HeadForwardUp:
                return head.forwardUp;
            case TargetPos.HeadForward:
                return head.anchors.forward;
        }
        return Vector3.zero;
    }

}
