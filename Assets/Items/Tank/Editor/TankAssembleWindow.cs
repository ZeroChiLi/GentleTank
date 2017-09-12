using UnityEditor;
using UnityEngine;

public class TankAssembleWindow : EditorWindow
{
    public bool createPrefab;                               // 是否创建预设
    public string generatePath = "Items/Tank/CustomTank/";  // 生成预设的位置
    public TankAssembleManager tankAssemble;

    private string relativePath;                            // 相对路径
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
        createPrefab = EditorGUILayout.Toggle("Create Prefab", createPrefab);
        valid = true;
        valid &= GetAllVariable();
        CreateButton();
    }

    /// <summary>
    /// 获取所有变量
    /// </summary>
    /// <returns></returns>
    private bool GetAllVariable()
    {
        if (createPrefab)
        {
            generatePath = EditorGUILayout.TextField("Generate Assets Path", generatePath);
            valid &= string.IsNullOrEmpty(generatePath);
        }
        tankAssemble = EditorGUILayout.ObjectField("TankAssmeble", tankAssemble, typeof(TankAssembleManager), false) as TankAssembleManager;
        return tankAssemble;
    }

    /// <summary>
    /// 创建预设按钮
    /// </summary>
    private void CreateButton()
    {
        if (!GUILayout.Button("Create Prefab"))
            return;
        if (!valid || !tankAssemble.IsValid())
        {
            Debug.LogErrorFormat("TankName Or Modules Be Empty. Or Module Set An Invalid ModuleType.");
            return;
        }

        relativePath = string.Format("{0}{1}{2}", generatePath, tankAssemble.tankName, ".prefab");

        if (createPrefab && System.IO.File.Exists(string.Format("{0}{1}{2}", Application.dataPath, "/", relativePath)))
        {
            Debug.LogErrorFormat("{0} Already Existed : {1}", tankAssemble.tankName, relativePath);
            return;
        }

        if (!AssembleTank(ref newTankPrefab))
            Debug.LogErrorFormat("Create Failed. {0}", "Assets/" + relativePath);
        if (createPrefab && PrefabUtility.CreatePrefab("Assets/" + relativePath, newTankPrefab) == null)
            Debug.LogErrorFormat("Create Failed. {0}", "Assets/" + relativePath);

    }

    /// <summary>
    /// 组装坦克
    /// </summary>
    /// <param name="newTank">新坦克对象</param>
    /// <returns>是否创建成功</returns>
    public bool AssembleTank(ref GameObject newTank)
    {
        newTank = new GameObject(tankAssemble.tankName);
        tankAssemble.InstantiateModules(newTank.transform);
        tankAssemble.AssembleTank();
        return true;
    }
}
