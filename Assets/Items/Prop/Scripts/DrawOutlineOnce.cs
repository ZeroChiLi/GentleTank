using UnityEngine;

public class DrawOutlineOnce : PostEffectsBase 
{
    public MeshFilter[] meshFilters;

    [Range(0, 1)]
    public float width = 0.05f;
    public Color color = Color.green;

    // 也可以在OnPostRender()中更新
    private void Update()
    {
        if (TargetMaterial != null )
        {
            TargetMaterial.SetFloat("_Width", width);
            TargetMaterial.SetColor("_Color", color);

            for (int j = 0; j < meshFilters.Length; j++)
                Graphics.DrawMesh(meshFilters[j].sharedMesh, meshFilters[j].transform.localToWorldMatrix, TargetMaterial, 0);   // 对选中物体再次渲染。
        }
    }
}
