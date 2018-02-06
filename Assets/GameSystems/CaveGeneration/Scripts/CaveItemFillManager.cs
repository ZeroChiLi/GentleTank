using System.Collections.Generic;
using UnityEngine;

public class CaveItemFillManager : MonoBehaviour
{
    public MapGenerator map;
    public CaveItemList mainItemList;
    public CaveItemList groundItemList;
    public Transform mainItemsParent;
    public Transform groundItemsParent;

    private Vector3 pos;
    private float scale;
    private CaveItem item;
    private Quaternion quat;
    private GameObject obj;

    /// <summary>
    /// 设置主要物件
    /// </summary>
    public GameObject SetMainItems(CaveRegion region)
    {
        pos = map.GetPosition(region.averageCoord);
        scale = 0f;
        item = mainItemList.GetRandomItem(region.RegionSize, ref scale, 2);
        quat = Quaternion.Euler(0, -GameMathf.RadianToAngle(Mathf.Atan2(region.deviation.y, region.deviation.x)), 0);
        obj = Instantiate(item.prefab, pos, quat, mainItemsParent);
        obj.transform.localScale = Vector3.one * scale;
        return obj;
    }

    /// <summary>
    /// 设置地板纹理物体
    /// </summary>
    /// <returns></returns>
    public List<GameObject> SetGroundItems(CaveRegion region, uint count)
    {
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            item = groundItemList.GetRandomItem();
            obj = Instantiate(item.prefab, map.GetPosition(region.GetRandomCoord()), Quaternion.Euler(GameMathf.RandomY()),groundItemsParent);
            obj.transform.localScale = new Vector3(obj.transform.localScale.x * GameMathf.RandomPlusOrMinus(), obj.transform.localScale.y, obj.transform.localScale.z);
            objs.Add(obj);
        }
        return objs;
    }

}
