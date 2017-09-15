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
        Center, Forward, Back, Left, Right, Up, Down, HeadLuanch, HeadForwardUp, HeadBackUp, BodyLeftWheelTop, BodyRightWheelTop, BodyForwadUp,BodyBackUp
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
        if (head != null)
            return GetHeadAnchor(head);
        TankModuleBody body = module as TankModuleBody;
        if (body != null)
            return GetBodyAnchor(body);

        return GetDefaultAnchor(module);
    }

    public Vector3 GetDefaultAnchor(TankModule module)
    {
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
        }
        return Vector3.zero;
    }

    public Vector3 GetHeadAnchor(TankModuleHead head)
    {
        switch (targetPos)
        {
            case TargetPos.HeadLuanch:
                return head.launchPos;
            case TargetPos.HeadForwardUp:
                return head.forwardUp;
            case TargetPos.HeadBackUp:
                return head.backUp;
        }
        return GetDefaultAnchor(head);
    }

    public Vector3 GetBodyAnchor(TankModuleBody body)
    {
        switch (targetPos)
        {
            case TargetPos.BodyLeftWheelTop:
                return body.leftWheelTop;
            case TargetPos.BodyRightWheelTop:
                return body.rightWheelTop;
            case TargetPos.BodyForwadUp:
                return body.forwardUp;
            case TargetPos.BodyBackUp:
                return body.backUp;
        }
        return GetDefaultAnchor(body);
    }

}
