using UnityEngine;

[System.Serializable]
public abstract class ModuleProperty
{
    public string moduleName;
    public string description;

    /// <summary>
    /// 获取所有属性字符串，不包括部件名字
    /// </summary>
    abstract public string GetAllPropertiesString();
    //{
    //    string[] properties = new string[2];
    //    properties[0] = "描述：" + description;
    //    properties[1] = "重量：" + weight;
    //    return properties;
    //}
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
    //public ModuleProperty property;
    public ModuleAnchors anchors;

    public abstract string GetProperties();
}
