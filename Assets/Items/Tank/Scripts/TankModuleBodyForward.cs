using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankModule/BodyForward")]
public class TankModuleBodyForward : TankModuleDecoration
{
    public enum TargetPos { BodyForwardUp, BodyForward}

    public TargetPos targetPos;

    public override Vector3 GetTargetAnchor(TankModule module)
    {
        TankModuleBody body = module as TankModuleBody;
        if (body == null)
        {
            Debug.LogError("TankModuleBodyForward.GetTargetAnchor() Parameter Should Be 'TankModuleBody'.");
            return Vector3.zero;
        }
        switch (targetPos)
        {
            case TargetPos.BodyForwardUp:
                return body.forwardUp;
            case TargetPos.BodyForward:
                return body.anchors.forward;
        }
        return Vector3.zero;
    }
}
