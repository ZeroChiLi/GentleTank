using UnityEngine;

public class AllPropsManager : MonoBehaviour 
{
    public Points spawnPoints;
    public bool spawnAtAwake = false;
    public float period = 10f;
    public int maxPropCount = 3;
    public bool randomProp = true;
    public ObjectPool[] propPools;


}
