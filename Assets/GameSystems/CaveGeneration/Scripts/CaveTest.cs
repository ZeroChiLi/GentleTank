using System.Collections.Generic;
using UnityEngine;

public class CaveTest : MonoBehaviour
{
    public MapGenerator mapGenerator;           // 产生地图集
    public MeshGenerator meshGenerator;         // 渲染渲染墙体网格
    public CaveItemFillManager caveItemFill;    // 填充小物体
    public Transform itemsParent;               // 所有填充物体的父类
    public uint groundItemRatio = 500;          // 每多少面积填充一个物体

    public CaveDebug caveDebug;
    [System.Serializable]
    public struct CaveDebug
    {
        public ObjectPool flags;
        public bool mouseClickToRebuild;
        public bool cleanInnerWalls;
        public bool setItems;
        public bool showRoomFlags;
        public bool showWallFlags;
        public bool showGizmos;
    }

    private Transform mainItemsParent;          // 所有主物体父对象
    private Transform groundItemsParent;        // 所有地面物体父对象

    private void Start()
    {
        RebuildMap();
    }

    private void Update()
    {
        if (caveDebug.mouseClickToRebuild && Input.GetMouseButtonDown(0))
            RebuildMap();
    }

    /// <summary>
    /// 重建地图
    /// </summary>
    public void RebuildMap()
    {
        CleanArtItem();
        CreateItemsParent();
        mapGenerator.GenerateMap();
        if (caveDebug.cleanInnerWalls)
            CleanInnerWallRegion();
        if (caveDebug.setItems)
            SetItems();
        meshGenerator.GenerateMesh(mapGenerator.borderedMap);
        caveDebug.flags.InactiveAll();
        if (caveDebug.showRoomFlags)
            MarkFlagToRooms(mapGenerator.caveRooms);
        if (caveDebug.showWallFlags)
            MarkFlagToWalls(mapGenerator.caveWalls);
    }

    /// <summary>
    /// 清除所有内部墙体
    /// </summary>
    public void CleanInnerWallRegion()
    {
        for (int i = 0; i < mapGenerator.caveWalls.Count; i++)
            if (!mapGenerator.caveWalls[i].isBorder)
                mapGenerator.SetRegion(mapGenerator.caveWalls[i], TileType.Empty);
    }

    /// <summary>
    /// 添加物体
    /// </summary>
    public void SetItems()
    {
        for (int i = 0; i < mapGenerator.caveWalls.Count; i++)
            if (!mapGenerator.caveWalls[i].isBorder)
                caveItemFill.SetMainItems(mapGenerator.caveWalls[i], mainItemsParent);
        CaveRoom room;
        for (int i = 0; i < mapGenerator.caveRooms.Count; i++)
        {
            room = mapGenerator.caveRooms[i];
            caveItemFill.SetGroundItems(room, (uint)(room.RegionSize / groundItemRatio), groundItemsParent);
        }
    }

    /// <summary>
    /// 标记房间（平均点）
    /// </summary>
    private void MarkFlagToRooms(List<CaveRoom> rooms)
    {
        for (int i = 0; i < rooms.Count; i++)
            caveDebug.flags.GetNextObject(mapGenerator.GetPosition(rooms[i].averageCoord));
    }

    /// <summary>
    /// 标记墙体（平均点）
    /// </summary>
    private void MarkFlagToWalls(List<CaveWall> walls)
    {
        for (int i = 0; i < walls.Count; i++)
            if (!walls[i].isBorder)
                caveDebug.flags.GetNextObject(mapGenerator.GetPosition(walls[i].averageCoord));
    }

    /// <summary>
    /// 创建物体们的父类
    /// </summary>
    private void CreateItemsParent()
    {
        mainItemsParent = new GameObject("CaveMainItems").transform;
        groundItemsParent = new GameObject("CaveGroundItems").transform;
        mainItemsParent.SetParent(itemsParent);
        groundItemsParent.SetParent(itemsParent);
    }

    /// <summary>
    /// 清除场景物体
    /// </summary>
    private void CleanArtItem()
    {
        if (mainItemsParent)
            Destroy(mainItemsParent.gameObject);
        if (groundItemsParent)
            Destroy(groundItemsParent.gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!caveDebug.showGizmos || mapGenerator == null)
            return;

        Gizmos.color = Color.red;
        for (int i = 0; i < mapGenerator.caveWalls.Count; i++)
        {
            if (mapGenerator.caveWalls[i].isBorder)
                continue;
            Vector3 pos = mapGenerator.GetPosition(mapGenerator.caveWalls[i].averageCoord);
            Vector3 len = new Vector3(mapGenerator.caveWalls[i].deviation.x, 0, mapGenerator.caveWalls[i].deviation.y);
            Gizmos.DrawLine(pos - len / 2, pos + len / 2);
        }
    }
}
