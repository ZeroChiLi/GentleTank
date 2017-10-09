using UnityEngine;

[System.Serializable]
public abstract class ModuleProperty
{
    public string description;

    /// <summary>
    /// 获取所有属性字符串
    /// </summary>
    abstract public string GetPropertiesString();
}

[System.Serializable]
public struct ModuleAnchors
{
    public Vector3 center;
    public Vector3 forward;
    public Vector3 back;
    public Vector3 left;
    public Vector3 right;
    public Vector3 up;
    public Vector3 down;
}

public abstract class ModuleBase : ScriptableObject
{
    public string moduleName;
    public GameObject prefab;
    public Sprite preview;
    public ModuleAnchors anchors;

    public abstract string GetProperties();
}
