using Item.Ammo;
using System.Collections;
using UnityEngine;

namespace Item.Tank
{
    public class TankAttackBoxing : TankAttack
    {
        public SpringBoxingGloveManager springBoxingGlove;     // 弹簧控制
        public AnimationCurve launchDistance = AnimationCurve.Linear(0, 0, 0.3f, 1);    // 发射距离比例,来回对称
        public float launchTotalTime = 0.6f;    // 总共发射来回时间

        private float launchElapsed;            // 发射后经过的时间

        protected new void OnEnable()
        {
            base.OnEnable();
            if (springBoxingGlove)
            {
                springBoxingGlove.fillAmount = 0;
                springBoxingGlove.ammo.launcher = playerManager;
            }
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            if (springBoxingGlove)
                springBoxingGlove.fillAmount = 0;
        }

        /// <summary>
        /// 攻击实际效果
        /// </summary>
        protected override void OnAttack(params object[] values)
        {
            if (values == null || values.Length == 0)
                Launch(forceSlider.value, damage, coolDownTime);
            else if (values.Length == 1)
                Launch((float)values[0], damage, coolDownTime);
            else if (values.Length == 3)
                Launch((float)values[0], (float)values[1], (float)values[2]);
        }

        /// <summary>
        /// 发射炮弹，自定义参数变量
        /// </summary>
        /// <param name="launchForce">发射力度</param>
        /// <param name="fireDamage">伤害值</param>
        /// <param name="coolDownTime">发射后冷却时间</param>
        private void Launch(float launchForce, float fireDamage, float coolDownTime)
        {
            springBoxingGlove.maxDistance = launchForce / 10f;
            springBoxingGlove.ammo.damage = fireDamage;
            springBoxingGlove.glove.enabled = true;

            StartCoroutine(LaunchBoxingGlove());

            cdTimer.Reset(coolDownTime);
        }

        /// <summary>
        /// 发射弹簧拳协程
        /// </summary>
        /// <returns></returns>
        private IEnumerator LaunchBoxingGlove()
        {
            launchElapsed = 0f;
            springBoxingGlove.ammo.needTurnBack = false;
            while (launchElapsed < launchTotalTime && launchElapsed >= 0f)
            {
                if (!springBoxingGlove.ammo.needTurnBack && launchElapsed < launchTotalTime / 2f)
                {
                    springBoxingGlove.fillAmount = launchDistance.Evaluate(launchElapsed);
                    launchElapsed += Time.deltaTime;
                }
                else
                {
                    if (springBoxingGlove.ammo.needTurnBack)
                    {
                        launchElapsed -= Time.deltaTime;
                        springBoxingGlove.fillAmount = launchDistance.Evaluate(launchElapsed);
                    }
                    else
                    {
                        launchElapsed += Time.deltaTime;
                        springBoxingGlove.fillAmount = launchDistance.Evaluate(launchTotalTime - launchElapsed);
                    }
                    springBoxingGlove.glove.enabled = false;
                }

                yield return null;
            }
            springBoxingGlove.fillAmount = 0f;
            springBoxingGlove.glove.enabled = false;
        }
    }
}