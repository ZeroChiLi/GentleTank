using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TankModuleBody))]
public class TankModuleBodyEditor : ModuleEditor 
{
    private TankModuleBody body;

    public override void SetDefaultValue()
    {
        base.SetDefaultValue();
        body = target as TankModuleBody;
        body.leftWheelTop = GameMathf.Round(moduleBounds.center + new Vector3(-moduleBounds.extents.x, -moduleBounds.extents.y, 0));
        body.rightWheelTop = GameMathf.Round(moduleBounds.center + new Vector3(moduleBounds.extents.x, -moduleBounds.extents.y, 0));
        body.raycastPos = body.anchors.forward;
        body.forwardUp = new Vector3(body.anchors.forward.x, body.anchors.up.y, body.anchors.forward.z);
        body.backUp = new Vector3(body.anchors.back.x, body.anchors.up.y, body.anchors.back.z);
    }
}
