using UnityEngine;

public class BlinkPostEffect : PostEffectsBase 
{
    public bool blinkAtAwake = true;
    public float blinkTime = 1f;
    [Range(0f, 1f)]
    public float maxColorAlpha = 0.5f;
    public AnimationCurve blinkCurve;
    public Color color = Color.green;
    public MeshFilter meshFilter;
    public bool isOpen;
    public CountDownTimer timer;
    public CountDownTimer Timer { get { return timer = timer == null ? timer = new CountDownTimer(blinkTime, true, true) : timer; } private set { timer = value; } }

    private void Awake()
    {
        if (blinkAtAwake)
            isOpen = true;
    }

    private void Update()
    {
        if (isOpen && TargetMaterial != null && meshFilter != null)
        {
            if (Timer.Duration != blinkTime)
                Timer.Reset(blinkTime);

            // 颜色透明度变化：指定事件的周期，根据曲线变化透明度。
            TargetMaterial.SetColor("_Color", new Color(color.r,color.g,color.b, maxColorAlpha * blinkCurve.Evaluate(Mathf.PingPong(Timer.GetPercent() * 2F,1))));

            for (int k = 0; k < meshFilter.sharedMesh.subMeshCount; k++)
                Graphics.DrawMesh(meshFilter.sharedMesh, meshFilter.transform.localToWorldMatrix, TargetMaterial, 0, null, k);
        }
    }
}
