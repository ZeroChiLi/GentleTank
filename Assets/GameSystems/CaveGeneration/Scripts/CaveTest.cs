using System.Collections.Generic;
using UnityEngine;

public class CaveTest : MonoBehaviour 
{
    public MapGenerator map;
    public ObjectPool flags;

    private void Start()
    {
        ReBuildMap();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ReBuildMap();
    }

    public void ReBuildMap()
    {
        map.GenerateMap();
        flags.InactiveAll();
        //MarkFlagToRooms(map.survivingRooms);
        MarkFlagToWalls(map.survivingWalls);
    }

    public void MarkFlagToRooms(List<CaveRoom> rooms)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            //rooms[i].UpdateAverageCoord();
            //Debug.Log(rooms[i].averageCoord.tileX + "  " + rooms[i].averageCoord.tileY);

            flags.GetNextObject(true, new Vector3(-map.width / 2 +rooms[i].averageCoord.tileX + 1f/2f,0, -map.height / 2 + rooms[i].averageCoord.tileY + 1f / 2f));
        }
    }

    public void MarkFlagToWalls(List<CaveWall> walls)
    {
        for (int i = 0; i < walls.Count; i++)
        {
            if (!walls[i].isBorder)
                flags.GetNextObject(true, new Vector3(-map.width / 2 + walls[i].averageCoord.tileX + 1f / 2f, 0, -map.height / 2 + walls[i].averageCoord.tileY + 1f / 2f));
        }
    }
}
