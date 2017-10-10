using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankModule/BodyBack")]
public class TankModuleBodyBack : TankModuleDecoration
{
    public enum TargetPos { BodyBackUp, BodyBack}

    public TargetPos targetPos;

    public override Vector3 GetTargetAnchor(TankModule module)
    {
        TankModuleBody body = module as TankModuleBody;
        if (body == null)
        {
            Debug.LogError("TankModuleBodyBack.GetTargetAnchor() Parameter Should Be 'TankModuleBody'.");
            return Vector3.zero;
        }
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
