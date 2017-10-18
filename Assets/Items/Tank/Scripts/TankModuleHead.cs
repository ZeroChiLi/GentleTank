using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankModule/Head")]
public class TankModuleHead : TankModule 
{
    public Vector3 launchPos;
    public Vector3 forwardUp;
    public Vector3 backUp;

    public MonoScript attackScript;
    public AudioClip chargingClip;
    public AudioClip fireClip;
}
