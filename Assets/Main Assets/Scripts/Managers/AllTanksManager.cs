using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Configure/All Tanks Manager")]
public class AllTanksManager : ScriptableObject
{
    public TankManager[] tanks;

    public TankManager this[int index]
    {
        get { return tanks[index]; }
        set { tanks[index] = value; }
    }

    public int Length { get { return tanks.Length; } }

    /// <summary>
    /// 获取坦克实例形状列表
    /// </summary>
    /// <returns>坦克实例形状列表</returns>
    public Transform[] GetTanksTransform()
    {
        Transform[] instanceList = new Transform[tanks.Length];
        for (int i = 0; i < tanks.Length; i++)
            instanceList[i] = tanks[i].Instance.transform;
        return instanceList;
    }

}
