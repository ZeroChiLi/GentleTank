using Item.Tank;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankAssemble")]
public class TankAssembleManager : ScriptableObject
{
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

    public void Clear()
    {
        head = null;
        body = null;
        leftWheel = null;
        cap = null;
        face = null;
        bodyForward = null;
        bodyBack = null;
    }

    /// <summary>
    /// 判断是否合格（可以组装成坦克）
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return head && body && leftWheel;
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
            Debug.LogErrorFormat("Cerate Tank Filed.Head Or Body Or LeftWheel Should Not Be Null.");
            return null;
        }
        GameObject newTank = new GameObject(name);
        newTank.transform.SetParent(parent);
        InstantiateModules(newTank.transform);
        AssembleTank();
        return newTank;
    }

    /// <summary>
    /// 初始化坦克组件属性
    /// </summary>
    /// <param name="tank">目标坦克</param>
    public void InitTankComponents(TankManager tank)
    {
        if (tank == null)
            return;
        InitTankAttack(tank, head);
        InitTankHealth(tank, body.healthProperties);
        InitTankMove(tank, leftWheel.moveProperties);
        EncapsulateBodyAndWheel(ref tank.boxCollider);
        AdjustHeadToBody(tank.boxCollider.bounds);
    }

    /// <summary>
    /// 初始化坦克攻击组件
    /// </summary>
    private void InitTankAttack(TankManager tank, TankModuleHead head)
    {
        TankModuleHead.AttackProperties properties = head.attackProperties;
        //tank.tankAttack = tank.gameObject.AddComponent(properties.attackScript.GetClass()) as TankAttack;
        tank.tankAttack = tank.GetComponentInChildren<TankAttack>();
        if (tank.tankAttack == null)
        {
            Debug.LogError("Tank Head Need 'TankAttack' Component!");
            return;
        }
        tank.stateController.attackManager = tank.tankAttack;
        tank.tankAttack.forceSlider = tank.GetComponent<TankManager>().aimSlider;
        tank.tankAttack.chargingClip = properties.chargingClip;
        tank.tankAttack.fireClip = properties.fireClip;
        tank.tankAttack.ResetSliderValue(properties.minLaunchForce, properties.maxLaunchForce, properties.maxChargeTime);
        tank.tankAttack.damage = properties.damage;
        tank.tankAttack.coolDownTime = properties.coolDownTime;
        tank.tankAttack.CDTimer.Reset(properties.coolDownTime);
        System.Type type = tank.tankAttack.GetType();
        if (type == typeof(TankAttackShooting))
        {
            TankAttackShooting attack = tank.tankAttack as TankAttackShooting;
            attack.ammoPool = head.attackProperties.ammoPool;
            attack.ammoSpawnPoint = new Point(head.ammoSpawnPoint);
        }
        else if (type == typeof(TankAttackBoxing))
        {
            TankAttackBoxing attack = tank.tankAttack as TankAttackBoxing;
            attack.springBoxingGlove = headObj.GetComponentInChildren<SpringBoxingGloveManager>();
            attack.launchDistance = new AnimationCurve(new Keyframe(0, 0, 0, 3.5f), new Keyframe(0.3f, 1, 0, 0));
        }
    }

    /// <summary>
    /// 初始化坦克移动组件
    /// </summary>
    private void InitTankMove(TankManager tank,TankModuleWheel.MoveProperties properties)
    {
        tank.tankMovement.speed = properties.speed;
        tank.tankMovement.turnSpeed = properties.turnSpeed;
    }

    /// <summary>
    /// 初始化坦克生命值组件
    /// </summary>
    private void InitTankHealth(TankManager tank, TankModuleBody.HealthProperties properties)
    {
        tank.tankHealth.maxHealth = properties.maxHealth;
    }

    /// <summary>
    /// 包装身体部件和轮子部件，重新计算AABB。删除子BoxCollider
    /// </summary>
    /// <param name="collider"></param>
    private void EncapsulateBodyAndWheel(ref BoxCollider collider)
    {
        Bounds bounds = new Bounds();
        bounds = bodyObj.GetComponent<MeshRenderer>().bounds;
        bounds.Encapsulate(leftWheelObj.GetComponent<BoxCollider>().bounds);
        bounds.Encapsulate(rightWheelObj.GetComponent<BoxCollider>().bounds);
        collider.center = bounds.center;
        collider.size = bounds.size;
        ComponentUtility.DestroyIfExist<BoxCollider>(bodyObj);
        ComponentUtility.DestroyIfExist<BoxCollider>(leftWheelObj);
        ComponentUtility.DestroyIfExist<BoxCollider>(rightWheelObj);
    }

    /// <summary>
    /// 调整头部部件碰撞体高度，避免和身体一直相交。
    /// </summary>
    /// <param name="body"></param>
    private void AdjustHeadToBody(Bounds body)
    {
        BoxCollider head = headObj.GetComponent<BoxCollider>();
        head.tag = "Player";
        float diff = body.max.y - head.bounds.min.y;
        if (diff < 0.05f)
            return;
        head.center += new Vector3(0, diff / 2f + 0.05f, 0);
        head.size -= new Vector3(0, diff, 0);
    }

}
