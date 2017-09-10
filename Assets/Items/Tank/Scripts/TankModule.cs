using UnityEngine;

[CreateAssetMenu(menuName = "TankModule/Default")]
public class TankModule : ScriptableObject
{
    public enum TankModuleType
    {
        None,Default,Head, Body, Wheel, Other
    }

    public GameObject prefab;
    public Sprite preview;
    public Vector3 center;
    public Vector3 forward;
    public Vector3 back;
    public Vector3 left;
    public Vector3 right;
    public Vector3 up;
    public Vector3 down;

    /// <summary>
    /// 获取部件类型枚举值
    /// </summary>
    /// <param name="module">目标部件</param>
    /// <returns>目标部件的类型枚举值</returns>
    static public TankModuleType GetModuleType(TankModule module)
    {
        if (module.GetType() == typeof(TankModuleHead))
            return TankModuleType.Head;
        else if (module.GetType() == typeof(TankModuleBody))
            return TankModuleType.Body;
        else if (module.GetType() == typeof(TankModuleWheel))
            return TankModuleType.Wheel;
        else if (module.GetType() == typeof(TankModuleOther))
            return TankModuleType.Other;
        else if (module.GetType() == typeof(TankModule))
            return TankModuleType.Default;
        return TankModuleType.None;
    }

    /// <summary>
    /// 连接头到身体
    /// </summary>
    /// <param name="head">头部件</param>
    /// <param name="headObj">头部件对象</param>
    /// <param name="body">身体部件</param>
    /// <param name="bodyObj">身体部件对象</param>
    static public void ConnectHeadToBody(TankModuleHead head,GameObject headObj,TankModuleBody body,GameObject bodyObj)
    {
        headObj.transform.position = bodyObj.transform.position + body.up - head.down;
    }

    /// <summary>
    /// 连接左轮胎到身体
    /// </summary>
    /// <param name="leftWheel">左轮胎</param>
    /// <param name="leftWheelObj">左轮胎对象</param>
    /// <param name="body">身体</param>
    /// <param name="bodyObj">身体对象</param>
    static public void ConnectLeftWheelToBody(TankModuleWheel leftWheel, GameObject leftWheelObj, TankModuleBody body, GameObject bodyObj)
    {
        leftWheelObj.transform.position = bodyObj.transform.position + body.leftWheelTop - leftWheel.up;
    }

    /// <summary>
    /// 连接右轮胎到身体
    /// </summary>
    /// <param name="rightWheel">右轮胎</param>
    /// <param name="rightObj">右轮胎对象</param>
    /// <param name="body">身体</param>
    /// <param name="bodyObj">身体对象</param>
    static public void ConnectRightWheelToBody(TankModuleWheel rightWheel, GameObject rightObj, TankModuleBody body, GameObject bodyObj)
    {
        if (rightWheel.wheelType == TankModuleWheel.WheelType.Left)
            rightObj.transform.position = bodyObj.transform.position + body.rightWheelTop - new Vector3(-rightWheel.up.x,rightWheel.up.y,rightWheel.up.z);
        else
            rightObj.transform.position = bodyObj.transform.position + body.rightWheelTop - rightWheel.up;
    }
    
    /// <summary>
    /// 连接其他类型的部件
    /// </summary>
    /// <param name="other">其他部件</param>
    /// <param name="obj">部件对象</param>
    /// <param name="targetModule">连接的目标部件</param>
    /// <param name="targetObj">连接的目标部件对象</param>
    static public void ConnectOtherModule(TankModuleOther other,GameObject obj,TankModule targetModule,GameObject targetObj)
    {
        obj.transform.position = targetObj.transform.position + other.GetAnchor(targetModule) - other.connectAnchor;
    }

}
