using UnityEngine;

/// <summary>
/// 屏幕后处理效果基类
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectsBase : MonoBehaviour
{

    protected void Start()
    {
        CheckResources();
    }

    /// <summary>
    /// 检测资源
    /// </summary>
    protected void CheckResources()
    {
        if (CheckSupport() == false)
            NotSupported();
    }

    /// <summary>
    /// 检测平台是否支持图片渲染
    /// </summary>
    /// <returns></returns>
    protected bool CheckSupport()
    {
        if (SystemInfo.supportsImageEffects == false)
        {
            Debug.LogWarning("This platform does not support image effects or render textures.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 不支持
    /// </summary>
    protected void NotSupported()
    {
        enabled = false;
    }

    /// <summary>
    /// 检测需要渲染的Shader可用性，然后返回使用了该shader的material
    /// </summary>
    /// <param name="shader">指定shader</param>
    /// <param name="material">创建的材质</param>
    /// <returns>得到指定shader的材质</returns>
    protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
    {
        if (shader == null)
            return null;

        if (shader.isSupported && material && material.shader == shader)
            return material;

        if (!shader.isSupported)
            return null;
        else
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            if (material)
                return material;
            else
                return null;
        }
    }
}
