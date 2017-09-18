using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TankModuleHead))]
public class TankModuleHeadEditor : ModuleEditor
{
    private TankModuleHead head;

    public override void SetDefaultValue()
    {
        base.SetDefaultValue();
        head = target as TankModuleHead;
        head.launchPos = head.anchors.forward;
        head.forwardUp = new Vector3(head.anchors.forward.x,head.anchors.up.y,head.anchors.forward.z);
        head.backUp = new Vector3(head.anchors.back.x, head.anchors.up.y, head.anchors.back.z);
    }
}
