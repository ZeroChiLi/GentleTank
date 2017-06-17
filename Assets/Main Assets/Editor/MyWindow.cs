using UnityEngine;
using UnityEditor;
using System.Collections;

// This example shows how to create a context menu inside a custom EditorWindow.
// context-click the green area to show the menu

public class GenericMenuExample : EditorWindow
{
    [MenuItem("Example/Open Window")]
    static void Init()
    {
        EditorWindow window = GetWindow<GenericMenuExample>();
        window.position = new Rect(50, 50, 250, 60);
        window.Show();
    }

    void Callback(object obj)
    {
        Debug.Log("Selected: " + obj);
    }

    void OnGUI()
    {
        Event currentEvent = Event.current;
        Rect contextRect = new Rect(10, 10, 100, 100);
        EditorGUI.DrawRect(contextRect, Color.green);

        if (currentEvent.type == EventType.ContextClick)
        {
            Vector2 mousePos = currentEvent.mousePosition;
            if (contextRect.Contains(mousePos))
            {
                // Now create the menu, add items and show it
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("MenuItem1"), false, Callback, "item 1");
                menu.AddItem(new GUIContent("MenuItem2"), false, Callback, "item 2");
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("SubMenu/MenuItem3"), false, Callback, "item 3");
                menu.ShowAsContext();
                currentEvent.Use();
            }
        }
    }
}