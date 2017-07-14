using UnityEngine;

/// <summary>
/// 特效，包含粒子和音效
/// </summary>
public class Effects : MonoBehaviour 
{
    public AudioSource effectAudio;             // 音效
    public ParticleSystem effectParticle;       // 粒子

    private float timeElapse;                   //计算从激活开始过去的时间

    private void Awake()
    {
        if (effectAudio == null)
            effectAudio = GetComponent<AudioSource>();
        if (effectParticle == null)
            effectParticle = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        timeElapse = 0;

        // 开启爆炸音效
        if (effectAudio != null)
            effectAudio.Play();

        // 显示爆炸粒子
        if (effectParticle != null)
            effectParticle.Play();
    }

    //在粒子结束时，该对象设置失效。
    void Update()
    {
        if (effectParticle.isStopped)
            gameObject.SetActive(false);
        //timeElapse += Time.deltaTime;
        //if (timeElapse > explosionParticle.main.duration)
    }
}
