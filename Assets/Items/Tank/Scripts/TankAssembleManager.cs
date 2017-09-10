using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TankModule/TankAssemble")]
public class TankAssembleManager : ScriptableObject 
{
    public string tankName = "tank";
    public TankModuleHead head;
    public TankModuleBody body;
    public TankModuleWheel leftWheel;
    public List<TankModuleOther> others;

    private GameObject headObj;
    private GameObject bodyObj;
    private GameObject leftWheelObj;
    private GameObject rightWheelObj;
    private List<GameObject> othersObj;

    /// <summary>
    /// 判断是否合格（可以组装成坦克）
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(tankName) && head && body && leftWheel;
    }

    /// <summary>
    /// 创建坦克对象
    /// </summary>
    /// <returns></returns>
    public GameObject CreateTank()
    {
        if (!IsValid())
        {
            Debug.LogErrorFormat("Cerate Tank Filed. TankName Or Head Or Body Or LeftWheel Should Not Be Null.");
            return null;
        }
        GameObject newTank = new GameObject(tankName);
        InstantiateModules(newTank.transform);
        AssembleTank();
        return newTank;
    }

    /// <summary>
    /// 生成部件实例
    /// </summary>
    /// <param name="parent"></param>
    public void InstantiateModules(Transform parent)
    {
        headObj = Instantiate(head.prefab, parent);
        bodyObj = Instantiate(body.prefab, parent);
        leftWheelObj = Instantiate(leftWheel.prefab, parent);
        rightWheelObj = Instantiate(leftWheel.prefab, parent);
        rightWheelObj.transform.localScale = new Vector3(-rightWheelObj.transform.localScale.x, rightWheelObj.transform.localScale.y, rightWheelObj.transform.localScale.z);
        othersObj = new List<GameObject>();
        if (others != null)
            for (int i = 0; i < others.Count; i++)
                othersObj.Add(Instantiate(others[i].prefab, parent));
    }

    /// <summary>
    /// 连接组合所有部件
    /// </summary>
    public void AssembleTank()
    {
        TankModule.ConnectHeadToBody(head, headObj, body, bodyObj);
        TankModule.ConnectLeftWheelToBody(leftWheel, leftWheelObj, body, bodyObj);
        TankModule.ConnectRightWheelToBody(leftWheel, rightWheelObj, body, bodyObj);
        for (int i = 0; i < othersObj.Count; i++)
            AssembleOtherModule(others[i], othersObj[i]);
    }

    /// <summary>
    /// 组合其他类型部件
    /// </summary>
    /// <param name="module">部件信息</param>
    /// <param name="obj">部件对象</param>
    public void AssembleOtherModule(TankModuleOther module,GameObject obj)
    {
        switch (module.targetType)
        {
            case TankModuleOther.TargetTankModuleType.Head:
                TankModule.ConnectOtherModule(module, obj, head, headObj);
                break;
            case TankModuleOther.TargetTankModuleType.Body:
                TankModule.ConnectOtherModule(module, obj, body, bodyObj);
                break;
            case TankModuleOther.TargetTankModuleType.LeftWheel:
                TankModule.ConnectOtherModule(module, obj, leftWheel, leftWheelObj);
                break;
            case TankModuleOther.TargetTankModuleType.RightWheel:
                TankModule.ConnectOtherModule(module, obj, leftWheel, rightWheelObj);
                break;
            default:
                break;
        }
    }
}
