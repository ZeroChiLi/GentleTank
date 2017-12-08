using UnityEngine;

public class DrawOutline : PostEffectsBase
{
    public Camera additionalCamera;

    public Shader drawOccupied;
    private Material occupiedMaterial = null;
    public Material OccupiedMaterial { get { return CheckShaderAndCreateMaterial(drawOccupied, ref occupiedMaterial); } }

    public Color outlineColor = Color.green;
    [Range(0, 10)]
    public int outlineWidth = 4;
    [Range(0, 9)]
    public int iterations = 1;

    public GameObject[] targets;

    private MeshFilter[] meshFilters;
    private RenderTexture temRT1;
    private RenderTexture temRT2;

    public void RenderToTexture(GameObject target,RenderTexture texture)
    {
        // 第一步：把渲染的物体渲染进纹理先
        additionalCamera.cullingMask = 1;
        additionalCamera.targetTexture = texture;
        additionalCamera.targetTexture.Release();
        additionalCamera.Render();

        // 第二步：把这个纹理备份到_SceneTex
        temRT1 = RenderTexture.GetTemporary(texture.width, texture.height);
        Graphics.Blit(additionalCamera.targetTexture, temRT1);
        TargetMaterial.SetTexture("_SceneTex", temRT1);

        // 第三步：渲染物体的面积
        additionalCamera.cullingMask = 1 << LayerMask.NameToLayer("PostEffect");
        meshFilters = target.GetComponentsInChildren<MeshFilter>();
        for (int j = 0; j < meshFilters.Length; j++)
            Graphics.DrawMesh(meshFilters[j].sharedMesh, meshFilters[j].transform.localToWorldMatrix, OccupiedMaterial, LayerMask.NameToLayer("PostEffect"), additionalCamera); // 描绘选中物体的所占面积
        additionalCamera.Render();

        // 第四步：实现描边效果
        TargetMaterial.SetColor("_Color", outlineColor);
        TargetMaterial.SetInt("_Width", outlineWidth);
        TargetMaterial.SetInt("_Iterations", iterations);
        temRT2 = RenderTexture.GetTemporary(texture.width, texture.height);
        Graphics.Blit(additionalCamera.targetTexture, temRT2, TargetMaterial);
        Graphics.Blit(temRT2, additionalCamera.targetTexture);

    }

}
