using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Points))]
internal class PointsEditor : Editor
{
    private Points Target;
    private ReorderableList pointList;

    private bool expandedPoints;
    private Vector2 numberDimension;
    private Vector2 labelDimension;
    private const float vSpace = 2;
    private const float hSpace = 3;
    private GUIStyle guiStyle;
    private Vector2 labelSize;

    private void OnEnable()
    {
        Target = target as Points;
        pointList = null;

        guiStyle = new GUIStyle();
        guiStyle.alignment = TextAnchor.MiddleCenter;

        labelSize = new Vector2(EditorGUIUtility.singleLineHeight * 2, EditorGUIUtility.singleLineHeight * 2);
    }

    public override void OnInspectorGUI()
    {
        if (pointList == null)
            SetupPointList();
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, "m_Script", "points");
        expandedPoints = EditorGUILayout.Foldout(expandedPoints, "Points Details");
        if (expandedPoints)
            pointList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// 配置好点对象列表
    /// </summary>
    private void SetupPointList()
    {
        pointList = new ReorderableList(serializedObject, serializedObject.FindProperty("points"), true, true, true, true);
        pointList.elementHeight *= 2;

        pointList.drawHeaderCallback = (Rect rect) => { GUI.Label(rect, "Point List"); };
        pointList.drawElementCallback = DrawPointElement;
    }

    /// <summary>
    /// 绘制点对象元素
    /// </summary>
    /// <param name="rect">矩形</param>
    /// <param name="index">索引</param>
    /// <param name="selected">是否选择了</param>
    /// <param name="focused">是否聚焦了</param>
    private void DrawPointElement(Rect rect, int index, bool selected, bool focused)
    {
        numberDimension = GUI.skin.button.CalcSize(new GUIContent("999"));
        labelDimension = GUI.skin.label.CalcSize(new GUIContent("Rotation "));

        SerializedProperty element = pointList.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += vSpace / 2;

        Rect r = new Rect(rect.position, numberDimension);
        r.y += numberDimension.y - r.height / 2;
        if (GUI.Button(r, new GUIContent(index.ToString(), "Go to the point in the scene view")))
        {
            pointList.index = index;
            SceneView.lastActiveSceneView.pivot = Target.transform.rotation * Target[index].position;
            SceneView.lastActiveSceneView.size = Target.appearance.focusSize * Target.appearance.pointSize;
            SceneView.lastActiveSceneView.Repaint();
        }

        r = new Rect(rect.position, labelDimension);
        r.x += hSpace + numberDimension.x;
        EditorGUI.LabelField(r, "Position");
        r.x += hSpace + r.width;
        r.width = rect.width - (numberDimension.x + hSpace + r.width + hSpace);
        EditorGUI.PropertyField(r, element.FindPropertyRelative("position"), GUIContent.none);

        r = new Rect(rect.position, labelDimension);
        r.y += numberDimension.y + vSpace;
        r.x += hSpace + numberDimension.x;
        r.width = labelDimension.x;
        EditorGUI.LabelField(r, "Rotation");
        r.x += hSpace + r.width;
        r.width = rect.width - (numberDimension.x + hSpace + r.width + hSpace);
        EditorGUI.PropertyField(r, element.FindPropertyRelative("eulerAngles"), GUIContent.none);
        r.x += r.width + hSpace;
    }

    private void OnSceneGUI()
    {
        if (pointList == null)
            SetupPointList();

        Matrix4x4 mOld = Handles.matrix;
        Color colorOld = Handles.color;
        Handles.matrix = Matrix4x4.Translate(Target.transform.position) * Matrix4x4.Rotate(Target.transform.rotation);

        for (int i = 0; i < Target.Count; ++i)
            DrawSelectionHandle(i);

        if (pointList.index >= 0 && pointList.index < Target.Count)
            DrawControl(pointList.index, Tools.current);

        Handles.color = colorOld;
        Handles.matrix = mOld;
    }

