using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item.Ammo
{
    public class GrenadeAmmo : ShellAmmo
    {
        public MeshRenderer mesh;
        public float delayTime = 2f;
        public Color normalColor = new Color(0.8f, 0, 0, 1);
        public Color blinkColor = new Color(0.7f, 0.7f, 0, 1);
        public int blinkTimes = 5;
        public AnimationCurve blinkSpeed = new AnimationCurve(new Keyframe(0, 0, 0, 0.1f), new Keyframe(1, 1, 1, 0));

        private CountDownTimer timer;
        private float blinkLength;

        protected new void Awake() { }

        protected new void OnEnable()
        {
            base.OnEnable();
            timer = new CountDownTimer(delayTime, false, true);
            blinkLength = 1f / blinkTimes;
            StartCoroutine(CrashCoroutine());
        }

        protected override void OnCrashed(Collider other) { }

        private IEnumerator CrashCoroutine()
        {
            while (!timer.IsTimeUp)
            {
                mesh.material.color = Color.Lerp(normalColor, blinkColor, Mathf.PingPong(Mathf.Repeat(blinkSpeed.Evaluate(timer.GetPercent()), blinkLength), blinkLength / 2f) * (2 * blinkTimes));
                yield return null;
            }
            base.OnCrashed(null);
        }
    }

}
