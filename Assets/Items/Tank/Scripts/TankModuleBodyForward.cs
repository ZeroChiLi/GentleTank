using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankModule/BodyForward")]
public class TankModuleBodyForward : TankModule 
{
    public enum TargetPos { BodyForwardUp, BodyForward}

    public TargetPos targetPos;

    public Vector3 GetTargetAnchor(TankModuleBody body)
    {
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
