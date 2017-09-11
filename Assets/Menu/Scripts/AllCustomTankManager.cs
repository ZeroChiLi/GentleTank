using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AllCustomTankManager : MonoBehaviour 
{
    static private AllCustomTankManager instance;
    [HideInInspector]
    static public AllCustomTankManager Instance { get { return instance; } }

    public List<TankAssembleManager> defaultTankAssembleList;
    public string customTankPath = "/Items/Tank/TankAssemble/Custom";       // 自定义坦克相对路径
    public Animator tankExhibition;                     // 坦克展台（自动旋转）
    public Vector3 tankOffset = new Vector3(10, 0, 0);        // 坦克之间偏移量
    public Vector3 tankStartRotation = new Vector3(0, 150, 0);      // 坦克初始旋转角

    public TankAssembleManager TemporaryAssemble { get { return temTankAssemble; } }
    private TankAssembleManager temTankAssemble;

    private string fullCustomTankPath { get { return Application.dataPath + customTankPath; } }
    private List<TankAssembleManager> customTankAssembleList = new List<TankAssembleManager>();
    private List<GameObject> defaultTankList = new List<GameObject>();           // 默认坦克列表
    private List<GameObject> customTankList = new List<GameObject>();            // 自定义坦克列表
    private int currentIndex;                           // 当前选中坦克索引
    private GameObject temTankObject;

    // 坦克列表索引器（从默认到自定义）
    public GameObject this[int index]
    {
        get
        {
            if (IsValidIndex(index))
                return index < defaultTankList.Count ? defaultTankList[index] : customTankList[index - defaultTankList.Count];
            return null;
        }
        set
        {
            if (IsValidIndex(index))
            {
                if (index < defaultTankList.Count)
                    defaultTankList[index] = value;
                else
                    customTankList[index - defaultTankList.Count] = value;
            }
        }
    }

    public int CurrentIndex { get { return currentIndex; } }
    public GameObject CurrentTank { get { return this[currentIndex]; } private set { this[currentIndex] = value; } }

    private void Awake()
    {
        instance = this;
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
        if (CurrentTank != null)     // 重置上一个选中的坦克位置
        {
            CurrentTank.transform.SetParent(transform);
            CurrentTank.SetActive(true);
        }
        currentIndex = index;
        if (CurrentTank != null)
        {
            ResetCurrentTank();
            ResetExhibitionTank(CurrentTank.transform);
        }
    }

    /// <summary>
    /// 是否是有效索引（索引对应有坦克）
    /// </summary>
    /// <param name="index">索引值</param>
    /// <returns>是否有效</returns>
    public bool IsValidIndex(int index)
    {
        return index >= 0 && index < defaultTankList.Count + customTankList.Count;
    }

    /// <summary>
    /// 获取当前坦克的组合装配
    /// </summary>
    /// <returns>当前坦克的组合</returns>
    public TankAssembleManager GetCurrentTankAssemble()
    {
        if (currentIndex < 0 || currentIndex >= defaultTankList.Count + customTankList.Count)
            return null;
        return currentIndex < defaultTankAssembleList.Count ? defaultTankAssembleList[currentIndex] : customTankAssembleList[currentIndex - defaultTankList.Count];
    }

    /// <summary>
    /// 重置当前临时坦克组装
    /// </summary>
    private void ResetTemTankAssemble()
    {
        temTankAssemble = ScriptableObject.CreateInstance<TankAssembleManager>();
        temTankAssemble.CopyFrom(GetCurrentTankAssemble());
        temTankAssemble.tankName = "TemporaryPreviewTank";
    }

    /// <summary>
    /// 重置展台坦克
    /// </summary>
    private void ResetExhibitionTank(Transform targetTank)
    {
        tankExhibition.transform.localPosition = targetTank.localPosition;
        targetTank.SetParent(tankExhibition.transform);
        tankExhibition.SetTrigger("Reset");
        targetTank.localRotation = Quaternion.Euler(tankStartRotation);
    }

    /// <summary>
    /// 预览新的坦克部件
    /// </summary>
    /// <param name="modulePreview">目标部件</param>
    public void PreviewNewModule(TankModulePreviewManager modulePreview)
    {
        switch (TankModule.GetModuleType(modulePreview.module))
        {
            case TankModule.TankModuleType.Head:
                if (modulePreview.module == TemporaryAssemble.head)
                    return;
                else
                    TemporaryAssemble.head = modulePreview.module as TankModuleHead;
                break;
            case TankModule.TankModuleType.Body:
                if (modulePreview.module == TemporaryAssemble.body)
                    return;
                else
                    TemporaryAssemble.body = modulePreview.module as TankModuleBody;
                break;
            case TankModule.TankModuleType.Wheel:
                if (modulePreview.module == TemporaryAssemble.leftWheel)
                    return;
                else
                    TemporaryAssemble.leftWheel = modulePreview.module as TankModuleWheel;
                break;
            //case TankModule.TankModuleType.Other:
            //    break;
            default:
                return;
        }

        PreviewTemporaryTank();
    }

    /// <summary>
    /// 清除临时坦克对象
    /// </summary>
    public void CleanTemTankObj()
    {
        if (temTankObject != null)
        {
            Destroy(temTankObject);
            temTankObject = null;
        }
    }

    /// <summary>
    /// 预览临时坦克
    /// </summary>
    /// <param name="hideCurrentTank">是否隐藏当前坦克</param>
    private void PreviewTemporaryTank(bool hideCurrentTank = true)
    {
        CleanTemTankObj();
        if (hideCurrentTank && CurrentTank != null)
            CurrentTank.SetActive(false);
        temTankObject = TemporaryAssemble.CreateTank(transform);
        SetupTankPos(temTankObject.transform, currentIndex);
        ResetExhibitionTank(temTankObject.transform);
    }

    /// <summary>
    /// 重置当前坦克
    /// </summary>
    /// <param name="cleanTemTank">是否清除临时坦克</param>
    public void ResetCurrentTank(bool cleanTemTank = true)
    {
        CurrentTank.SetActive(true);
        ResetTemTankAssemble();
        if (cleanTemTank)
            CleanTemTankObj();
    }

    /// <summary>
    /// 提交修改（部件修改）
    /// </summary>
    public void CommitChange()
    {
        if (temTankObject == null)
            return;
        TankAssembleManager tankAssemble = GetCurrentTankAssemble();
        tankAssemble.CopyFrom(temTankAssemble);
        Destroy(CurrentTank);
        temTankObject.name = tankAssemble.tankName;
        CurrentTank = temTankObject;
        temTankObject = null;
        ResetTemTankAssemble();
    }

}
