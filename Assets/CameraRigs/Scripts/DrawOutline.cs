using System.Collections.Generic;
using UnityEngine;

public class DrawOutline : PostEffectsBase
{
    public bool autoPostEffect;
    public Camera additionalCamera;

    public Shader drawOccupied;
    private Material occupiedMaterial = null;
    public Material OccupiedMaterial { get { return CheckShaderAndCreateMaterial(drawOccupied, ref occupiedMaterial); } }

    public Color outlineColor = Color.green;
    [Range(0, 10)]
    public int outlineWidth = 4;
    [Range(0, 9)]
    public int iterations = 1;

    public List<GameObject> targets;

    private MeshFilter[] meshFilters;
    private RenderTexture temRT1;
    private RenderTexture temRT2;

    /// <summary>
    /// 脚本激活时配置相机
    /// </summary>
    private void OnEnable()
    {
        SetupAddtionalCamera();
    }

    /// <summary>
    /// 额外相机的初始化
    /// </summary>
    private void SetupAddtionalCamera()
    {
        additionalCamera.enabled = false;
        additionalCamera.CopyFrom(MainCamera);
        additionalCamera.clearFlags = CameraClearFlags.Color;
        additionalCamera.backgroundColor = Color.black;
        additionalCamera.cullingMask = 1 << LayerMask.NameToLayer("PostEffect");       // 标记渲染"PostEffect"层的物体
    }

    /// <summary>
    /// 渲染当前相机后处理的描边效果
    /// </summary>
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (autoPostEffect && TargetMaterial != null && drawOccupied != null && additionalCamera != null && targets != null && targets.Count != 0)
        {
            SetupAddtionalCamera();

            temRT1 = RenderTexture.GetTemporary(source.width, source.height, 0);
            additionalCamera.targetTexture = temRT1;

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] == null)
                    continue;
                meshFilters = targets[i].GetComponentsInChildren<MeshFilter>();
                for (int j = 0; j < meshFilters.Length; j++)
                    if ((MainCamera.cullingMask & (1 << meshFilters[j].gameObject.layer)) != 0) // 把主相机没渲染的也不加入渲染队列
                        Graphics.DrawMesh(meshFilters[j].sharedMesh, meshFilters[j].transform.localToWorldMatrix, OccupiedMaterial, LayerMask.NameToLayer("PostEffect"), additionalCamera); // 描绘选中物体的所占面积
            }
            additionalCamera.Render();  // 需要调用渲染函数，才能及时把描绘物体渲染到纹理中

            TargetMaterial.SetTexture("_SceneTex", source);
            TargetMaterial.SetColor("_Color", outlineColor);
            TargetMaterial.SetInt("_Width", outlineWidth);
            TargetMaterial.SetInt("_Iterations", iterations);

            // 使用描边混合材质实现描边效果
            Graphics.Blit(temRT1, destination, TargetMaterial);

            temRT1.Release();
        }
        else
            Graphics.Blit(source, destination);
    }

    public void RenderToTexture(GameObject target, RenderTexture texture)
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
