using System.Collections.Generic;

public class CaveWall : CaveRegion
{
    public bool isBorder;

    public CaveWall(List<CaveCoord> tiles)
    {
        SetTiles(tiles);
    }
}
