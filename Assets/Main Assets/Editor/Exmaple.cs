using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BeginScrollViewExample : EditorWindow
{
    Vector2 scrollPos;
    string t = "This is a string inside a Scroll view!";

    [MenuItem("Examples/Modify internal Quaternion")]
    static void Init()
    {
        BeginScrollViewExample window = (BeginScrollViewExample)EditorWindow.GetWindow(typeof(BeginScrollViewExample), true, "My Empty Window");
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        scrollPos =
            EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(100), GUILayout.Height(100));
        GUILayout.Label(t);
        EditorGUILayout.EndScrollView();
        if (GUILayout.Button("Add More Text", GUILayout.Width(100), GUILayout.Height(100)))
            t += " \nAnd this is more text!";
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Clear"))
            t = "";
    }
}