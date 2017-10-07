using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankModule/BodyBack")]
public class TankModuleBodyBack : TankModule 
{
    public enum TargetPos { BodyBackUp, BodyBack}

    public TargetPos targetPos;

    public Vector3 GetTargetAnchor(TankModuleBody body)
    {
        switch (targetPos)
        {
            case TargetPos.BodyBackUp:
                return body.backUp;
            case TargetPos.BodyBack:
                return body.anchors.back;
        }
        return Vector3.zero;
    }
}
