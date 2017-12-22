using UnityEngine;

public class BlinkPostEffect : PostEffectsBase 
{
    public Camera targetCamera;
    public bool blinkAtAwake = true;
    public float blinkTime = 1f;
    [Range(0f, 1f)]
    public float maxColorAlpha = 0.5f;
    public AnimationCurve blinkCurve;
    public Color color = Color.green;
    public MeshFilter[] meshFilters;
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
        if (isOpen && TargetMaterial != null )
        {
            if (Timer.Duration != blinkTime)
                Timer.Reset(blinkTime);
            TargetMaterial.SetColor("_Color", new Color(color.r,color.g,color.b, maxColorAlpha * blinkCurve.Evaluate(Mathf.PingPong(Timer.GetPercent() * 2F,1))));

            for (int j = 0; j < meshFilters.Length; j++)
                for (int k = 0; k < meshFilters[j].sharedMesh.subMeshCount; k++)
                    Graphics.DrawMesh(meshFilters[j].sharedMesh, meshFilters[j].transform.localToWorldMatrix, TargetMaterial, 0, null, k);
        }
    }
}
