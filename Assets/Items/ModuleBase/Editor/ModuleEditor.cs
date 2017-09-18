using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModuleBase), true)]
public class ModuleEditor : Editor 
{
    public ModuleBase module;

    protected GameObject targetPrefab;
    protected Bounds moduleBounds;

    private void OnEnable()
    {
        module = target as ModuleBase;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (module.prefab != null && targetPrefab != module.prefab)
            targetPrefab = module.prefab;
        SetDefaultValueButton();
    }

    /// <summary>
    /// 通过预设来获取物体各基本点位置
    /// </summary>
    virtual public void SetDefaultValueButton()
    {
        if (module.prefab == null)
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
        moduleBounds = module.prefab.GetComponent<MeshFilter>().sharedMesh.bounds;
        SetDefaultValue();
    }
    
    /// <summary>
    /// 通过盒子碰撞体设置默认值
    /// </summary>
    public void SetDefaultValueByBoxCollider()
    {
        BoxCollider collider = module.prefab.GetComponent<BoxCollider>();
        moduleBounds = new Bounds(collider.center,collider.size);
        SetDefaultValue();
    }

    /// <summary>
    /// 设置默认值
    /// </summary>
    virtual public void SetDefaultValue()
    {
        module.anchors.center = GameMathf.Round(moduleBounds.center);
        module.anchors.forward = GameMathf.Round(moduleBounds.center + new Vector3(0, 0, moduleBounds.extents.z));
        module.anchors.back = GameMathf.Round(moduleBounds.center + new Vector3(0, 0, -moduleBounds.extents.z));
        module.anchors.left = GameMathf.Round(moduleBounds.center + new Vector3(-moduleBounds.extents.x, 0, 0));
        module.anchors.right = GameMathf.Round(moduleBounds.center + new Vector3(moduleBounds.extents.x, 0, 0));
        module.anchors.up = GameMathf.Round(moduleBounds.center + new Vector3(0, moduleBounds.extents.y, 0));
        module.anchors.down = GameMathf.Round(moduleBounds.center + new Vector3(0, -moduleBounds.extents.y, 0));
    }

}
