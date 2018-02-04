using System.Collections.Generic;
using UnityEngine;

public class CaveTest : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public MeshGenerator meshGenerator;
    public CaveItemFillManager caveItemFill;
    public ObjectPool flags;
    public bool cleanInnerWalls;
    public bool setItems;
    public bool showFlags;
    public bool showGizmos;

    private List<GameObject> artItems = new List<GameObject>();

    private void Start()
    {
        ReBuildMap();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ReBuildMap();
    }

    /// <summary>
    /// 重建地图
    /// </summary>
    public void ReBuildMap()
    {
        CleanArtItem();
        mapGenerator.GenerateMap();
        if (cleanInnerWalls)
            CleanInnerWallRegion();
        if (setItems)
            SetItems();
        meshGenerator.GenerateMesh(mapGenerator.borderedMap);
        flags.InactiveAll();
        if (showFlags)
        {
            //MarkFlagToRooms(map.survivingRooms);
            MarkFlagToWalls(mapGenerator.caveWalls);
        }
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
    /// 给小墙体
    /// </summary>
    public void SetItems()
    {
        for (int i = 0; i < mapGenerator.caveWalls.Count; i++)
            if (!mapGenerator.caveWalls[i].isBorder)
                artItems.Add(caveItemFill.SetMainItems(mapGenerator.caveWalls[i]));
    }

    /// <summary>
    /// 标记房间（平均点）
    /// </summary>
    public void MarkFlagToRooms(List<CaveRoom> rooms)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            //rooms[i].UpdateAverageCoord();
            //Debug.Log(rooms[i].averageCoord.tileX + "  " + rooms[i].averageCoord.tileY);

            flags.GetNextObject(mapGenerator.GetPosition(rooms[i].averageCoord));
        }
    }


    /// <summary>
    /// 标记墙体（平均点）
    /// </summary>
    public void MarkFlagToWalls(List<CaveWall> walls)
    {
        for (int i = 0; i < walls.Count; i++)
        {
            if (walls[i].isBorder)
                continue;
            //walls[i].UpdateVarianceAndDeviation();
            flags.GetNextObject(mapGenerator.GetPosition(walls[i].averageCoord));
        }
    }

    /// <summary>
    /// 清除场景物体
    /// </summary>
    private void CleanArtItem()
    {
        for (int i = 0; i < artItems.Count; i++)
            Destroy(artItems[i]);
        artItems.Clear();
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos || mapGenerator == null)
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
