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
    private List<TankAssembleManager> customTankAssembleList = new List<TankAssembleManager>();
    private List<GameObject> defaultTankList = new List<GameObject>();           // 默认坦克列表
    private List<GameObject> customTankList = new List<GameObject>();            // 自定义坦克列表
    private int currentIndex;                           // 当前选中坦克索引
    private TankAssembleManager temTankAssemble;
    public TankAssembleManager CurrentTemAssemble { get { return temTankAssemble; } }

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

    public int CurrentIndex { get { return currentIndex; } }
    public GameObject CurrentTank { get { return this[currentIndex]; } }
    public TankAssembleManager CurrentTankAssemble { get { return GetTankAssemble(currentIndex); } }

    private void Awake()
    {
        if (defaultTankAssembleList == null)
            defaultTankAssembleList = new List<TankAssembleManager>();
    }

    /// <summary>
    /// 创建所有坦克（默认和自定义）
    /// </summary>
    public void CreateAllTanks()
    {
        CreateTanks(defaultTankAssembleList, defaultTankList);
        GetAllCustomTankAssemble();
        CreateTanks(customTankAssembleList, customTankList);
    }

    /// <summary>
    /// 创建坦克
    /// </summary>
    /// <param name="assembleList">坦克组合列表</param>
    private void CreateTanks(List<TankAssembleManager> assembleList,List<GameObject> targetObjList)
    {
        if (assembleList == null)
            return;
        for (int i = 0; i < assembleList.Count; i++)
            targetObjList.Add(assembleList[i].CreateTank(transform));
    }

    /// <summary>
    /// 通过路径获取所有自定义坦克
    /// </summary>
    private void GetAllCustomTankAssemble()
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

    /// <summary>
    /// 设置所有坦克的位置
    /// </summary>
    public void SetupAllTanksPosition()
    {
        for (int i = 0; i < defaultTankList.Count; i++)
            SetupTankPos(defaultTankList[i].transform, i);

        for (int i = 0; i < customTankList.Count; i++)
            SetupTankPos(customTankList[i].transform, i + defaultTankList.Count);
    }

    /// <summary>
    /// 设置单个坦克位置
    /// </summary>
    /// <param name="tankTransform">坦克的转换信息</param>
    /// <param name="index">当前索引值</param>
    public void SetupTankPos(Transform tankTransform, int index)
    {
        tankTransform.localPosition = tankOffset * index;
        tankTransform.localRotation = Quaternion.Euler(tankStartRotation);
    }

    /// <summary>
    /// 选择当前坦克
    /// </summary>
    /// <param name="index">坦克索引值</param>
    public void SelectCurrentTank(int index)
    {
        if (this[currentIndex] != null)     // 重置上一个选中的坦克位置
            this[currentIndex].transform.SetParent(transform);
        currentIndex = index;
        if (this[index] != null)
        {
            ResetTemTankAssemble();
            ResetCurrentTankAnimation();
        }
    }

    /// <summary>
    /// 重置当前临时坦克组装
    /// </summary>
    public void ResetTemTankAssemble()
    {
        temTankAssemble = new TankAssembleManager(CurrentTankAssemble);
    }

    /// <summary>
    /// 重置当前坦克展台动画
    /// </summary>
    public void ResetCurrentTankAnimation()
    {
        tankExhibition.transform.localPosition = this[currentIndex].transform.localPosition;
        this[currentIndex].transform.SetParent(tankExhibition.transform);
        tankExhibition.SetTrigger("Reset");
        this[currentIndex].transform.localRotation = Quaternion.Euler(tankStartRotation);
    }

    /// <summary>
    /// 获取坦克组合（包括默认和自定义）
    /// </summary>
    /// <param name="index">索引值</param>
    /// <returns>坦克组合</returns>
    public TankAssembleManager GetTankAssemble(int index)
    {
        if (index < 0 || index >= defaultTankList.Count + customTankList.Count)
            return null;
        return index < defaultTankAssembleList.Count ? defaultTankAssembleList[index] : customTankAssembleList[index - defaultTankList.Count];
    }
}
