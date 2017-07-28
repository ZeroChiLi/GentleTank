using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankModelUI : MonoBehaviour 
{
    /// <summary>
    /// 渲染颜色，成功返回改变后颜色，失败返回白色
    /// </summary>
    /// <returns>返回变后颜色</returns>
    public Color ChangeColor(Color color)
    {
        MeshRenderer[] meshRenderer = GetComponentsInChildren<MeshRenderer>();
        if (meshRenderer == null)
            return Color.white;
        for (int i = 0; i < meshRenderer.Length; i++)
            meshRenderer[i].material.color = color;
        return color;
    }
}
