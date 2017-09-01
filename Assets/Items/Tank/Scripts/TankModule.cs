using System.Collections.Generic;
using UnityEngine;

public enum TankModuleType
{
    None, Turret, Body, WheelLeft,WheelRight
}

[System.Serializable]
public class ConnectAnchor
{
    public TankModuleType type;
    public Vector3 anchor;
}

[CreateAssetMenu(menuName = "Tank/TankModule")]
public class TankModule : ScriptableObject 
{
    public GameObject prefab;
    public Sprite preview;
    public TankModuleType type;
    public List<ConnectAnchor> connectAnchor = new List<ConnectAnchor>();

    public ConnectAnchor this[TankModuleType type]
    {
        get
        {
            for (int i = 0; i < connectAnchor.Count; i++)
                if (connectAnchor[i].type == type)
                    return connectAnchor[i];
            return null;
        }
    }
}
