using UnityEditor;
using UnityEngine;

public class TankAssembleWindow : EditorWindow
{
    public string generatePath = "Items/Tank/CustomTank/";  // 生成预设的位置
    public string tankName;                                 // 预设名称
    public TankModule bodyModule;                           // 车身部件
    public TankModule turretModule;                         // 头部部件
    public TankModule wheelLeftModule;                      // 左边车轮部件
    public TankModule wheelRightModule;

    private string relativePath;                            // 相对路径
    private GameObject turret;                              // 临时头部对象
    private GameObject wheelLeft;                           // 临时左边车轮对象
    private GameObject wheelRight;                          // 临时右边车轮对象
    private bool valid;                                     // 是否创建有效

    private Vector2 scrollPos;                              // 滑动面板位置
    private GameObject newTankPrefab;                       // 创建的坦克预设

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
        relativePath = string.Format("{0}{1}{2}", generatePath, tankName, ".prefab");
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
        return (wheelLeftModule != null && wheelLeftModule.type == TankModuleType.WheelLeft
            && wheelRightModule != null && wheelRightModule.type == TankModuleType.WheelRight
            && bodyModule != null && bodyModule.type == TankModuleType.Body
            && turretModule != null && turretModule.type == TankModuleType.Turret);
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
        if (System.IO.File.Exists(string.Format("{0}{1}{2}", Application.dataPath, "/", relativePath)))
        {
            Debug.LogErrorFormat("{0} Already Existed : {1}", tankName, relativePath);
            return;
        }

        if (!AssembleTank(ref newTankPrefab) || PrefabUtility.CreatePrefab("Assets/" + relativePath, newTankPrefab) == null)
            Debug.LogErrorFormat("Create Failed. {0}", "Assets/" + relativePath);
        else
            Debug.LogFormat("Create Successed. {0}", "Assets/" + relativePath);

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

        turret.transform.localPosition = bodyModule[TankModuleType.Turret].anchor - turretModule[TankModuleType.Body].anchor;
        wheelLeft.transform.localPosition = bodyModule[TankModuleType.WheelLeft].anchor - wheelLeftModule[TankModuleType.Body].anchor;
        wheelRight.transform.localPosition = bodyModule[TankModuleType.WheelRight].anchor - wheelRightModule[TankModuleType.Body].anchor;
        return true;
    }

}
