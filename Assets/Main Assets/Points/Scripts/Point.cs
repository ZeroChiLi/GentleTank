using UnityEngine;

[System.Serializable]
public class Point
{
    [HideInInspector]
    public bool enable = true; 
    public Vector3 position;
    public Vector3 rotation;
}