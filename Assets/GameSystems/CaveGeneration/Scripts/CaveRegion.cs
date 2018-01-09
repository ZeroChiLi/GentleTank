using System.Collections.Generic;
using UnityEngine;

public abstract class CaveRegion 
{
    public CaveCoord averageCoord;                              // 平均点

    public int RegionSize { get { return tiles.Count; } }       // 区域大小
    protected List<CaveCoord> tiles = new List<CaveCoord>();    // 所有坐标

    /// <summary>
    /// 设置坐标列表
    /// </summary>
    public void SetTiles(List<CaveCoord> tiles)
    {
        this.tiles = tiles;
        UpdateAverageCoord();
    }

    /// <summary>
    /// 更新墙体平均点
    /// </summary>
    private void UpdateAverageCoord()
    {
        if (tiles.Count == 0)
        {
            Debug.LogWarning("CaveRegion tiles Count Is Zero.");
            averageCoord = new CaveCoord(0,0);
            return;
        }
        float x = 0, y = 0;
        for (int i = 0; i < tiles.Count; i++)
        {
            x += tiles[i].tileX;
            y += tiles[i].tileY;
        }
        averageCoord = new CaveCoord(Mathf.RoundToInt(x / tiles.Count), Mathf.RoundToInt(y / tiles.Count));
    }
}
