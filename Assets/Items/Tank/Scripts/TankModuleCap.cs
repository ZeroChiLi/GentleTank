using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankModule/Cap")]
public class TankModuleCap : TankModuleDecoration
{
    public enum TargetPos { HeadForwardUp, HeadUp, HeadBackUp }

    public TargetPos targetPos;

    public override Vector3 GetTargetAnchor(TankModule module)
    {
        TankModuleHead head = module as TankModuleHead;
        if (head == null)
        {
            Debug.LogError("TankModuleCap.GetTargetAnchor() Parameter Should Be 'TankModuleHead'.");
            return Vector3.zero;
        }
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
