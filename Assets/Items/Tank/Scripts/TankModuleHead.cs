using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankModule/Head")]
public class TankModuleHead : TankModule 
{
    [System.Serializable]
    public class AttackProperties
    {
        public MonoScript attackScript;
        public AudioClip chargingClip;
        public AudioClip fireClip;

        public float coolDownTime = 1f;
        public float minLaunchForce = 15f;          // 最小发射力度
        public float maxLaunchForce = 30f;          // 最大发射力度
        public float maxChargeTime = 0.75f;         // 最大发射蓄力时间
        public float damage = 50;                   // 伤害值

        [HideInInspector]
        public ObjectPool ammoPool;
    }

    public Vector3 launchPos;
    public Vector3 forwardUp;
    public Vector3 backUp;

    public AttackProperties attackProperties;
}
