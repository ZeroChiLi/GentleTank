using UnityEngine;


[CreateAssetMenu(menuName = "Module/TankModule/Wheel")]
public class TankModuleWheel : TankModule
{
    public enum WheelType { Left, Right }

    [System.Serializable]
    public class MoveProperties
    {
        public float speed = 12f;
        public float turnSpeed = 180f;
    }

    public WheelType wheelType;
    public MoveProperties moveProperties;
}
