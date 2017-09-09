using UnityEngine;

[CreateAssetMenu(menuName = "TankModule/Other")]
public class TankModuleOther : TankModule
{
    public enum TargetTankModuleType
    {
        Head,Body,LeftWheel,RightWheel
    }
    public enum TargetPos
    {
        Forward,Back,Left,Right,Up,Down,HeadLaunch
    }
    public Vector3 connectAnchor;
    public TargetTankModuleType targetType;
    public TargetPos targetPos;

}
