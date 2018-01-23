using System.Collections.Generic;
using UnityEngine;
using System.Text;

[CreateAssetMenu(menuName = "GameSystem/Cave/CaveItemSizeList")]
public class CaveItemList : ScriptableObject
{
    public LevelOfSize levelOfSize;
    public List<CaveItem> caveItemList;

    private List<CaveItem> ascendingItemList;

    /// <summary>
    /// 依据物体占地面积升序排序所有物体。
    /// </summary>
    public void UpdateAscendingItemList()
    {
        ascendingItemList = new List<CaveItem>(caveItemList);
        //StringBuilder str = new StringBuilder();
        //for (int i = 0; i < ascendingItemList.Count; i++)
        //    str.Append(ascendingItemList[i].Area + " ");
        //Debug.Log(str + "\n---\n");

        ascendingItemList.Sort(CaveItem.CompareArea);

        //str = new StringBuilder();
        //for (int i = 0; i < ascendingItemList.Count; i++)
        //    str.Append(ascendingItemList[i].Area + " ");
        //Debug.Log(str + "\n");
    }

}
