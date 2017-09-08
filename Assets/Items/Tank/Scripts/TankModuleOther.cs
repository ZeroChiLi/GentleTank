using UnityEngine;

[CreateAssetMenu(menuName = "TankModule/Other")]
public class TankModuleOther : TankModule
{
    public enum TargetTankModuleType
    {
        Head,Body,LeftWheel,RightWheel
    }
    public TargetTankModuleType targetType;
    public Vector3 connectAnchor;

}
