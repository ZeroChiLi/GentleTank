using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AllCustomTankManager : MonoBehaviour
{
    public const int MaxSize = 10;                                      // 最大坦克数量
    public const string ResourcesPath = "TankAssemble/Custom";          // 自定义坦克组合的资源路径

    static public AllCustomTankManager Instance { get; private set; }

    public Animator tankExhibition;                                     // 坦克展台（自动旋转）
    public Vector3 tankOffset = new Vector3(10, 0, 0);                  // 坦克之间偏移量
    public Vector3 tankStartRotation = new Vector3(0, 150, 0);          // 坦克初始旋转角
    public CurrentTankPanelManager tankPanel;
    public TankAssembleManager defaultTankAssemble;                     // 默认坦克组装（用来创建）

    public CatchTextureCameraRig catchTextureCam;       // 捕获纹理相机设备
    public List<RenderTexture> textureList;             // 纹理列表

    //public UnityEvent OnAllTankSetupEvent;              // 所有坦克配置好后响应
    public UnityEvent OnTankPreviewClickedEvent;        // 坦克预览选项点击后事件

    public TankAssembleManager TemporaryAssemble { get { return temTankAssemble; } }
    private TankAssembleManager temTankAssemble;
    public GameObject TemporaryTankObject { get { return temTankObject; } set { temTankObject = value; } }
    private GameObject temTankObject;
    public int CurrentIndex { get { return currentIndex; } }
    private int currentIndex;                           // 当前选中坦克索引

    public int Count { get { return customTankList.Count; } }
    public GameObject this[int index] { get { return index >= Count ? null : customTankList[index]; } set { customTankList[index] = value; } }
    public GameObject CurrentTank { get { return this[currentIndex]; } set { this[currentIndex] = value; } }
    public TankAssembleManager CurrentTankAssemble { get { return currentIndex >= Count ? null : tankAssembleList[currentIndex]; } set { tankAssembleList[currentIndex] = value; } }

    public List<TankAssembleManager> tankAssembleList = new List<TankAssembleManager>();
    private List<GameObject> customTankList = new List<GameObject>();            // 自定义坦克列表
    private GameObject newTank;

    /// <summary>
    /// 初始化单例
    /// </summary>
    private void Awake()
    {
        Instance = this;
        if (tankAssembleList == null)
            tankAssembleList = new List<TankAssembleManager>();
    }

    private void Start()
    {
        CreateAllTanks();
        SetupAllTanksPosition();
        //OnAllTankSetupEvent.Invoke();
        SetupAllTankTexture();
        SelectMasterTank();
        OnTankPreviewClickedEvent.Invoke();
    }

    /// <summary>
    /// 创建所有坦克（默认和自定义）
    /// </summary>
    private void CreateAllTanks()
    {
        tankAssembleList = new List<TankAssembleManager>(Resources.LoadAll<TankAssembleManager>(ResourcesPath));
        for (int i = 0; i < tankAssembleList.Count; i++)
            if (tankAssembleList[i].IsValid())
                customTankList.Add(tankAssembleList[i].CreateTank(transform));
    }

    /// <summary>
    /// 设置所有坦克的位置
    /// </summary>
    private void SetupAllTanksPosition()
    {
        for (int i = 0; i < customTankList.Count; i++)
            SetupTankPos(customTankList[i].transform, i);
    }

    /// <summary>
    /// 设置单个坦克位置
    /// </summary>
    /// <param name="tankTransform">坦克的转换信息</param>
    /// <param name="index">当前索引值</param>
    private void SetupTankPos(Transform tankTransform, int index)
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
    private void ResetTemTankAssemble()
    {
        temTankAssemble = ScriptableObject.CreateInstance<TankAssembleManager>();
        temTankAssemble.CopyFrom(CurrentTankAssemble);
        temTankAssemble.name = "TemporaryPreviewTank";
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
        switch (TankModule.GetModuleType(modulePreview.target as TankModule))
        {
            case TankModule.TankModuleType.Head:
                if (modulePreview.target == TemporaryAssemble.head)
                    return;
                else
                    TemporaryAssemble.head = modulePreview.target as TankModuleHead;
                break;
            case TankModule.TankModuleType.Body:
                if (modulePreview.target == TemporaryAssemble.body)
                    return;
                else
                    TemporaryAssemble.body = modulePreview.target as TankModuleBody;
                break;
            case TankModule.TankModuleType.Wheel:
                if (modulePreview.target == TemporaryAssemble.leftWheel)
                    return;
                else
                    TemporaryAssemble.leftWheel = modulePreview.target as TankModuleWheel;
                break;
            //case TankModule.TankModuleType.Other:
            //    if (TemporaryAssemble.others.Contains(modulePreview.target as TankModuleOther))
            //        TemporaryAssemble.others.Remove(modulePreview.target as TankModuleOther);
            //    else
            //        TemporaryAssemble.others.Add(modulePreview.target as TankModuleOther);
            //    break;
            case TankModule.TankModuleType.Cap:
                TemporaryAssemble.cap = modulePreview.target == TemporaryAssemble.cap ? null : modulePreview.target as TankModuleCap;
                break;
            case TankModule.TankModuleType.Face:
                TemporaryAssemble.face = modulePreview.target == TemporaryAssemble.face ? null : modulePreview.target as TankModuleFace;
                break;
            case TankModule.TankModuleType.BodyForward:
                TemporaryAssemble.bodyForward = modulePreview.target == TemporaryAssemble.bodyForward ? null : modulePreview.target as TankModuleBodyForward;
                break;
            case TankModule.TankModuleType.BodyBack:
                TemporaryAssemble.bodyBack = modulePreview.target == TemporaryAssemble.bodyBack ? null : modulePreview.target as TankModuleBodyBack;
                break;
            default:
                return;
        }

        PreviewTemporaryTank();
    }

    /// <summary>
    /// 清除临时坦克对象
    /// </summary>
    private void CleanTemTankObj(bool alsoAssemble = true)
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
        TemporaryTankObject.name = CurrentTankAssemble.name;
        CurrentTank = TemporaryTankObject;
        TemporaryTankObject = null;
        ResetTemTankAssemble();
    }

    /// <summary>
    /// 添加新的坦克组装
    /// </summary>
    /// <param name="tankAssemble">目标坦克组装</param>
    /// <returns>返回新的坦克</returns>
    public GameObject AddNewTank()
    {
        tankAssembleList[Count].CopyFrom(defaultTankAssemble);
        newTank = tankAssembleList[Count].CreateTank(transform);
        customTankList.Add(newTank);
        SetupTankPos(newTank.transform, Count - 1);
        return newTank;
    }

    /// <summary>
    /// 删除当前坦克
    /// </summary>
    public void DeleteCurrentTank()
    {
        if (CurrentTank == null)
            return;
        CurrentTankAssemble.Clear();
        Destroy(CurrentTank);
        customTankList.Remove(CurrentTank);
        for (int i = CurrentIndex; i < Count; i++)
        {
            tankAssembleList[i].CopyFrom(tankAssembleList[i + 1]);
            tankAssembleList[i + 1].Clear();
        }
        SetupAllTanksPosition();
    }

    /// <summary>
    /// 获取当前坦克与临时坦克的重量差值（当前 - 临时）
    /// </summary>
    /// <returns>当前坦克与临时坦克的重量差值</returns>
    public float GetTemAndCurrentWeightDifference()
    {
        return TemporaryAssemble.GetTotalWeight() - CurrentTankAssemble.GetTotalWeight();
    }

    /// <summary>
    /// 选择当前坦克组合，保存到主人配置中
    /// </summary>
    /// <returns>成功返回true</returns>
    public bool SetCurrentTankToMaster()
    {
        if (CurrentTankAssemble == null)
        {
            Toast.Instance.ShowToast("选择失败。");
            return false;
        }
        MasterManager.Instance.SelectedTank = CurrentTankAssemble;
        return true;
    }

    /// <summary>
    /// 选中主角坦克
    /// </summary>
    public void SelectMasterTank()
    {
        if (MasterManager.Instance.SelectedTank == null)
            SelectCurrentTank(0);
        for (int i = 0; i < tankAssembleList.Count; i++)
            if (MasterManager.Instance.SelectedTank == tankAssembleList[i])
                SelectCurrentTank(i);
    }

    #region 坦克预览纹理相关

    /// <summary>
    /// 配置所有坦克预览纹理
    /// </summary>
    public void SetupAllTankTexture()
    {
        for (int i = 0; i < textureList.Count; i++)
        {
            if (this[i] == null)
                textureList[i].Release();
            else
                catchTextureCam.RenderTarget(this[i].transform, textureList[i]);
        }
    }

    /// <summary>
    /// 捕获当前坦克的纹理
    /// </summary>
    public void CatchCurrentTankTexture()
    {
        CatchTankTexture(CurrentIndex);
    }

    /// <summary>
    /// 捕获新的纹理
    /// </summary>
    /// <param name="index">指定索引值</param>
    public void CatchTankTexture(int index)
    {
        if (this[index] != null)
            catchTextureCam.RenderTarget(this[index].transform, textureList[index]);
    }

    /// <summary>
    /// 当坦克预览点击时响应
    /// </summary>
    public void OnTankPreviewClicked()
    {
        OnTankPreviewClickedEvent.Invoke();
    }

    #endregion
}
