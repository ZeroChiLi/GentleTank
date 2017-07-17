using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AimMode))]
[CanEditMultipleObjects]
public class AimModelEditor : Editor
{
    private AimMode aimMode;
    private List<TagWithColor> tagColorList;
    private int listCount;

    public override void OnInspectorGUI()
    {
        aimMode = (AimMode)target;
        tagColorList = aimMode.tagColorList;

        InputListSize();


        for (int i = 0; i < tagColorList.Count; i++)
        {
            EditorGUILayout.TagField("Tag",tagColorList[i].tag);
        }
    }

    public void InputListSize()
    {
        listCount = EditorGUILayout.IntField("Size : ", listCount);

    }
}
