using System.Collections.Generic;
using UnityEngine;

public class CaveWall 
{
    public List<CaveCoord> tiles;                       //所有坐标。
    public int roomSize;                                //就是tiles.Count。
    public CaveCoord averageCoord;                      //平均点
    public bool isBorder;

    public CaveWall() { }

    public CaveWall(List<CaveCoord> roomTiles)
    {
        tiles = roomTiles;
        roomSize = tiles.Count;
    }

    // 更新墙体平均点
    public void UpdateAverageCoord()
    {
        if (tiles == null || tiles.Count == 0)
            return;
        float x = 0, y = 0;
        for (int i = 0; i < tiles.Count; i++)
        {
            x += tiles[i].tileX;
            y += tiles[i].tileY;
        }
        averageCoord = new CaveCoord(Mathf.RoundToInt(x / tiles.Count), Mathf.RoundToInt(y / tiles.Count));
    }
}
