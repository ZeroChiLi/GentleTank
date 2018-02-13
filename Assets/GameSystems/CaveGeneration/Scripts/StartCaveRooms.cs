using System.Collections.Generic;
using UnityEngine;

public class StartCaveRooms : MonoBehaviour
{
    public MapGenerator map;
    public List<CaveRoomInformation> rooms = new List<CaveRoomInformation>();

    private List<CaveRoom> caveRooms = new List<CaveRoom>();
    private Color green = new Color(0, 1, 0, 0.3f);
    private Color red = new Color(1, 0, 0, 0.4f);

    /// <summary>
    /// 创建所有洞穴房间
    /// </summary>
    public void BuildCaveRooms()
    {
        caveRooms.Clear();
        List<CaveCoord> coords = new List<CaveCoord>();
        for (int i = 0; i < rooms.Count; i++)
        {
            coords.Clear();
            int size = (int)rooms[i].size.x * (int)rooms[i].size.y;
            if (size <= 0)
                continue;
            for (int j = 0; j < rooms[i].size.x; j++)
                for (int k = 0; k < rooms[i].size.y; k++)
                    coords.Add(GetCoord(rooms[i].pos, rooms[i].size, j, k));
            caveRooms[i] = new CaveRoom(coords);
        }
    }

    /// <summary>
    /// 获取地图坐标
    /// </summary>
    public CaveCoord GetCoord(Vector3 center, Vector2 size, int x, int y)
    {
        return new CaveCoord((int)(center.x - size.x / 2 + x + map.width / 2), (int)(center.z - size.y / 2 + y + map.height / 2));
    }

    private void OnDrawGizmos()
    {
        if (map)
        {
            Gizmos.color = green;
            Gizmos.DrawCube(Vector3.zero, new Vector3(map.width, 1, map.height));
        }
        Gizmos.color = red;
        for (int i = 0; i < rooms.Count; i++)
            Gizmos.DrawCube(rooms[i].pos, GameMathf.Vec2ToVec3XZ(rooms[i].size));
    }
}
