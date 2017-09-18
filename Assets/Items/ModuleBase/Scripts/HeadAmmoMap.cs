using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Module/HeadAmmoMap")]
public class HeadAmmoMap : ScriptableObject
{
    [System.Serializable]
    public class HeadAndAmmo
    {
        public TankModuleHead head;
        public AmmoModule ammo;
    }

    public List<HeadAndAmmo> headAmmoList = new List<HeadAndAmmo>();

    /// <summary>
    /// 通过头部获取子弹部件
    /// </summary>
    /// <param name="head">头部部件</param>
    /// <returns>返回对应的子弹部件</returns>
    public AmmoModule GetAmmo(TankModuleHead head)
    {
        for (int i = 0; i < headAmmoList.Count; i++)
            if (headAmmoList[i].head == head)
                return headAmmoList[i].ammo;
        return null;
    }


}
