using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSystem/Cave/CaveItemSizeList")]
public class CaveItemList : ScriptableObject
{
    public float minSize = 0.1f;            // 最小尺寸
    public float maxSize = 100f;            // 最大尺寸
    public int gradient = 10;               // 梯度变换次数
    public List<CaveItem> caveItemList;     // 物体列表

    private List<CaveItem> ascendingItemList;   // 物体占地面积的升序列表
    // 每个梯度之间包含的物体，小于最小尺寸在索引0，大于最大尺寸在索引gradient + 1。一共gradient + 2个列表。
    private List<List<CaveItem>> gradientCaveItem = new List<List<CaveItem>>();

    private void OnEnable()
    {
        UpdateGrandientCaveItem();
    }

    /// <summary>
    /// 更新梯度物体列表
    /// </summary>
    public void UpdateGrandientCaveItem()
    {
        UpdateAscendingItemList();
        gradientCaveItem = new List<List<CaveItem>>();
        float spacing = (maxSize - minSize) / gradient;
        for (int i = 0; i < gradient + 2; i++)
            gradientCaveItem.Add(new List<CaveItem>());
        int j = 0;
        for (int i = 0; i < ascendingItemList.Count; i++)
        {
            if (gradientCaveItem[j] == null)
                gradientCaveItem[j] = new List<CaveItem>();
            if (ascendingItemList[i].AreaSize < minSize + j * spacing)
            {
                gradientCaveItem[j].Add(ascendingItemList[i]);
                continue;
            }
            j = (int)((ascendingItemList[i].AreaSize - minSize) / spacing) + 1;
            if (j > gradient + 1)
                j = gradient + 1;
            gradientCaveItem[j].Add(ascendingItemList[i]);
        }
    }

    /// <summary>
    /// 依据物体占地面积升序排序所有物体。
    /// </summary>
    public void UpdateAscendingItemList()
    {
        ascendingItemList = new List<CaveItem>(caveItemList);
        ascendingItemList.Sort(CaveItem.CompareArea);
    }

}
