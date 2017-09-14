using UnityEngine;

[CreateAssetMenu(menuName = "TankModule/Other")]
public class TankModuleOther : TankModule
{
    public enum TargetTankModuleType
    {
        Head, Body, LeftWheel, RightWheel
    }

    public enum TargetPos
    {
        Center, Forward, Back, Left, Right, Up, Down, HeadLuanch, BodyLeftWheelTop, BodyRightWheelTop
    }

    public Vector3 connectAnchor;
    public TargetTankModuleType targetType;
    public TargetPos targetPos;

    /// <summary>
    /// 获取位置
    /// </summary>
    /// <param name="module">部件</param>
    /// <param name="anchorPos">锚点</param>
    /// <returns>获取的位置值</returns>
    public Vector3 GetAnchor(TankModule module)
    {
        TankModuleHead head = module as TankModuleHead;
        TankModuleBody body = module as TankModuleBody;
        switch (targetPos)
        {
            case TargetPos.Center:
                return module.anchors.center;
            case TargetPos.Forward:
                return module.anchors.forward;
            case TargetPos.Back:
                return module.anchors.back;
            case TargetPos.Left:
                return module.anchors.left;
            case TargetPos.Right:
                return module.anchors.right;
            case TargetPos.Up:
                return module.anchors.up;
            case TargetPos.Down:
                return module.anchors.down;
            case TargetPos.HeadLuanch:
                return head == null ? Vector3.zero : head.launchPos;
            case TargetPos.BodyLeftWheelTop:
                return body == null ? Vector3.zero : body.leftWheelTop;
            case TargetPos.BodyRightWheelTop:
                return body == null ? Vector3.zero : body.rightWheelTop;
        }
        return Vector3.zero;
    }

}
