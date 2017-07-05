using UnityEngine;

public class TankColor : MonoBehaviour
{

    /// <summary>
    /// 渲染所有带'T'类的脚本的子组件们颜色
    /// </summary>
    /// <param name="color">渲染颜色</param>
    public void RenderColorByComponent<T>(Color color) where T : Component
    {
        T[] renderChildren = GetComponentsInChildren<T>();
        if (renderChildren == null)
            return;
        for (int i = 0; i < renderChildren.Length; i++)
            SetMeshRenderColor(renderChildren[i], color);
    }

    /// <summary>
    /// 设置渲染网眼颜色
    /// </summary>
    /// <param name="gameObject">设置对象</param>
    /// <param name="color">渲染颜色</param>
    private void SetMeshRenderColor(Component needRenderComponent, Color color)
    {
        MeshRenderer[] meshRenderer = needRenderComponent.GetComponentsInChildren<MeshRenderer>();
        if (meshRenderer == null)
            return;
        for (int i = 0; i < meshRenderer.Length; i++)
            meshRenderer[i].material.color = color;
    }

}
