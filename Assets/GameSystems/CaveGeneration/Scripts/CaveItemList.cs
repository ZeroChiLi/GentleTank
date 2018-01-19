using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSystem/Cave/CaveItemSizeList")]
public class CaveItemList : ScriptableObject
{
    [System.Serializable]
    public class CaveItem
    {
        public GameObject item;
        public Vector3 size = new Vector3(1, 0, 1);
        public Vector3 offset = Vector3.zero;
        public Vector3 minScale = Vector3.one;
        public Vector3 maxScale = Vector3.one;
    }

    public List<CaveItem> caveItemList;

}
