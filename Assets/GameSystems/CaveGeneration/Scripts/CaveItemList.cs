using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSystem/Cave/CaveItemSizeList")]
public class CaveItemList : ScriptableObject
{
    public float minSize = 0f;            // 最小尺寸
    public float maxSize = 100f;            // 最大尺寸
    public int gradient = 10;               // 梯度变换次数
    public List<CaveItem> caveItemList;     // 物体列表

    public float SizeLength { get { return maxSize - minSize; } }
    public float Spacing { get { return SizeLength / gradient; } }

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
    private void UpdateGrandientCaveItem()
    {
        UpdateAscendingItemList();
        gradientCaveItem = new List<List<CaveItem>>();
        for (int i = 0; i < gradient + 2; i++)
            gradientCaveItem.Add(new List<CaveItem>());
        for (int i = 0; i < ascendingItemList.Count; i++)
            gradientCaveItem[GetSizeIndex(ascendingItemList[i].AreaSize)].Add(ascendingItemList[i]);
    }

    /// <summary>
    /// 依据物体占地面积升序排序所有物体。
    /// </summary>
    private void UpdateAscendingItemList()
    {
        ascendingItemList = new List<CaveItem>(caveItemList);
        ascendingItemList.Sort(CaveItem.CompareArea);
    }

    /// <summary>
    /// 通过所需大小，获取合理物体
    /// </summary>
    /// <param name="size">所需大小</param>
    /// <param name="scale">输出物体的缩放值</param>
    /// <param name="approximate">索引的容差值</param>
    public CaveItem GetRandomItem(float size, ref float scale, uint approximate = 1)
    {
        List<CaveItem> items = GetItemsWithSize(size, approximate);
        if (items.Count == 0)
            return null;
        CaveItem item = items[Random.Range(0, items.Count)];
        scale = Mathf.Sqrt(size / item.AreaSize);
        return item;
    }

    /// <summary>
    /// 获取随即物体
    /// </summary>
    public CaveItem GetRandomItem()
    {
        return caveItemList[Random.Range(0, caveItemList.Count)];
    }

    /// <summary>
    /// 通过所需大小，获取对应相近的物体列表
    /// </summary>
    private List<CaveItem> GetItemsWithSize(float size, uint approximate = 1)
    {
        List<CaveItem> objs = new List<CaveItem>();
        int currentIndex = GetSizeIndex(size);
        CollectItemToList(objs, currentIndex);
        for (int i = 1; i < approximate + 1; i++)
        {
            if (IndexIsInRange(currentIndex + i))
                CollectItemToList(objs, currentIndex + i);
            if (IndexIsInRange(currentIndex - i))
                CollectItemToList(objs, currentIndex - i);
        }
        return objs;
    }

    /// <summary>
    /// 获取尺寸对应列表的索引
    /// </summary>
    private int GetSizeIndex(float size)
    {
        if (size <= minSize)
            return 0;
        if (size >= maxSize)
            return gradient + 1;
        return (int)((size - minSize) / Spacing) + 1;
    }

    /// <summary>
    /// 判断索引是否合法
    /// </summary>
    private bool IndexIsInRange(int index)
    {
        return index >= 0 && index < gradient + 2;
    }

    /// <summary>
    /// 收集指定索引的所有物体到列表中
    /// </summary>
    private void CollectItemToList(List<CaveItem> list, int index)
    {
        for (int i = 0; i < gradientCaveItem[index].Count; i++)
            if (!list.Contains(gradientCaveItem[index][i]))
                list.Add(gradientCaveItem[index][i]);
    }
}
