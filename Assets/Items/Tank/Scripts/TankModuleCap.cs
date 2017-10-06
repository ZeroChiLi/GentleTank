using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankModule/Cap")]
public class TankModuleCap : TankModule
{
    public enum TargetPos { HeadForwardUp, HeadUp, HeadBackUp }

    public TargetPos targetPos;

    public Vector3 GetTargetAnchor(TankModuleHead head)
    {
        switch (targetPos)
        {
            case TargetPos.HeadForwardUp:
                return head.forwardUp;
            case TargetPos.HeadUp:
                return head.anchors.up;
            case TargetPos.HeadBackUp:
                return head.backUp;
        }
        return Vector3.zero;
    }
}
