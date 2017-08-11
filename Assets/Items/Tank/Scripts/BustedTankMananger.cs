using UnityEngine;

public class BustedTankMananger : MonoBehaviour 
{
    [Range(0,1)]
    public float colorLerp = 0.5f;                                  // 颜色插值（默认残骸颜色到手动设置颜色插值）
    public Color bustedColor = new Color(0.286f, 0.286f, 0.286f,1); // 默认残骸颜色

    /// <summary>
    /// 配置坦克残骸位置和颜色插值
    /// </summary>
    /// <param name="transform">位置，角度</param>
    /// <param name="color">颜色</param>
    public void SetupBustedTank(Transform transform,Color color)
    {
        this.transform.position = transform.position;
        this.transform.rotation = transform.rotation;
        GetComponent<MeshRenderer>().material.color = Color.Lerp(bustedColor,color,colorLerp);
    }
}
