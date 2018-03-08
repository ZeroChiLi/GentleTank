using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

public class MainMenuPostProcessManager : MonoBehaviour
{
    public PostProcessingBehaviour postProcess;
    [System.Serializable]
    public class DepthOfFieldEffect
    {
        public AnimationCurve changeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public int minFocalLength = 10;
        public int maxFocalLength = 30;
        public float duration = 2f;
    }
    public DepthOfFieldEffect dofEffect;

    private DepthOfFieldModel dofModel;
    private DepthOfFieldModel.Settings dofSetting;
    private CountDownTimer dofTimer;
    private Coroutine lastCoroutine;

    public void SetPostProcessEnable(bool enable)
    {
        postProcess.enabled = enable;
    }

    public void SetDepthOfFieldEnable(bool enable)
    {
        postProcess.profile.depthOfField.enabled = enabled;
    }

    public void PlayDepthOfFieldEffect(bool ascend = true)
    {
        if (lastCoroutine != null)
        {
            StopCoroutine(lastCoroutine);
            lastCoroutine = null;
        }
        lastCoroutine = StartCoroutine(DepthOfFieldCoroutine(ascend));
    }

    private IEnumerator DepthOfFieldCoroutine(bool ascend)
    {
        dofTimer = new CountDownTimer(dofEffect.duration);
        dofModel = postProcess.profile.depthOfField;
        dofSetting = dofModel.settings;
        float focalLength = dofEffect.maxFocalLength - dofEffect.minFocalLength;
        float evaluate;
        while (!dofTimer.IsTimeUp)
        {
            evaluate = dofEffect.changeCurve.Evaluate(ascend ? dofTimer.GetPercent() : 1 - dofTimer.GetPercent());
            dofSetting.focalLength = dofEffect.minFocalLength + focalLength * (evaluate);
            dofModel.settings = dofSetting;
            yield return null;
        }
        dofSetting.focalLength = ascend ? dofEffect.maxFocalLength : dofEffect.minFocalLength;
        dofModel.settings = dofSetting;
    }

}
