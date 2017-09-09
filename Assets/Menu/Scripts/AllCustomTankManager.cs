using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AllCustomTankManager : MonoBehaviour 
{
    public List<TankAssembleManager> defaultTankAssembleList;
    public string customTankPath = "/Items/Tank/TankAssemble/Custom";       // 自定义坦克相对路径
    public Animator tankExhibition;                     // 坦克展台（自动旋转）
    public Vector3 tankOffset = new Vector3(10, 0, 0);        // 坦克之间偏移量
    public Vector3 tankStartRotation = new Vector3(0, 150, 0);      // 坦克初始旋转角

    private string fullCustomTankPath { get { return Application.dataPath + customTankPath; } }
    private List<TankAssembleManager> customTankAssembleList;
    private List<GameObject> defaultTankList;           // 默认坦克列表
    private List<GameObject> customTankList;            // 自定义坦克列表
    private int currentIndex;                           // 当前选中坦克索引

    // 坦克列表索引器（从默认到自定义）
    public GameObject this[int index]
    {
        get
        {
            if (index < 0 || index >= defaultTankList.Count + customTankList.Count)
                return null;
            return index < defaultTankList.Count ? defaultTankList[index] : customTankList[index - defaultTankList.Count];
        }
    }

    public GameObject CurrentTank { get { return this[currentIndex]; } }

    private void Awake()
    {
        customTankAssembleList = new List<TankAssembleManager>();
        defaultTankList = new List<GameObject>();
        customTankList = new List<GameObject>();
    }

    public void CreateAllTanks()
    {
        CreateTanks(defaultTankAssembleList);
        GetAllCustomTankAssemble();
        CreateTanks(customTankAssembleList);
    }

    public void CreateTanks(List<TankAssembleManager> assembleList)
    {
        if (assembleList == null)
            return;
        for (int i = 0; i < assembleList.Count; i++)
            defaultTankList.Add(Instantiate(assembleList[i].CreateTank(), transform));
    }

    public void GetAllCustomTankAssemble()
    {
        if (!Directory.Exists(fullCustomTankPath))
        {
            Debug.LogError(fullCustomTankPath + " Doesn't Exists");
            return;
        }

        FileInfo[] files = new DirectoryInfo(fullCustomTankPath).GetFiles("*.asset", SearchOption.AllDirectories);
        TankAssembleManager tankAssemble;

        for (int i = 0; i < files.Length; i++)
        {
            tankAssemble = AssetDatabase.LoadAssetAtPath<TankAssembleManager>(string.Format("{0}{1}{2}{3}", "Assets", customTankPath, "/", files[i].Name)) as TankAssembleManager;
            if (tankAssemble != null)
                customTankAssembleList.Add(tankAssemble);
            else
                Debug.LogWarningFormat("Instantiate Failed. Assets{0}/{1}", customTankPath, files[i].Name);
        }
    }

}
