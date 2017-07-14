using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 特效状态，吸收、混乱、释放
public enum EffectState
{
    None,Absorb,Chaos,Release,Completed,Crack
} 

public class EffectController : MonoBehaviour 
{
    public ObjectPool AbsorbEffectPool;             // 吸收效果池
    public ObjectPool ChaosEffectPool;              // 混乱效果池
    public ObjectPool ReleaseEffectPool;            // 释放效果池
    public ObjectPool CompletedEffectPool;          // 完成效果池
    public ObjectPool CrackEffectPool;              // 默认效果池

    public EffectState CurrentState { get { return currentState; } }    // 获取当前特效状态

    private EffectState currentState = EffectState.None;    // 当前特效状态
    private GameObject currentEffect;                       // 当前特性对象
    private ParticleSystem.MainModule currentParticleMain;  // 当前特效的粒子的主模型
    private float radius;                                   // 特效发射半径
    private Transform currentEffectTransform;               // 特效位置

    /// <summary>
    /// 设置粒子发射的半径
    /// </summary>
    /// <param name="radius">粒子发射的半径</param>
    public void SetParticleShapeRaidus(float radius)
    {
        this.radius = radius;
    }

    /// <summary>
    /// 关闭特效
    /// </summary>
    public void CloseEffect()
    {
        if (!EffectActive())
            return;
        currentEffect.SetActive(false);
        currentState = EffectState.None;
        currentEffect = null;
    }

    /// <summary>
    /// 设置特效
    /// </summary>
    /// <param name="effect">特效的状态</param>
    /// <param name="transform">特效的位置</param>
    public void SetEffect(EffectState effect,Transform transform)
    {
        if (effect == currentState)
            return;
        CloseEffect();
        currentState = effect;
        currentEffect = GetEffectObject(effect,transform);
        ParticleSystem particle = currentEffect.GetComponent<ParticleSystem>();
        currentParticleMain = particle.main;
        ParticleSystem.ShapeModule shape = particle.shape;
        shape.radius = radius;
    }

    /// <summary>
    /// 获取特效，失败返回null
    /// </summary>
    /// <param name="effect">特效状态</param>
    /// <param name="transform">特效的位置</param>
    /// <returns>特效对象</returns>
    public GameObject GetEffectObject(EffectState effect, Transform transform)
    {
        GameObject effectObject = null;
        switch (effect)
        {
            case EffectState.Absorb:
                effectObject= AbsorbEffectPool.GetNextObjectActive(transform);
                break;
            case EffectState.Chaos:
                effectObject = ChaosEffectPool.GetNextObjectActive(transform);
                break;
            case EffectState.Release:
                effectObject = ReleaseEffectPool.GetNextObjectActive(transform);
                break;
            case EffectState.Completed:
                effectObject = CompletedEffectPool.GetNextObjectActive(transform);
                break;
            case EffectState.Crack:
                effectObject = CrackEffectPool.GetNextObjectActive(transform);
                break;
        }
        if (effectObject != null)
            effectObject.transform.Rotate(-90, 0, 0);
        return effectObject;
    }

    /// <summary>
    /// 特效是否正确有效
    /// </summary>
    /// <returns>是否正确有效</returns>
    public bool EffectActive()
    {
        if (currentState == EffectState.None || currentEffect == null)
            return false;
        return true;
    }

    /// <summary>
    /// 设置粒子颜色
    /// </summary>
    /// <param name="color">颜色</param>
    public void SetColor(Color color)
    {
        if (!EffectActive())
            return;
        currentParticleMain.startColor = color;
    }
}
