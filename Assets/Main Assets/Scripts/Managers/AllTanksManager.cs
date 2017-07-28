using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Configure/All Tanks Manager")]
public class AllTanksManager : ScriptableObject
{
    static private AllTanksManager instance;                    // 坦克管理单例
    static public AllTanksManager Instance { get { return instance; } }

    public TankManager[] tanks;                                 // 所有坦克
    private List<TankManager> activeTanks;                      // 所有有效坦克

    // 有效坦克的索引
    public TankManager this[int index]
    {
        get { return activeTanks[index]; }
        set { activeTanks[index] = value; }
    }

    public int OriginalLength { get { return tanks.Length; } }  //所有坦克数量
    public int Count { get { return activeTanks.Count; } }      //有效坦克数量

    /// <summary>
    /// 设置单例
    /// </summary>
    public void SetupInstance()
    {
        instance = this;
    }

    /// <summary>
    /// 获取坦克（包括无效）
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>索引对应坦克</returns>
    public TankManager GetOriginalTank(int index)
    {
        return tanks[index];
    }

    /// <summary>
    /// 激活时初始化
    /// </summary>
    private void OnEnable()
    {
        activeTanks = new List<TankManager>();
        for (int i = 0; i < tanks.Length; i++)
            if (tanks[i].active)
                activeTanks.Add(tanks[i]);
    }

    /// <summary>
    /// 获取坦克实例形状列表
    /// </summary>
    /// <returns>坦克实例形状列表</returns>
    public List<Transform> GetTanksTransform()
    {
        List<Transform> instanceList = new List<Transform>();
        for (int i = 0; i < Count; i++)
            instanceList.Add(activeTanks[i].Instance.transform);
        return instanceList;
    }

    /// <summary>
    /// 通过ID获取坦克
    /// </summary>
    /// <param name="id">坦克ID</param>
    /// <returns>返回ID对应的坦克</returns>
    public TankManager GetTankByID(int id)
    {
        for (int i = 0; i < Count; i++)
            if (id == activeTanks[i].PlayerID)
                return activeTanks[i];
        return null;
    }

}