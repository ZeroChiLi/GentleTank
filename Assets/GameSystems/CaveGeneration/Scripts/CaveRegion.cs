using System.Collections.Generic;
using UnityEngine;

public class CaveRegion
{
    public int RegionSize { get { return tiles.Count; } }       // 区域大小
    protected List<CaveCoord> tiles = new List<CaveCoord>();    // 所有坐标

    public CaveCoord averageCoord;                              // 平均点
    public Vector2 variance;                                    // 所有点的方差
    public Vector2 deviation;                                   // 所有点的标准差
    public Vector2 NormalizedDeviation { get { return deviation.normalized; } }  // 归一化的标准差

    public CaveCoord this[int index] { get { return tiles[index]; } }
    public int Count { get { return tiles.Count; } }

    public CaveRegion() { }

    public CaveRegion(List<CaveCoord> tiles)
    {
        SetTiles(tiles);
    }
    /// <summary>
    /// 设置坐标列表
    /// </summary>
    public void SetTiles(List<CaveCoord> tiles)
    {
        this.tiles = tiles;
        UpdateAverageCoord();
        UpdateVarianceAndDeviation();
    }

    /// <summary>
    /// 更新区域平均点
    /// </summary>
    private void UpdateAverageCoord()
    {
        if (tiles.Count == 0)
        {
            Debug.LogWarning("CaveRegion tiles Count Is Zero.");
            averageCoord = new CaveCoord(0, 0);
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

    /// <summary>
    /// 更新区域所有点的方差和标准差，需要确保更新了平均点
    /// </summary>
    private void UpdateVarianceAndDeviation()
    {
        float x = 0, y = 0;

        for (int i = 0; i < tiles.Count; i++)
        {
            x += GameMathf.Pow2(tiles[i].tileX - averageCoord.tileX);
            y += GameMathf.Pow2(tiles[i].tileY - averageCoord.tileY);
        }
        variance = new Vector2(x / tiles.Count, y / tiles.Count);
        deviation.x = Mathf.Sqrt(variance.x);
        deviation.y = Mathf.Sqrt(variance.y);
    }

    /// <summary>
    /// 获取区域内随机坐标
    /// </summary>
    public CaveCoord GetRandomCoord()
    {
        return tiles[Random.Range(0, tiles.Count)];
    }
}
