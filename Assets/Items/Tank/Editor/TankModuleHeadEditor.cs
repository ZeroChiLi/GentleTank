using Item.Tank;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TankModuleHead))]
public class TankModuleHeadEditor : ModuleEditor
{
    private TankModuleHead head { get { return target as TankModuleHead; } }

    public override void SetDefaultValue()
    {
        base.SetDefaultValue();
        //head = target as TankModuleHead;
        head.forwardUp = new Vector3(head.anchors.forward.x,head.anchors.up.y,head.anchors.forward.z);
        head.backUp = new Vector3(head.anchors.back.x, head.anchors.up.y, head.anchors.back.z);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (head == null || head.attackProperties == null)
            return;
        if (head.attackProperties.attackScript != null && head.attackProperties.attackScript.GetClass() == typeof(TankAttackShooting))
            head.attackProperties.ammoPool = EditorGUILayout.ObjectField("Ammo Object Pool", head.attackProperties.ammoPool, typeof(ObjectPool), false) as ObjectPool;
        EditorUtility.SetDirty(target);
    }

}
