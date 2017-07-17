using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class TagWithColor
{
    public string tag;          // 标签
    public Color color;         // 颜色
}

[CreateAssetMenu(menuName = "Skill/Aim Mode")]
public class AimMode : ScriptableObject
{
    public Color normalColor = Color.black;             // 正常状态颜色
    public Color disableColor = Color.gray;             // 无效状态颜色
    public List<TagWithColor> tagColorList = new List<TagWithColor>();  // 标签颜色列表

    public TagWithColor this[int index]
    {
        get { return tagColorList[index]; }
        set { tagColorList[index] = value; }
    }

    /// <summary>
    /// 是否包含标签
    /// </summary>
    /// <param name="tag">标签名</param>
    /// <returns>是否包含标签</returns>
    public bool ContainTag(string tag)
    {
        for (int i = 0; i < tagColorList.Count; i++)
            if (tagColorList[i].tag == tag)
                return true;
        return false;
    }

    /// <summary>
    /// 获取标签对应列表的索引，失败返回-1
    /// </summary>
    /// <param name="tag">标签名</param>
    /// <returns>标签对应列表的索引</returns>
    public int GetTagIndex(string tag)
    {
        for (int i = 0; i < tagColorList.Count; i++)
            if (tagColorList[i].tag == tag)
                return i;
        return -1;
    }

    /// <summary>
    /// 通过标签获取颜色，失败时返回粉色
    /// </summary>
    /// <param name="tag">标签</param>
    /// <returns>标签对应颜色</returns>
    public Color GetColorByTag(string tag)
    {
        for (int i = 0; i < tagColorList.Count; i++)
            if (tagColorList[i].tag == tag)
                return tagColorList[i].color;
        return Color.magenta;
    }
}
