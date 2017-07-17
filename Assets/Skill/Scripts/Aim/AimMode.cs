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
public class AimMode : Editor
{
    public Color normalColor = Color.black;             // 正常状态颜色
    public Color disableColor = Color.gray;             // 无效状态颜色
    public List<TagWithColor> tagColorList = new List<TagWithColor>();  // 标签颜色列表

    private int listCount;

    public TagWithColor this[int index]
    {
        get { return tagColorList[index]; }
        set { tagColorList[index] = value; }
    }

    /// <summary>
    /// 获取标签及其颜色数据，失败返回空
    /// </summary>
    /// <param name="tag">标签名</param>
    /// <returns>标签及其颜色数据</returns>
    public TagWithColor GetTagWithColorByTag(string tag)
    {
        for (int i = 0; i < tagColorList.Count; i++)
            if (tagColorList[i].tag == tag)
                return tagColorList[i];
        return null;
    }
}
