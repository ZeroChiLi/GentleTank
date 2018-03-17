using UnityEngine;
using UnityEngine.PostProcessing;

public class MainMenuPostProcessManager : MonoBehaviour
{
    public PostProcessingBehaviour postProcess;     // 特效对象

    public float smoothTime = 1f;           // 变换持续时间
    /// <summary>
    /// 深度模糊特效控制
    /// </summary>
    [System.Serializable]
    public class EffectSetting { public float focusDistance = 0.1f, aperture = 2f, focalLength = 10f; }
    public EffectSetting defaultEffect;
    public EffectSetting startEffect;
    public EffectSetting armsEffect;
    public EffectSetting settingEffect;

    private DepthOfFieldModel dofModel;
    private DepthOfFieldModel.Settings dofSetting;
    private Vector3 current = new Vector3(0.1f, 2f, 10f);
    private Vector3 target = new Vector3(0.1f, 2f, 10f);
    private Vector3 velocity;

    private void Awake()
    {
        dofModel = postProcess.profile.depthOfField;
        dofSetting = dofModel.settings;
    }

    private void Update()
    {
        current = Vector3.SmoothDamp(current, target, ref velocity, smoothTime);
        dofSetting.focusDistance = current.x;
        dofSetting.aperture = current.y;
        dofSetting.focalLength = current.z;
        dofModel.settings = dofSetting;
    }

    /// <summary>
    /// 切换到默认效果
    /// </summary>
    public void ToDefaultEffect()
    {
        target = EffectToVector3(defaultEffect);
    }

    /// <summary>
    /// 切换到开始菜单效果
    /// </summary>
    public void ToStartEffect()
    {
        target = EffectToVector3(startEffect);
    }

    /// <summary>
    /// 切换到装备菜单效果
    /// </summary>
    public void ToArmsEffect()
    {
        target = EffectToVector3(armsEffect);
    }

    /// <summary>
    /// 切换到设置菜单效果
    /// </summary>
    public void ToSettingEffect()
    {
        target = EffectToVector3(settingEffect);
    }

    /// <summary>
    /// 将特效转换成Vector3
    /// </summary>
    public Vector3 EffectToVector3(EffectSetting effect)
    {
        return new Vector3(effect.focusDistance, effect.aperture, effect.focalLength);
    }

}
