
[System.Serializable]
public class TankModuleProperty : ModuleProperty
{
    public float weight;

    public override string GetPropertiesString()
    {
        return string.Format("描述：{0}\n重量：{1}", description, weight);
    }
}
