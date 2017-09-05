using UnityEngine;

public class TankModuleManager : MonoBehaviour 
{
    public TankModule headModule;       // 头部部件
    public TankModule bodyModule;       // 身体部件
    public TankModule wheelLeftModule;  // 左车轮部件

    public GameObject head;             // 头 
    public GameObject body;             // 身体
    public GameObject wheelLeft;        // 左车轮
    public GameObject wheelRight;       // 右车轮

    /// <summary>
    /// 配置坦克
    /// </summary>
    /// <param name="headModule">头部部件</param>
    /// <param name="bodyModule">身体部件</param>
    /// <param name="wheelLeftModule">左车轮部件</param>
    /// <param name="head">头</param>
    /// <param name="body">身体</param>
    /// <param name="wheelLeft">左车轮</param>
    /// <param name="wheelRight">右车轮</param>
    public void Setup(TankModule headModule, TankModule bodyModule, TankModule wheelLeftModule, GameObject head, GameObject body, GameObject wheelLeft, GameObject wheelRight)
    {
        this.headModule = headModule;
        this.bodyModule = bodyModule;
        this.wheelLeftModule = wheelLeftModule;
        this.head = head;
        this.body = body;
        this.wheelLeft = wheelLeft;
        this.wheelRight = wheelRight;
    }

    /// <summary>
    /// 是否有效（满足基本部件）
    /// </summary>
    /// <returns>是否有效</returns>
    public bool IsValid()
    {
        return head && body && wheelLeft && wheelRight;
    }
}
