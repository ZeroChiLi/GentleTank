using System.Collections.Generic;
using UnityEngine;

public class CaveTest : MonoBehaviour 
{
    public MapGenerator map;
    public ObjectPool flags;

    private void Start()
    {
        map.GenerateMap();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            map.GenerateMap();
            flags.InactiveAll();
            MarkFlagToRooms(map.survivingRooms);
        }
    }

    public void MarkFlagToRooms(List<CaveRoom> rooms)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].UpdateAverageCoord();
            //Debug.Log(rooms[i].averageCoord.tileX + "  " + rooms[i].averageCoord.tileY);

            flags.GetNextObject(true, new Vector3(-map.width / 2 +rooms[i].averageCoord.tileX + 1f/2f,0, -map.height / 2 + rooms[i].averageCoord.tileY + 1f / 2f));
        }
    }
}
