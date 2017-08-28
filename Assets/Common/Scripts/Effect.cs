using UnityEngine;

/// <summary>
/// 特效，包含粒子和音效
/// </summary>
public class Effect : MonoBehaviour 
{
    public bool ativeWithParticle = true;               // 是否随这粒子结束失活对象
    public AudioSource effectAudio;                     // 音效
    public ParticleSystem effectParticle;               // 粒子

    private void Awake()
    {
        if (effectAudio == null)
            effectAudio = GetComponent<AudioSource>();
        if (effectParticle == null)
            effectParticle = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// 激活时生效
    /// </summary>
    void OnEnable()
    {
        // 开启爆炸音效
        if (effectAudio != null)
            effectAudio.Play();

        // 显示爆炸粒子
        if (effectParticle != null)
            effectParticle.Play();
    }

    /// <summary>
    /// 在粒子结束时，该对象设置失效
    /// </summary>
    void Update()
    {
        if (ativeWithParticle && effectParticle != null && effectParticle.isStopped)
            gameObject.SetActive(false);
    }
}
