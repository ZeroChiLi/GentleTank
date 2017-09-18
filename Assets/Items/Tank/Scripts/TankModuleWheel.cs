using UnityEngine;


[CreateAssetMenu(menuName = "Module/TankModule/Wheel")]
public class TankModuleWheel : TankModule 
{
    public enum WheelType
    {
        Left,Right
    }

    public WheelType wheelType;

}
