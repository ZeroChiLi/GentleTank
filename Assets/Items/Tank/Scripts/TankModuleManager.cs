using UnityEngine;

public class TankModuleManager : MonoBehaviour 
{
    public GameObject head;             // 头 
    public GameObject body;             // 身体
    public GameObject wheelLeft;        // 左车轮
    public GameObject wheelRight;       // 右车轮

    /// <summary>
    /// 配置坦克
    /// </summary>
    /// <param name="head">头</param>
    /// <param name="body">身体</param>
    /// <param name="wheelLeft">左车轮</param>
    /// <param name="wheelRight">右车轮</param>
    public void Setup(GameObject head, GameObject body, GameObject wheelLeft, GameObject wheelRight)
    {
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
