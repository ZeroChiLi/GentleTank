
[System.Serializable]
public abstract class ModuleProperty
{
    public string description;

    /// <summary>
    /// 获取所有属性字符串
    /// </summary>
    abstract public string GetPropertiesString();
}
