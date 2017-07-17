using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AimMode))]
[CanEditMultipleObjects]
public class AimModeEditor : Editor
{
    private AimMode aimMode;
    private List<TagWithColor> tagColorList;
    private int listCount;

    /// <summary>
    /// 更新面板时更新列表
    /// </summary>
    public override void OnInspectorGUI()
    {
        aimMode = (AimMode)target;
        tagColorList = aimMode.tagColorList;
        listCount = tagColorList.Count;

        DefaultColor();
        InputListSize();
        ShowTagColorList();
    }

    /// <summary>
    /// 默认颜色配置
    /// </summary>
    public void DefaultColor()
    {
        aimMode.normalColor = EditorGUILayout.ColorField("Normal Color",aimMode.normalColor);
        aimMode.disableColor = EditorGUILayout.ColorField("Disable Color", aimMode.disableColor);
    }

    /// <summary>
    /// 输入列表长度，用来改变
    /// </summary>
    public void InputListSize()
    {
        listCount = Mathf.Clamp(EditorGUILayout.IntField("List Size", listCount),0,10);      // 限制范围0~10
        if (listCount == tagColorList.Count)
            return;
        if (listCount > tagColorList.Count)
            for (int i = tagColorList.Count; i < listCount; i++)
                tagColorList.Add(new TagWithColor());
        else
            for (int i = tagColorList.Count; i > listCount; i--)
                tagColorList.Remove(tagColorList[i-1]);
    }

    /// <summary>
    /// 显示标签颜色列表
    /// </summary>
    public void ShowTagColorList()
    {
        EditorGUI.indentLevel = 1;
        for (int i = 0; i < tagColorList.Count; i++)
        {
            EditorGUILayout.BeginVertical("BOX");
            tagColorList[i].tag = EditorGUILayout.TagField("Tag", tagColorList[i].tag);
            tagColorList[i].color = EditorGUILayout.ColorField("Color", tagColorList[i].color);
            EditorGUILayout.EndVertical();
        }
        EditorGUI.indentLevel = 0;
    }
}
