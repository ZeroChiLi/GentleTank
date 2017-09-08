using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TankModuleWheel))]
public class TankModuleWheelEditor : TankModuleEditor 
{
    private Vector3 tem;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //SwapLeftAndRightButton();
    }

    public void SwapLeftAndRightButton()
    {
        if (GUILayout.Button("SwapLeftAndRight"))
        {
            tem = tankModule.left;
            tankModule.left = tankModule.right;
            tankModule.right = tem;
        }
    }
}
