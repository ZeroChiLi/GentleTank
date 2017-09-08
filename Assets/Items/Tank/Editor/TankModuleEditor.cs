using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TankModule))]
public class TankModuleEditor : Editor 
{
    public TankModule tankModule;

    protected GameObject targetPrefab;
    protected Bounds moduleBounds;

    private void OnEnable()
    {
        tankModule = target as TankModule;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (tankModule.prefab != null && targetPrefab != tankModule.prefab)
        {
            targetPrefab = tankModule.prefab;
            SetDefaultValue();
        }
        SetDefaultValueButton();
    }

    /// <summary>
    /// 通过预设来获取物体各基本点位置
    /// </summary>
    virtual public void SetDefaultValueButton()
    {
        if (!GUILayout.Button("Set Default Value") || tankModule.prefab == null)
            return;
        SetDefaultValue();
    }

    /// <summary>
    /// 设置默认值
    /// </summary>
    virtual public void SetDefaultValue()
    {
        moduleBounds = tankModule.prefab.GetComponent<MeshFilter>().sharedMesh.bounds;
        tankModule.center = GameMathf.ClampZeroWithRound(moduleBounds.center);
        tankModule.forward = GameMathf.ClampZeroWithRound(tankModule.center + new Vector3(0,0,moduleBounds.extents.z));
        tankModule.back = GameMathf.ClampZeroWithRound(tankModule.center + new Vector3(0, 0, -moduleBounds.extents.z));
        tankModule.left = GameMathf.ClampZeroWithRound(tankModule.center + new Vector3(-moduleBounds.extents.x, 0, 0));
        tankModule.right = GameMathf.ClampZeroWithRound(tankModule.center + new Vector3(moduleBounds.extents.x, 0, 0));
        tankModule.up = GameMathf.ClampZeroWithRound(tankModule.center + new Vector3(0, moduleBounds.extents.y, 0));
        tankModule.down = GameMathf.ClampZeroWithRound(tankModule.center + new Vector3(0, -moduleBounds.extents.y, 0));
    }

}
