using UnityEditor;
using UnityEngine;

public class TankAssembleWindow : EditorWindow
{
    public string generatePath = "Items/Tank/CustomTank/";  // 生成预设的位置
    public string tankName;                                 // 预设名称
    public TankModule headModule;                           // 头部部件
    public TankModule bodyModule;                           // 车身部件
    public TankModule wheelLeftModule;                      // 左边车轮部件

    private string relativePath;                            // 相对路径
    private GameObject head;                                // 临时头部对象
    private GameObject body;                                // 临时身体对象
    private GameObject wheelLeft;                           // 临时左边车轮对象
    private GameObject wheelRight;                          // 临时右边车轮对象
    private bool valid;                                     // 是否创建有效

    private Vector2 scrollPos;                              // 滑动面板位置
    private GameObject newTankPrefab;                       // 创建的坦克预设
    private TankModuleManager temTankModuleManager;         // 临时部件管理器
    private TankModuleTypeTag temTypeTag;                   // 临时部件类型标记

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
        headModule = EditorGUILayout.ObjectField("Turret", headModule, typeof(TankModule), false) as TankModule;
        wheelLeftModule = EditorGUILayout.ObjectField("Left Wheel", wheelLeftModule, typeof(TankModule), false) as TankModule;
        return Check();
    }

    /// <summary>
    /// 检测所有部件是否有效
    /// </summary>
    /// <returns>只要存在无效的就返回false</returns>
    public bool Check()
    {
        return (wheelLeftModule != null && wheelLeftModule.type == TankModuleType.WheelLeft
            && bodyModule != null && bodyModule.type == TankModuleType.Body
            && headModule != null && headModule.type == TankModuleType.Head);
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
        InstantiateModules(newTank.transform);
        SetupTankModuleManager(newTank).Setup(headModule, bodyModule, wheelLeftModule, head, body, wheelLeft, wheelRight);

        head.transform.localPosition = bodyModule[TankModuleType.Head].anchor - headModule[TankModuleType.Body].anchor;
        wheelLeft.transform.localPosition = bodyModule[TankModuleType.WheelLeft].anchor - wheelLeftModule[TankModuleType.Body].anchor;
        wheelRight.transform.localPosition = bodyModule[TankModuleType.WheelRight].anchor - wheelLeftModule[TankModuleType.Body].anchor;
        return true;
    }

    /// <summary>
    /// 配置坦克部件管理器
    /// </summary>
    /// <param name="tank">坦克对象</param>
    /// <returns>返回对应部件管理器</returns>
    private TankModuleManager SetupTankModuleManager(GameObject tank)
    {
        temTankModuleManager = tank.GetComponent<TankModuleManager>();
        if (temTankModuleManager == null)
            temTankModuleManager = tank.AddComponent<TankModuleManager>();
        return temTankModuleManager;
    }

    /// <summary>
    /// 创建部件们的实例
    /// </summary>
    /// <param name="parent">父对象</param>
    private void InstantiateModules(Transform parent)
    {
        body = SetTankModuleTypeTag(Instantiate(bodyModule.prefab, parent), TankModuleType.Body);
        head = SetTankModuleTypeTag(Instantiate(headModule.prefab, parent), TankModuleType.Head);
        wheelLeft = SetTankModuleTypeTag(Instantiate(wheelLeftModule.prefab, parent), TankModuleType.WheelLeft);
        wheelRight = SetTankModuleTypeTag(Instantiate(wheelLeftModule.prefab, parent), TankModuleType.WheelRight);
        wheelRight.transform.localScale = new Vector3(-wheelRight.transform.localScale.x, wheelRight.transform.localScale.y, wheelRight.transform.localScale.z);
    }

    /// <summary>
    /// 设置坦克部件的类型标签
    /// </summary>
    /// <param name="module">部件</param>
    /// <param name="type">类型</param>
    /// <returns>返回本身这个部件</returns>
    private GameObject SetTankModuleTypeTag(GameObject module, TankModuleType type)
    {
        temTypeTag = module.GetComponent<TankModuleTypeTag>();
        if (temTypeTag == null)
            temTypeTag = module.AddComponent<TankModuleTypeTag>();
        temTypeTag.moduleType = type;
        return module;
    }

}
