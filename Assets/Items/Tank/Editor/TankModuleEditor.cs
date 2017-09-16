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
            targetPrefab = tankModule.prefab;
        SetDefaultValueButton();
    }

    /// <summary>
    /// 通过预设来获取物体各基本点位置
    /// </summary>
    virtual public void SetDefaultValueButton()
    {
        if (tankModule.prefab == null)
            return;
        if (GUILayout.Button("Set Default By MeshFilter"))
            SetDefaultValueByMeshFilter();
        if (GUILayout.Button("Set Default By BoxCollider"))
            SetDefaultValueByBoxCollider();
    }

    /// <summary>
    /// 拖过网格过滤器设置默认值
    /// </summary>
    public void SetDefaultValueByMeshFilter()
    {
        moduleBounds = tankModule.prefab.GetComponent<MeshFilter>().sharedMesh.bounds;
        SetDefaultValue();
    }
    
    /// <summary>
    /// 通过盒子碰撞体设置默认值
    /// </summary>
    public void SetDefaultValueByBoxCollider()
    {
        BoxCollider collider = tankModule.prefab.GetComponent<BoxCollider>();
        moduleBounds = new Bounds(collider.center,collider.size);
        SetDefaultValue();
    }

    /// <summary>
    /// 设置默认值
    /// </summary>
    virtual public void SetDefaultValue()
    {
        tankModule.anchors.center = GameMathf.Round(moduleBounds.center);
        tankModule.anchors.forward = GameMathf.Round(moduleBounds.center + new Vector3(0, 0, moduleBounds.extents.z));
        tankModule.anchors.back = GameMathf.Round(moduleBounds.center + new Vector3(0, 0, -moduleBounds.extents.z));
        tankModule.anchors.left = GameMathf.Round(moduleBounds.center + new Vector3(-moduleBounds.extents.x, 0, 0));
        tankModule.anchors.right = GameMathf.Round(moduleBounds.center + new Vector3(moduleBounds.extents.x, 0, 0));
        tankModule.anchors.up = GameMathf.Round(moduleBounds.center + new Vector3(0, moduleBounds.extents.y, 0));
        tankModule.anchors.down = GameMathf.Round(moduleBounds.center + new Vector3(0, -moduleBounds.extents.y, 0));
    }

}
