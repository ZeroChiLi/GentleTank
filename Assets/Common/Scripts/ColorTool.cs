using UnityEngine;

static public class ColorTool 
{
    static public Color WarningColor = Color.magenta;       // 错误的颜色，默认粉色

    /// <summary>
    /// 给对象渲染颜色（包括自己和子组件），成功返回改变后颜色，失败返回粉红色
    /// </summary>
    /// <param name="gameObject">渲染对象</param>
    /// <param name="color">渲染颜色</param>
    /// <param name="materialName">特定材质名称</param>
    /// <returns>返回变后颜色</returns>
    static public Color ChangeSelfAndChildrens(GameObject gameObject,Color color,string materialName = null)
    {
        MeshRenderer selfMesh = gameObject.GetComponent<MeshRenderer>();
        if (selfMesh != null && (materialName == null  ||  materialName == selfMesh.material.name ))   // 自己本身有材质就染色
            selfMesh.material.color = color;

        MeshRenderer[] meshRenderer = gameObject.GetComponentsInChildren<MeshRenderer>();
        if (meshRenderer == null)       // 子组件如果没有材质
        {
            if (selfMesh == null)       // 且本身组件也没有返回错误颜色
                return WarningColor;
            return color;
        }

        for (int i = 0; i < meshRenderer.Length; i++)
            if (materialName == null || materialName == meshRenderer[i].material.name)
                meshRenderer[i].material.color = color;
        return color;
    }

    /// <summary>
    /// 获取带颜色的字符串
    /// </summary>
    /// <param name="color">指定颜色</param>
    /// <param name="str">指定字符串</param>
    /// <returns>带指定颜色的字符串</returns>
    static public string GetColorString(Color color,string str)
    {
        return string.Format("<#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(color), str);
    }
}
