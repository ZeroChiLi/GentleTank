using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSystem/CaveItemSizeList")]
public class CaveItemList : ScriptableObject 
{
    [System.Serializable]
    public struct CaveItem
    {
        public GameObject item;
        public Vector3 size;
        public Vector3 offset;
    }

    public List<CaveItem> caveItemList;

}
