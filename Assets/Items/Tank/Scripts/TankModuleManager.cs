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

    private TankModule temModule;
    private GameObject temObj;
    private GameObject previewObj;

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

    /// <summary>
    /// 通过类型获取坦克部件
    /// </summary>
    /// <param name="type">部件类型</param>
    /// <returns></returns>
    public TankModule GetTankModule(TankModuleType type)
    {
        switch (type)
        {
            case TankModuleType.Head:
                return headModule;
            case TankModuleType.Body:
                return bodyModule;
            case TankModuleType.WheelLeft:
                return wheelLeftModule;
            case TankModuleType.WheelRight:
                break;
            case TankModuleType.Other:
                break;
            case TankModuleType.Skin:
                break;
        }
        return null;
    }

    /// <summary>
    /// 获取坦克部件对象
    /// </summary>
    /// <param name="type">部件类型</param>
    /// <returns>部件对象</returns>
    public GameObject GetTankModuleObj(TankModuleType type)
    {
        switch (type)
        {
            case TankModuleType.Head:
                return head;
            case TankModuleType.Body:
                return body;
            case TankModuleType.WheelLeft:
                return wheelLeft;
            case TankModuleType.WheelRight:
                return wheelRight;
            case TankModuleType.Other:
                break;
            case TankModuleType.Skin:
                break;
        }
        return null;
    }

    /// <summary>
    /// 连接部件
    /// </summary>
    /// <param name="obj">部件对象</param>
    /// <param name="module">部件信息</param>
    /// <returns>是否连接成功</returns>
    public bool ConnectModuleObj(GameObject obj, TankModule module)
    {
        switch (module.type)
        {
            case TankModuleType.Head:
                if (bodyModule[TankModuleType.Head] != null && module[TankModuleType.Body] != null)
                    obj.transform.localPosition = bodyModule[TankModuleType.Head].anchor - module[TankModuleType.Body].anchor;
                return true;
            case TankModuleType.Body:
                //if (module[TankModuleType.Head] != null && headModule[TankModuleType.Body] != null)
                //    obj.transform.localPosition = headModule[TankModuleType.Body].anchor - module[TankModuleType.Head].anchor;
                return true;
            case TankModuleType.WheelLeft:
                if (module[TankModuleType.Body] != null && bodyModule[TankModuleType.WheelLeft] != null)
                    obj.transform.localPosition = bodyModule[TankModuleType.WheelLeft].anchor - module[TankModuleType.Body].anchor;
                return true;
            case TankModuleType.WheelRight:
                break;
            case TankModuleType.Other:
                break;
            case TankModuleType.Skin:
                break;
            default:
                break;
        }
        return false;
    }

    /// <summary>
    /// 预览组合部件
    /// </summary>
    /// <param name="module">部件</param>
    public void PreviewModule(TankModule module)
    {
        if (previewObj != null)
            Destroy(previewObj);
        temModule = GetTankModule(module.type);
        if (temModule == null)
            return;
        temObj = GetTankModuleObj(module.type);
        if (temObj != null)
            temObj.SetActive(false);
        previewObj = Instantiate(module.prefab, transform);
        if (!ConnectModuleObj(previewObj, temModule))
        {
            Destroy(previewObj);
            Debug.Log("Failed");
        }
    }
}
