using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankModule/Body")]
public class TankModuleBody : TankModule
{
    [System.Serializable]
    public class HealthProperties
    {
        public float maxHealth = 200f;
    }

    public Vector3 leftWheelTop;
    public Vector3 rightWheelTop;
    public Vector3 raycastPos;
    public Vector3 forwardUp;
    public Vector3 backUp;

    public HealthProperties healthProperties;

}