    /// <summary>
    /// 根据是否使用屏幕大小，获取点大小
    /// </summary>
    /// <param name="position">点的位置</param>
    /// <returns>点的实际大小</returns>
    private float GetPointSize(Vector3 position)
    {
        float pointSize;
        if (Target.appearance.useScreenSize)
            pointSize = HandleUtility.GetHandleSize(position) * Target.appearance.pointSize / 2f;
        else
            pointSize = Target.appearance.pointSize * 4f;
        return pointSize;
    }

    /// <summary>
    /// 绘制选择对象的控制
    /// </summary>
    /// <param name="i">点对象索引值</param>
    private void DrawSelectionHandle(int i)
    {
        if (Event.current.button != 0)
            return;
        Vector3 pos = Target[i].position;

        Handles.color = Target.appearance.pointColor;
        if (Handles.Button(pos, Quaternion.identity, GetPointSize(pos), GetPointSize(pos), Handles.SphereHandleCap) && pointList.index != i)
        {
            pointList.index = i;
            InternalEditorUtility.RepaintAllViews();
        }

        DrawPointAxisLine(i, Vector3.right, Color.red);
        DrawPointAxisLine(i, Vector3.up, Color.green);
        DrawPointAxisLine(i, Vector3.forward, Color.blue);

        Handles.BeginGUI();
        GUILayout.BeginArea(LabelRect(pos));
        guiStyle.normal.textColor = Target.appearance.indexFontColor;
        guiStyle.fontSize = Target.appearance.indexFontSize;
        GUILayout.Label(new GUIContent(i.ToString(), string.Format("Point {0}\nPosition: {1}\nRotation: {2}", i, Target[i].position, Target[i].eulerAngles)), guiStyle);
        GUILayout.EndArea();
        Handles.EndGUI();
    }

    /// <summary>
    /// 获取索引标签矩形
    /// </summary>
    /// <param name="pos">点对象所在位置</param>
    /// <returns>引标签矩形</returns>
    private Rect LabelRect(Vector3 pos)
    {
        Vector2 labelPos = HandleUtility.WorldToGUIPoint(pos);
        labelPos.y -= labelSize.y / 2;
        labelPos.x -= labelSize.x / 2;
        return new Rect(labelPos, labelSize);
    }

    /// <summary>
    /// 绘制点对象自身的轴
    /// </summary>
    /// <param name="i">索引</param>
    /// <param name="dir">方向</param>
    /// <param name="color">颜色</param>
    private void DrawPointAxisLine(int i, Vector3 dir, Color color)
    {
        Handles.color = color;
        if (Target.appearance.useScreenSize)
            Handles.DrawLine(Target[i].position, Target[i].position + Target[i].rotation * dir * Target.appearance.axisLength * GetPointSize(Target[i].position));
        else
            Handles.DrawLine(Target[i].position, Target[i].position + Target[i].rotation * dir * Target.appearance.axisLength);

    }

    /// <summary>
    /// 绘制移动和旋转的控制
    /// </summary>
    /// <param name="i">点对象的索引</param>
    /// <param name="type">工具类型</param>
    private void DrawControl(int i, Tool type)
    {
        if (type != Tool.Move && type != Tool.Rotate)
            return;
        Handles.color = Target.appearance.selectedColor;
        Point point = Target[i];
        EditorGUI.BeginChangeCheck();
        Quaternion rotation = (Tools.pivotRotation == PivotRotation.Local) ? Quaternion.identity : Quaternion.Inverse(Target.transform.rotation);
        Handles.SphereHandleCap(0, point.position, rotation, GetPointSize(point.position), EventType.Repaint);

        Quaternion newRotation = new Quaternion();
        Vector3 pos = new Vector3();
        if (type == Tool.Rotate)
            newRotation = Handles.RotationHandle(point.rotation, point.position);
        else if (type == Tool.Move)
            pos = Handles.PositionHandle(point.position, rotation);

        if (EditorGUI.EndChangeCheck())
        {
            if (type == Tool.Rotate)
            {
                Undo.RecordObject(target, "Rotate Point");
                point.rotation = newRotation;
            }
            else if (type == Tool.Move)
            {
                Undo.RecordObject(target, "Move Point");
                point.position = pos;
            }
            Target[i] = point;
        }
    }
}
