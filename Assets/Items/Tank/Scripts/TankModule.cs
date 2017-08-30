using System.Collections.Generic;
using UnityEngine;

public enum ModuleType
{
    None, Turret, Body, WheelLeft,WheelRight
}

[System.Serializable]
public class ConnectAnchor
{
    public ModuleType type;
    public Vector3 anchor;
    public Vector3 rotation;
}

[CreateAssetMenu(menuName = "TankModule")]
public class TankModule : ScriptableObject 
{
    public GameObject prefab;
    public ModuleType type;
    public List<ConnectAnchor> connectAnchor = new List<ConnectAnchor>();

    public ConnectAnchor this[ModuleType type]
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
