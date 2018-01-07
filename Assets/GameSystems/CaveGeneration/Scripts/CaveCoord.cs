
public struct CaveCoord
{
    public int tileX;
    public int tileY;

    public CaveCoord(int x, int y)
    {
        tileX = x;
        tileY = y;
    }

    /// <summary>
    /// 两坐标之间平方之和
    /// </summary>
    public float SqrMagnitude(CaveCoord coordB)
    {
        return (tileX - coordB.tileX) * (tileX - coordB.tileX) + (tileY - coordB.tileY) * (tileY - coordB.tileY);
    }

} 
