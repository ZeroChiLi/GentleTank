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
    private RenderTexture tempRT;

    private void Awake()
    {
        //SetupAddtionalCamera();
    }

    private void SetupAddtionalCamera()
    {
        additionalCamera.enabled = false;
        //additionalCamera.CopyFrom(MainCamera);
        additionalCamera.clearFlags = CameraClearFlags.Color;
        //additionalCamera.backgroundColor = Color.black;
        additionalCamera.cullingMask = 1 << LayerMask.NameToLayer("PostEffect");       // 标记渲染"PostEffect"层的物体
    }

    public void RenderToTexture(GameObject target,RenderTexture texture)
    {
        additionalCamera.targetTexture = texture;
        TargetMaterial.SetTexture("_SceneTex", texture);
        meshFilters = target.GetComponentsInChildren<MeshFilter>();
        for (int j = 0; j < meshFilters.Length; j++)
            Graphics.DrawMesh(meshFilters[j].sharedMesh, meshFilters[j].transform.localToWorldMatrix, OccupiedMaterial, LayerMask.NameToLayer("PostEffect"), additionalCamera); // 描绘选中物体的所占面积

        TargetMaterial.SetTexture("_MainTex", additionalCamera.targetTexture);
        TargetMaterial.SetColor("_Color", outlineColor);
        TargetMaterial.SetInt("_Width", outlineWidth);
        TargetMaterial.SetInt("_Iterations", iterations);

        //additionalCamera.RenderWithShader(targetShader, "");
        additionalCamera.Render();
    }

}
