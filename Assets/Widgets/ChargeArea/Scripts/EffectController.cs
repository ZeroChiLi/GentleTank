using UnityEngine;

namespace Widget.ChargeArea
{
    // 特效状态，吸收、混乱、释放
    public enum EffectState
    {
        None, Absorb, Chaos, Release, Completed, Crack
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
        private ParticleSystem currentParticle;                 // 当前特效的粒子系统
        private ParticleSystem.MainModule currentParticleMain;  // 当前特效的粒子的主模型
        private ParticleSystem.ShapeModule currentParticleShape;// 当前特效粒子的发射形状
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
        /// <param name="color">粒子特效颜色</param>
        public void SetEffect(EffectState effect, Transform transform, Color color)
        {
            if (effect == currentState)
                return;
            CloseEffect();
            currentState = effect;
            currentEffect = GetEffectObject(effect, transform);             // 先获取特效

            currentParticle = currentEffect.GetComponent<ParticleSystem>(); // 获取特效的粒子
            currentParticleMain = currentParticle.main;
            currentParticleMain.startColor = color;                         // 设置粒子颜色

            currentParticleShape = currentParticle.shape;
            currentParticleShape.radius = radius;                           // 设置粒子发射半径

            currentEffect.SetActive(true);                                  // 最后再激活特效
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
                    effectObject = AbsorbEffectPool.GetNextObject(transform, false);
                    break;
                case EffectState.Chaos:
                    effectObject = ChaosEffectPool.GetNextObject(transform, false);
                    break;
                case EffectState.Release:
                    effectObject = ReleaseEffectPool.GetNextObject(transform, false);
                    break;
                case EffectState.Completed:
                    effectObject = CompletedEffectPool.GetNextObject(transform, false);
                    break;
                case EffectState.Crack:
                    effectObject = CrackEffectPool.GetNextObject(transform, false);
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

        public void SetParticleColor(Color color)
        {
            if (EffectActive())
                currentParticleMain.startColor = color;
        }

    }
}