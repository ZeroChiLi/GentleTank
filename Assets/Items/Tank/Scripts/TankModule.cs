using System.Collections.Generic;
using UnityEngine;

public enum TankModuleType
{
    None, Head, Body, WheelLeft, WheelRight, Other, Skin
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
    public Vector3 center;
    public Vector3 forward;
    public Vector3 back;
    public Vector3 left;
    public Vector3 right;
    public Vector3 up;
    public Vector3 down;

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
