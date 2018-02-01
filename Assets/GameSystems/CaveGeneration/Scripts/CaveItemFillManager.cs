using UnityEngine;

public class CaveItemFillManager : MonoBehaviour 
{
    public MapGenerator map;
    public CaveItemList mainItemList;
    public CaveItemList groundItemList;
    public CaveItemList fillItemList;

    public GameObject SetItems(CaveRegion region)
    {
        Vector3 pos = map.GetPosition(region.averageCoord);
        float scale = 0f;
        CaveItem item = mainItemList.GetRandomItem(region.RegionSize,ref scale,2);
        Quaternion quat = Quaternion.Euler(0,-Mathf.Atan2(region.NormalizedDeviation.y, region.NormalizedDeviation.x),0);
        GameObject obj = Instantiate(item.prefab, pos,quat);
        obj.transform.localScale = Vector3.one * scale;
        return obj;
    }

}
