using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankModule/Face")]
public class TankModuleFace : TankModule 
{
    public enum TargetPos { HeadForwardUp,HeadForward}

    public TargetPos targetPos;

    public Vector3 GetTargetAnchor(TankModuleHead head)
    {
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
