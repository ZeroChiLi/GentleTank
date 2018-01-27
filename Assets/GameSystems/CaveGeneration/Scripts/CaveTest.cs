using System.Collections.Generic;
using UnityEngine;

public class CaveTest : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public MeshGenerator meshGenerator;
    public CaveItemFillManager caveItemFill;
    public ObjectPool flags;
    public bool cleanInnerWalls;
    public bool showGizmos;

    private Vector3 offset;

    private void Start()
    {
        ReBuildMap();
        //caveItemFill.groundItemList.UpdateAscendingItemList();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ReBuildMap();
    }

    public void ReBuildMap()
    {
        mapGenerator.GenerateMap();
        if (cleanInnerWalls)
            CleanInnerWallRegion();
        meshGenerator.GenerateMesh(mapGenerator.borderedMap);
        offset = new Vector3((1 - mapGenerator.width) / 2, 0, (1 - mapGenerator.height) / 2);
        flags.InactiveAll();
        //MarkFlagToRooms(map.survivingRooms);
        MarkFlagToWalls(mapGenerator.survivingWalls);
    }

    /// <summary>
    /// 清除所有内部墙体
    /// </summary>
    public void CleanInnerWallRegion()
    {
        for (int i = 0; i < mapGenerator.survivingWalls.Count; i++)
        {
            if (!mapGenerator.survivingWalls[i].isBorder)
                mapGenerator.SetRegion(mapGenerator.survivingWalls[i], TileType.Empty);
        }
    }

    public void MarkFlagToRooms(List<CaveRoom> rooms)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            //rooms[i].UpdateAverageCoord();
            //Debug.Log(rooms[i].averageCoord.tileX + "  " + rooms[i].averageCoord.tileY);

            flags.GetNextObject(new Vector3(-mapGenerator.width / 2 + rooms[i].averageCoord.tileX + 1f / 2f, 0, -mapGenerator.height / 2 + rooms[i].averageCoord.tileY + 1f / 2f));
        }
    }

    public void MarkFlagToWalls(List<CaveWall> walls)
    {
        for (int i = 0; i < walls.Count; i++)
        {
            if (walls[i].isBorder)
                continue;
            //walls[i].UpdateVarianceAndDeviation();
            flags.GetNextObject(offset + new Vector3(walls[i].averageCoord.tileX, 0, walls[i].averageCoord.tileY));
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos || mapGenerator == null)
            return;

        Gizmos.color = Color.red;
        for (int i = 0; i < mapGenerator.survivingWalls.Count; i++)
        {
            if (mapGenerator.survivingWalls[i].isBorder)
                continue;
            Vector3 pos = offset + new Vector3(mapGenerator.survivingWalls[i].averageCoord.tileX, 0, mapGenerator.survivingWalls[i].averageCoord.tileY);
            Vector3 len = new Vector3(mapGenerator.survivingWalls[i].deviation.x, 0, mapGenerator.survivingWalls[i].deviation.y);
            Gizmos.DrawLine(pos - len / 2, pos + len / 2);
        }
    }
}
