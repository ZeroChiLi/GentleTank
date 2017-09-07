using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TankModule))]
public class TankAssembleEditor : Editor 
{
    public TankModule tankModule;

    private Bounds moduleBounds;

    private void OnEnable()
    {
        tankModule = target as TankModule;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SetDefaultValueButton();
    }
    
    /// <summary>
    /// 通过预设来获取物体各基本点位置
    /// </summary>
    public void SetDefaultValueButton()
    {
        if (tankModule.prefab == null || !GUILayout.Button("Set Default Value"))
            return;
        moduleBounds = tankModule.prefab.GetComponent<MeshFilter>().sharedMesh.bounds;
        tankModule.center = GameMathf.CeilZeroWithPrecision(moduleBounds.center);
        tankModule.forward = tankModule.center + new Vector3(0,0,moduleBounds.extents.z);
        tankModule.back = tankModule.center + new Vector3(0, 0, -moduleBounds.extents.z);
        tankModule.left = tankModule.center + new Vector3(-moduleBounds.extents.x, 0, 0);
        tankModule.right = tankModule.center + new Vector3(moduleBounds.extents.x, 0, 0);
        tankModule.up = tankModule.center + new Vector3(0, moduleBounds.extents.y, 0);
        tankModule.down = tankModule.center + new Vector3(0, -moduleBounds.extents.y, 0);
        Debug.Log(moduleBounds.center);
        Debug.Log(moduleBounds.size);
        Debug.Log(moduleBounds.extents);
    }

}
