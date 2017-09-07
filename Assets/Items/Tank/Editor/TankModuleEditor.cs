using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TankModule))]
public class TankModuleEditor : Editor 
{
    public TankModule tankModule;

    private Bounds moduleBounds;

    public override void OnInspectorGUI()
    {
        if (tankModule != target as TankModule)
        {
            tankModule = target as TankModule;
            SetDefaultValue();
        }
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
        SetDefaultValue();
    }

    /// <summary>
    /// 设置默认值
    /// </summary>
    public void SetDefaultValue()
    {
        moduleBounds = tankModule.prefab.GetComponent<MeshFilter>().sharedMesh.bounds;
        tankModule.center = GameMathf.ClampZeroWithRound(moduleBounds.center);
        tankModule.forward = GameMathf.ClampZeroWithRound(tankModule.center + new Vector3(0,0,moduleBounds.extents.z));
        tankModule.back = GameMathf.ClampZeroWithRound(tankModule.center + new Vector3(0, 0, -moduleBounds.extents.z));
        tankModule.left = GameMathf.ClampZeroWithRound(tankModule.center + new Vector3(-moduleBounds.extents.x, 0, 0));
        tankModule.right = GameMathf.ClampZeroWithRound(tankModule.center + new Vector3(moduleBounds.extents.x, 0, 0));
        tankModule.up = GameMathf.ClampZeroWithRound(tankModule.center + new Vector3(0, moduleBounds.extents.y, 0));
        tankModule.down = GameMathf.ClampZeroWithRound(tankModule.center + new Vector3(0, -moduleBounds.extents.y, 0));
        tankModule.downLeft = GameMathf.ClampZeroWithRound(tankModule.center + new Vector3(-moduleBounds.extents.x, -moduleBounds.extents.y, 0));
        tankModule.downRight = GameMathf.ClampZeroWithPrecision(tankModule.center + new Vector3(moduleBounds.extents.x, -moduleBounds.extents.y, 0));
    }

}
