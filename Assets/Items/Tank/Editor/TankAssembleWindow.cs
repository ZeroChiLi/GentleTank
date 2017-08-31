using UnityEditor;
using UnityEngine;

public class TankAssembleWindow : EditorWindow
{
    public string generatePath = "Items/Tank/CustomTank/";
    public string tankName;
    public TankModule bodyModule;
    public TankModule turretModule;
    public TankModule wheelLeftModule;
    public TankModule wheelRightModule;

    private string fullPath;
    private GameObject turret;
    private GameObject wheelLeft;
    private GameObject wheelRight;

    private bool valid;
    private Vector2 scrollPos;                  // 滑动面板位置
    private GameObject newTankPrefab;

    [MenuItem("Window/Tank Assemble")]
    static void ShowWindows()
    {
        EditorWindow window = GetWindow<TankAssembleWindow>();
        window.minSize = new Vector2(200f, 150f);
        window.Show();
    }

    private void OnGUI()
    {
        valid = GetAllVariable();
        CreateButton();
    }

    /// <summary>
    /// 获取所有变量
    /// </summary>
    /// <returns></returns>
    private bool GetAllVariable()
    {
        generatePath = EditorGUILayout.TextField("Generate Assets Path", generatePath);
        tankName = EditorGUILayout.TextField("Tank Renderers Name", tankName);
        fullPath = string.Format("{0}{1}{2}", generatePath, tankName, ".prefab");
        valid &= string.IsNullOrEmpty(generatePath) & string.IsNullOrEmpty(tankName);
        bodyModule = EditorGUILayout.ObjectField("Body", bodyModule, typeof(TankModule), false) as TankModule;
        turretModule = EditorGUILayout.ObjectField("Turret", turretModule, typeof(TankModule), false) as TankModule;
        wheelLeftModule = EditorGUILayout.ObjectField("Left Wheel", wheelLeftModule, typeof(TankModule), false) as TankModule;
        wheelRightModule = EditorGUILayout.ObjectField("Right Wheel", wheelRightModule, typeof(TankModule), false) as TankModule;
        return Check();
    }

    /// <summary>
    /// 检测所有部件是否有效
    /// </summary>
    /// <returns>只要存在无效的就返回false</returns>
    public bool Check()
    {
        return (wheelLeftModule != null && wheelLeftModule.type == ModuleType.WheelLeft
            && wheelRightModule != null && wheelRightModule.type == ModuleType.WheelRight
            && bodyModule != null && bodyModule.type == ModuleType.Body
            && turretModule != null && turretModule.type == ModuleType.Turret);
    }

    /// <summary>
    /// 创建预设按钮
    /// </summary>
    private void CreateButton()
    {
        if (!GUILayout.Button("Create Prefab"))
            return;
        if (!valid)
        {
            Debug.LogErrorFormat("TankName Or Modules Be Empty. Or Module Set An Invalid ModuleType.");
            return;
        }
        if (System.IO.File.Exists(string.Format("{0}{1}{2}", Application.dataPath, "/", fullPath)))
        {
            Debug.LogErrorFormat("{0} Already Existed : {1}", tankName, fullPath);
            return;
        }

        if (!AssembleTank(ref newTankPrefab) || PrefabUtility.CreatePrefab("Assets/" + fullPath, newTankPrefab) == null)
        {
            Debug.LogErrorFormat("Create Failed. {0}", "Assets/" + fullPath);
            return;
        }

    }

    /// <summary>
    /// 组装坦克
    /// </summary>
    /// <param name="newTank">新坦克对象</param>
    /// <returns>是否创建成功</returns>
    public bool AssembleTank(ref GameObject newTank)
    {
        newTank = new GameObject(tankName);
        Instantiate(bodyModule.prefab, newTank.transform);
        turret = Instantiate(turretModule.prefab, newTank.transform);
        wheelLeft = Instantiate(wheelLeftModule.prefab, newTank.transform);
        wheelRight = Instantiate(wheelRightModule.prefab, newTank.transform);

        turret.transform.localPosition = bodyModule[ModuleType.Turret].anchor - turretModule[ModuleType.Body].anchor;
        wheelLeft.transform.localPosition = bodyModule[ModuleType.WheelLeft].anchor - wheelLeftModule[ModuleType.Body].anchor;
        wheelRight.transform.localPosition = bodyModule[ModuleType.WheelRight].anchor - wheelRightModule[ModuleType.Body].anchor;
        return true;
    }

}
