using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class TankModuleProperty : ModuleProperty
{
    public float weight;

    public override string GetAllPropertiesString()
    {
        return string.Format("描述：{0}\n重量：{1}\n", description, weight);
    }
}
