using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AllCustomTankManager : MonoBehaviour
{
    static public AllCustomTankManager Instance { get; private set; }

    public int maxSize = 10;                                                // 最大坦克数量
    public string customTankPath = "/Items/Tank/TankAssemble/Custom";       // 自定义坦克相对路径
    public Animator tankExhibition;                                         // 坦克展台（自动旋转）
    public Vector3 tankOffset = new Vector3(10, 0, 0);                      // 坦克之间偏移量
    public Vector3 tankStartRotation = new Vector3(0, 150, 0);              // 坦克初始旋转角
    public CurrentTankPanelManager tankPanel;

    public TankAssembleManager TemporaryAssemble { get { return temTankAssemble; } }
    private TankAssembleManager temTankAssemble;
    public GameObject TemporaryTankObject { get { return temTankObject; } set { temTankObject = value; } }
    private GameObject temTankObject;
    public int CurrentIndex { get { return currentIndex; } }
    private int currentIndex;                           // 当前选中坦克索引

    public int Count { get { return customTankAssembleList.Count; } }
    public GameObject this[int index] { get { return index >= Count ? null : customTankList[index]; } set { customTankList[index] = value; } }
    public GameObject CurrentTank { get { return this[currentIndex]; } set { this[currentIndex] = value; } }
    public TankAssembleManager CurrentTankAssemble { get { return customTankAssembleList[currentIndex]; } set { customTankAssembleList[currentIndex] = value; } }

    private string fullCustomTankPath { get { return Application.dataPath + customTankPath; } }
    private List<TankAssembleManager> customTankAssembleList = new List<TankAssembleManager>();
    private List<GameObject> customTankList = new List<GameObject>();            // 自定义坦克列表
    private GameObject newTank;

    /// <summary>
    /// 初始化单例
    /// </summary>
    private void Awake()
    {
        Instance = this;
        if (customTankAssembleList == null)
            customTankAssembleList = new List<TankAssembleManager>();
    }

    /// <summary>
    /// 添加所有坦克配置完后事件
    /// </summary>
    private void Start()
    {
        AllCustomTankPreviewManager.Instance.allTankSetupHandle += SelectTankOnAllTankSetup;
    }

    /// <summary>
    /// 在所有坦克配置完后选择坦克
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SelectTankOnAllTankSetup(object sender, System.EventArgs e)
    {
        SelectCurrentTank(0);
    }

    /// <summary>
    /// 创建所有坦克（默认和自定义）
    /// </summary>
    public void CreateAllTanks()
    {
        GetAllCustomTankAssemble();
        for (int i = 0; i < customTankAssembleList.Count; i++)
            customTankList.Add(customTankAssembleList[i].CreateTank(transform));
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
        for (int i = 0; i < customTankList.Count; i++)
            SetupTankPos(customTankList[i].transform, i);
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
    /// 重置当前临时坦克组装
    /// </summary>
    public void ResetTemTankAssemble()
    {
        temTankAssemble = ScriptableObject.CreateInstance<TankAssembleManager>();
        temTankAssemble.CopyFrom(CurrentTankAssemble);
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
            case TankModule.TankModuleType.Other:
                if (TemporaryAssemble.others.Contains(modulePreview.module as TankModuleOther))
                    TemporaryAssemble.others.Remove(modulePreview.module as TankModuleOther);
                else
                    TemporaryAssemble.others.Add(modulePreview.module as TankModuleOther);
                break;
            default:
                return;
        }

        PreviewTemporaryTank();
    }

    /// <summary>
    /// 清除临时坦克对象
    /// </summary>
    public void CleanTemTankObj(bool alsoAssemble = true)
    {
        if (temTankObject != null)
        {
            Destroy(temTankObject);
            temTankObject = null;
        }
        if (alsoAssemble)
            temTankAssemble = null;
    }

    /// <summary>
    /// 预览临时坦克
    /// </summary>
    /// <param name="hideCurrentTank">是否隐藏当前坦克</param>
    private void PreviewTemporaryTank(bool hideCurrentTank = true)
    {
        CleanTemTankObj(false);
        if (hideCurrentTank)
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
        if (cleanTemTank)
            CleanTemTankObj();
        if (CurrentTank != null)
        {
            CurrentTank.SetActive(true);
            ResetTemTankAssemble();
        }
    }

    /// <summary>
    /// 提交修改（部件修改）
    /// </summary>
    public void CommitChange()
    {
        if (TemporaryTankObject == null)
            return;
        CurrentTankAssemble.CopyFrom(TemporaryAssemble);
        Destroy(CurrentTank);
        TemporaryTankObject.name = CurrentTankAssemble.tankName;
        CurrentTank = TemporaryTankObject;
        TemporaryTankObject = null;
        ResetTemTankAssemble();
        EditorUtility.SetDirty(CurrentTankAssemble);
    }

    /// <summary>
    /// 获取坦克组装资源路径
    /// </summary>
    /// <param name="index">坦克索引值</param>
    /// <returns>坦克组装资源路径</returns>
    public string GetTankAssembleAssetPath(int index)
    {
        return string.Format("Assets{0}/CustomTank{1}.asset", customTankPath, index);
    }

    /// <summary>
    /// 获取坦克组装绝对路径
    /// </summary>
    /// <param name="index">坦克索引值</param>
    /// <returns>坦克组装绝对路径</returns>
    public string GetTankAssembleFullPath(int index)
    {
        return string.Format("{0}{1}/CustomTank{2}.asset", Application.dataPath, customTankPath, index);
    }

    /// <summary>
    /// 添加新的坦克组装
    /// </summary>
    /// <param name="tankAssemble">目标坦克组装</param>
    /// <returns>返回新的坦克</returns>
    public GameObject AddNewTank(TankAssembleManager tankAssemble)
    {
        tankAssemble.tankName = "CustomTank" + Count;
        customTankAssembleList.Add(tankAssemble);
        newTank = tankAssemble.CreateTank(transform);
        customTankList.Add(newTank);
        SetupTankPos(newTank.transform, Count - 1);
        AssetDatabase.CreateAsset(tankAssemble, GetTankAssembleAssetPath(Count - 1));
        return newTank;
    }

    /// <summary>
    /// 删除当前坦克
    /// </summary>
    public void DeleteCurrentTank()
    {
        if (CurrentTank == null)
            return;
        Destroy(CurrentTank);
        customTankList.Remove(CurrentTank);
        customTankAssembleList.Remove(CurrentTankAssemble);
        AssetDatabase.DeleteAsset(GetTankAssembleAssetPath(currentIndex));
        RenameTankAssembles(currentIndex + 1);
        SetupAllTanksPosition();
    }

    /// <summary>
    /// 重命名坦克组装（删除坦克后，后面的坦克名字尾数减一）
    /// </summary>
    /// <param name="startIndex">起始坦克组装索引</param>
    private void RenameTankAssembles(int startIndex)
    {
        int j = startIndex;
        for (int i = startIndex; i < maxSize; i++)
        {
            if (!File.Exists(GetTankAssembleFullPath(i)))
                continue;
            AssetDatabase.RenameAsset(GetTankAssembleAssetPath(i), "CustomTank" + (j - 1));
            ++j;
        }
    }

    /// <summary>
    /// 获取当前坦克与临时坦克的重量差值（当前 - 临时）
    /// </summary>
    /// <returns>当前坦克与临时坦克的重量差值</returns>
    public float GetTemAndCurrentWeightDifference()
    {
        return TemporaryAssemble.GetTotalWeight() - CurrentTankAssemble.GetTotalWeight();
    }
}
