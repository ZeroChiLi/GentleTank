using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(AimMode))]
//[CanEditMultipleObjects]
public class AimModeEditor : Editor
{
    private AimMode aimMode;
    private SerializedProperty damageProp;
    private SerializedProperty armorProp;
    private SerializedProperty gunProp;
    private int listCount;

    /// <summary>
    /// 初始化配置
    /// </summary>
    private void Awake()
    {
        aimMode = (AimMode)target;
        listCount = aimMode.tagColorList.Count;
    }

    ///// <summary>
    ///// 更新面板时更新列表
    ///// </summary>
    //public override void OnInspectorGUI()
    //{
    //    serializedObject.Update();
    //    DefaultColor();             // 正常、无效颜色
    //    InputListSize();            // 列表长度
    //    ShowTagColorList();         // 列表项
    //    serializedObject.ApplyModifiedProperties();
    //}

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
        if (listCount == aimMode.tagColorList.Count)
            return;
        if (listCount > aimMode.tagColorList.Count)
            for (int i = aimMode.tagColorList.Count; i < listCount; i++)
                aimMode.tagColorList.Add(new TagWithColor());
        else
            for (int i = aimMode.tagColorList.Count; i > listCount; i--)
                aimMode.tagColorList.Remove(aimMode.tagColorList[i-1]);
    }

    /// <summary>
    /// 显示标签颜色列表
    /// </summary>
    public void ShowTagColorList()
    {
        EditorGUI.indentLevel = 1;
        for (int i = 0; i < aimMode.tagColorList.Count; i++)
        {
            EditorGUILayout.BeginVertical("BOX");
            aimMode.tagColorList[i].tag = EditorGUILayout.TagField("Tag", aimMode.tagColorList[i].tag);
            aimMode.tagColorList[i].color = EditorGUILayout.ColorField("Color", aimMode.tagColorList[i].color);
            EditorGUILayout.EndVertical();
        }
        EditorGUI.indentLevel = 0;
    }
}
