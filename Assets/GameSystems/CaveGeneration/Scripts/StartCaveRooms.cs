using System.Collections.Generic;
using UnityEngine;

public class StartCaveRooms : MonoBehaviour
{
    public MapGenerator map;
    public List<CaveRoomEditor> rooms = new List<CaveRoomEditor>();

    private List<CaveRoom> caveRooms;
    private Color green = new Color(0, 1, 0, 0.3f);
    private Color red = new Color(1, 0, 0, 0.4f);

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
