using System.Collections.Generic;
using UnityEngine;

public abstract class TankModuleDecoration : TankModule 
{
    public Vector3 connectAnchor;

    public abstract Vector3 GetTargetAnchor(TankModule module);

}
