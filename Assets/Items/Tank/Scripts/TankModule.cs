using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Module/TankModule/Default")]
public class TankModule : ModuleBase
{
    public enum TankModuleType
    {
        None, Default, Head, Body, Wheel, Other, Cap, Face, BodyForward, BodyBack
    }

    public TankModuleProperty property;

    public override string GetProperties()
    {
        return property.GetPropertiesString();
    }

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
        else if (module.GetType() == typeof(TankModuleCap))
            return TankModuleType.Cap;
        else if (module.GetType() == typeof(TankModuleFace))
            return TankModuleType.Face;
        else if (module.GetType() == typeof(TankModuleBodyForward))
            return TankModuleType.BodyForward;
        else if (module.GetType() == typeof(TankModuleBodyBack))
            return TankModuleType.BodyBack;
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
        headObj.transform.position = bodyObj.transform.position + body.anchors.up - head.anchors.down;
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
        leftWheelObj.transform.position = bodyObj.transform.position + body.leftWheelTop - leftWheel.anchors.up;
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
            rightObj.transform.position = bodyObj.transform.position + body.rightWheelTop - new Vector3(-rightWheel.anchors.up.x,rightWheel.anchors.up.y,rightWheel.anchors.up.z);
        else
            rightObj.transform.position = bodyObj.transform.position + body.rightWheelTop - rightWheel.anchors.up;
    }

    /// <summary>
    /// 连接装饰部件
    /// </summary>
    /// <param name="decoration">装饰部件</param>
    /// <param name="decorationObj">装饰部件对象</param>
    /// <param name="targetModule">连接目标部件</param>
    /// <param name="targetObj">连接目标部件对象</param>
    static public void ConnectDecorationModule(TankModuleDecoration decoration,GameObject decorationObj, TankModule targetModule,GameObject targetObj)
    {
        decorationObj.transform.SetParent(targetObj.transform);
        decorationObj.transform.position = targetObj.transform.position + decoration.GetTargetAnchor(targetModule) - decoration.connectAnchor;
    }

}
