using UnityEngine;

public class ShellExplosion : MonoBehaviour 
{
    public AudioSource explosionAudio;          //爆炸音效
    public ParticleSystem explosionParticle;    //爆炸粒子

    private float timeElapse;                   //计算从激活开始过去的时间

    void OnEnable()
    {
        timeElapse = 0;

        // 开启爆炸音效
        explosionAudio.Play();

        // 显示爆炸粒子
        ParticleSystem explosionParticles = GetComponent<ParticleSystem>();
        explosionParticles.Play();
    }

    //在粒子结束时，该对象设置失效。
    void Update()
    {
        timeElapse += Time.deltaTime;
        if (timeElapse > explosionParticle.main.duration)
            gameObject.SetActive(false);
    }
}
