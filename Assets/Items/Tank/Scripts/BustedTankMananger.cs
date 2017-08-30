using UnityEngine;

public class BustedTankMananger : MonoBehaviour 
{
    [Range(0,1)]
    public float colorLerp = 0.5f;                                  // 颜色插值（默认残骸颜色到手动设置颜色插值）
    public Color bustedColor = new Color(0.286f, 0.286f, 0.286f,1); // 默认残骸颜色
    public bool isDisslove = true;                                  // 是否会消融
    public float delayTime = 7f;                                    // 延迟消融时间
    public float dissloveTime = 3f;                                 // 消融时间 
    public AnimationCurve dissolveTime = AnimationCurve.EaseInOut(0,0,1,1); //消融变化曲线

    private Material material;                                      // 自己的材质
    private CountDownTimer delayTimer;                              // 延时计时器
    private CountDownTimer dissloveTimer;                           // 消融计时器

    /// <summary>
    /// 初始化两个计时器，获取自己的材质
    /// </summary>
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        delayTimer = new CountDownTimer(delayTime, false, false);
        dissloveTimer = new CountDownTimer(dissloveTime, false, false);
    }

    /// <summary>
    /// 如果勾选消融，那就先开始计时延时计时器
    /// </summary>
    private void OnEnable()
    {
        if (isDisslove)
            delayTimer.Start();
    }

    /// <summary>
    /// 消失后重置预设
    /// </summary>
    private void OnDisable()
    {
		material.SetFloat("_BurnAmount", 0.0f);
        delayTimer.Reset(delayTime, true);
        dissloveTimer.Reset(dissloveTime, true);
    }

    /// <summary>
    /// 如果勾选消融，更新延时和消融
    /// </summary>
    private void Update()
    {
        if (!isDisslove || !delayTimer.IsTimeUp)
            return;

        if (dissloveTimer.IsStoped)
            dissloveTimer.Start();

        if (dissloveTimer.IsTimeUp)
            gameObject.SetActive(false);
        else
            material.SetFloat("_BurnAmount", dissolveTime.Evaluate(dissloveTimer.GetPercent()));
    }

    /// <summary>
    /// 配置坦克残骸位置和颜色插值
    /// </summary>
    /// <param name="transform">位置，角度</param>
    /// <param name="color">颜色</param>
    public void SetupBustedTank(Transform transform,Color color)
    {
        this.transform.position = transform.position;
        this.transform.rotation = transform.rotation;
        material.color = Color.Lerp(bustedColor,color,colorLerp);
		material.SetFloat("_BurnAmount", 0.0f);
    }
}
