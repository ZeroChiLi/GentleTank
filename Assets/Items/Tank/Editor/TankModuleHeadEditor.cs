using UnityEditor;

[CustomEditor(typeof(TankModuleHead))]
public class TankModuleHeadEditor : TankModuleEditor
{
    private TankModuleHead head;

    public override void SetDefaultValue()
    {
        base.SetDefaultValue();
        head = target as TankModuleHead;
        head.launchPos = head.forward;
    }
}
