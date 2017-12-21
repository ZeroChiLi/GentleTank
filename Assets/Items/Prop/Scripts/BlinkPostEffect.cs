using UnityEngine;

public class BlinkPostEffect : PostEffectsBase 
{
    public bool blinkAtAwake = true;
    public float blinkTime = 1f;
    [Range(0f, 1f)]
    public float maxColorAlpha = 0.5f;
    public AnimationCurve blinkCurve;
    public Color color = Color.green;
    public MeshFilter[] meshFilters;
    public bool isOpen;
    public CountDownTimer timer;

    private void Awake()
    {
        timer = new CountDownTimer(blinkTime, true, true);
        if (blinkAtAwake)
            isOpen = true;
    }

    // 也可以在OnPostRender()中更新
    private void Update()
    {
        if (isOpen && TargetMaterial != null )
        {
            if (timer.Duration != blinkTime)
                timer.Reset(blinkTime);
            TargetMaterial.SetColor("_Color", new Color(color.r,color.g,color.b, maxColorAlpha * blinkCurve.Evaluate(Mathf.PingPong(timer.GetPercent() * 2F,1))));

            for (int j = 0; j < meshFilters.Length; j++)
                Graphics.DrawMesh(meshFilters[j].sharedMesh, meshFilters[j].transform.localToWorldMatrix, TargetMaterial, 0);   // 对选中物体再次渲染。
        }
    }
}
