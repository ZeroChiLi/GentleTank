using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[System.Serializable]
public class TagWithColor
{
    public string tag;          // 标签
    public Color color;         // 颜色
}

[System.Serializable]
[CreateAssetMenu(menuName = "Skill/Aim Mode")]
public class AimMode : ScriptableObject
{
    public Sprite aimSprite;                            // 瞄准图片精灵
    public bool showInScreen = true;                    // 是否显示在屏幕，否则显示在场景中
    public Color normalColor = Color.black;             // 正常状态颜色
    public Color disableColor = Color.gray;             // 无效状态颜色
    public List<TagWithColor> tagColorList = new List<TagWithColor>();  // 标签颜色列表

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
