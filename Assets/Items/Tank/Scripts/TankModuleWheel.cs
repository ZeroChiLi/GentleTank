using UnityEngine;


[CreateAssetMenu(menuName = "TankModule/Wheel")]
public class TankModuleWheel : TankModule 
{
    public enum WheelType
    {
        Left,Right
    }

    public WheelType wheelType;

}
