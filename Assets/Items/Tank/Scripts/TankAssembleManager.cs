using Item.Tank;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankAssemble")]
public class TankAssembleManager : ScriptableObject
{
    public string tankName = "tank";
    public TankModuleHead head;
    public TankModuleBody body;
    public TankModuleWheel leftWheel;
    public TankModuleCap cap;
    public TankModuleFace face;
    public TankModuleBodyForward bodyForward;
    public TankModuleBodyBack bodyBack;

    private GameObject headObj;
    private GameObject bodyObj;
    private GameObject leftWheelObj;
    private GameObject rightWheelObj;
    private GameObject capObj;
    private GameObject faceObj;
    private GameObject bodyForwardObj;
    private GameObject bodyBackObj;

    private float totalWeight;

    /// <summary>
    /// 复制坦克部件管理器
    /// </summary>
    /// <param name="copySrc">复制源</param>
    public void CopyFrom(TankAssembleManager copySrc)
    {
        head = copySrc.head;
        body = copySrc.body;
        leftWheel = copySrc.leftWheel;
        cap = copySrc.cap;
        face = copySrc.face;
        bodyForward = copySrc.bodyForward;
        bodyBack = copySrc.bodyBack;
    }

    /// <summary>
    /// 判断是否合格（可以组装成坦克）
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(tankName) && head && body && leftWheel;
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
        if (cap != null)
            capObj = Instantiate(cap.prefab, headObj.transform);
        if (face != null)
            faceObj = Instantiate(face.prefab, headObj.transform);
        if (bodyForward != null)
            bodyForwardObj = Instantiate(bodyForward.prefab, bodyObj.transform);
        if (bodyBack != null)
            bodyBackObj = Instantiate(bodyBack.prefab, bodyObj.transform);
    }

    /// <summary>
    /// 连接组合所有部件
    /// </summary>
    public void AssembleTank()
    {
        TankModule.ConnectHeadToBody(head, headObj, body, bodyObj);
        TankModule.ConnectLeftWheelToBody(leftWheel, leftWheelObj, body, bodyObj);
        TankModule.ConnectRightWheelToBody(leftWheel, rightWheelObj, body, bodyObj);

        if (cap != null)
            TankModule.ConnectDecorationModule(cap, capObj, head, headObj);
        if (face != null)
            TankModule.ConnectDecorationModule(face, faceObj, head, headObj);
        if (bodyForward != null)
            TankModule.ConnectDecorationModule(bodyForward, bodyForwardObj, body, bodyObj);
        if (bodyBack != null)
            TankModule.ConnectDecorationModule(bodyBack, bodyBackObj, body, bodyObj);

    }

    /// <summary>
    /// 获取总重量
    /// </summary>
    /// <returns>总重量</returns>
    public float GetTotalWeight()
    {
        totalWeight = head.property.weight + body.property.weight + leftWheel.property.weight;
        totalWeight += cap == null ? 0 : cap.property.weight;
        totalWeight += face == null ? 0 : face.property.weight;
        totalWeight += bodyForward == null ? 0 : bodyForward.property.weight;
        totalWeight += bodyBack == null ? 0 : bodyBack.property.weight;
        return totalWeight;
    }

    /// <summary>
    /// 创建坦克对象
    /// </summary>
    /// <returns></returns>
    public GameObject CreateTank(Transform parent)
    {
        if (!IsValid())
        {
            Debug.LogErrorFormat("Cerate Tank Filed. TankName Or Head Or Body Or LeftWheel Should Not Be Null.");
            return null;
        }
        GameObject newTank = new GameObject(tankName);
        newTank.transform.SetParent(parent);
        InstantiateModules(newTank.transform);
        AssembleTank();
        return newTank;
    }

    public void InitTankComponents(TankManager tank)
    {
        if (tank == null)
            return;
        tank.tankAttack = tank.gameObject.AddComponent(MasterManager.Instance.SelectedTank.head.attackScript.GetClass()) as TankAttack;
        tank.tankAttack.forceSlider = tank.GetComponent<TankManager>().aimSlider;
        tank.tankAttack.chargingClip = MasterManager.Instance.SelectedTank.head.chargingClip;
        tank.tankAttack.fireClip = MasterManager.Instance.SelectedTank.head.fireClip;
        System.Type type = tank.tankAttack.GetType();
        if (type == typeof(TankAttackShooting))
        {
            TankAttackShooting attack = tank.tankAttack as TankAttackShooting;
            attack.ammoPool = head.ammoPool;
            attack.ammoSpawn = tank.ammoSpawn;
        }
        else if (type == typeof(TankAttackBoxing))
        {
            TankAttackBoxing attack = tank.tankAttack as TankAttackBoxing;
            attack.springBoxingGlove = headObj.GetComponentInChildren<SpringBoxingGloveManager>();
        }
    }

}
