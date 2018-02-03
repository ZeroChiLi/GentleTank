using UnityEngine;

public class CaveItemFillManager : MonoBehaviour
{
    public MapGenerator map;
    public CaveItemList mainItemList;
    public CaveItemList groundItemList;
    public CaveItemList fillItemList;
    public Transform caveMainItems;
    public Transform caveGroundItems;

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
        obj = Instantiate(item.prefab, pos, quat, caveMainItems);
        obj.transform.localScale = Vector3.one * scale;
        return obj;
    }

    public GameObject SetGroundItems()
    {
        return null;
    }

}
