using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AimMode))]
[CanEditMultipleObjects]
public class AimModelEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.TagField("Tag","Untagged");
    }
}
