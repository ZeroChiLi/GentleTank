using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AllCustomTankPreviewManager : MonoBehaviour 
{
    public string defaultTankPath = "/Items/Tank/Prefabs/DefaultTanks";     // 默认坦克相对路径
    public string customTankPath = "/Items/Tank/Prefabs/CustomTanks";       // 自定义坦克相对路径
    public IntervalOffsetCam intervalCam;               // 间隔变化相机
    public PostEffectsBase cameraEffect;                // 屏幕后处理特效
    public RenderTexture selectedTexture;               // 选中的预览纹理
    public Transform allTanksParent;                    // 所有坦克的父对象
    public Vector3 tankStartPos;                        // 起始位置（相对）
    public Vector3 offset = new Vector3(10,0,0);        // 坦克之间偏移量
    public Vector3 rotation = new Vector3(0, 150, 0);   // 坦克旋转角
    public List<RenderTexture> textureList;             // 纹理列表

    private string fullDefaultTankPath { get { return Application.dataPath + defaultTankPath; } }
    private string fullCustomTankPath { get { return Application.dataPath + customTankPath; } }
    private List<GameObject> defaultTankList;           // 默认坦克列表
    private List<GameObject> customTankList;            // 自定义坦克列表
    private WaitForSeconds delayTime = new WaitForSeconds(0.1f);    // 渲染延迟
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

    /// <summary>
    /// 获取所有坦克，设置好位置
    /// </summary>
    private void Awake()
    {
        defaultTankList = new List<GameObject>();
        customTankList = new List<GameObject>();
        GetAllPrefabs();
        SetupAllTanksPos();
        StartCoroutine(SetupAllTankTexture());
    }

    /// <summary>
    /// 创建所有预设
    /// </summary>
    private void GetAllPrefabs()
    {
        GetTanks(ref defaultTankList, fullDefaultTankPath, defaultTankPath);
        GetTanks(ref customTankList, fullCustomTankPath, customTankPath);
    }

    /// <summary>
    /// 创建坦克列表
    /// </summary>
    /// <param name="tanksList">创建到的列表</param>
    /// <param name="fullPath">绝对路径</param>
    /// <param name="relativePath">相对路径</param>
    /// <returns>返回是否创建成功</returns>
    private bool GetTanks(ref List<GameObject> tanksList,string fullPath,string relativePath)
    {
        if (!Directory.Exists(fullPath))
        {
            Debug.LogError(fullPath + " Doesn't Exists");
            return false;
        }

        tanksList.Clear();
        FileInfo[] files = new DirectoryInfo(fullPath).GetFiles("*.prefab", SearchOption.AllDirectories);
        GameObject tank;

        for (int i = 0; i < files.Length; i++)
        {
            tank = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("{0}{1}{2}{3}", "Assets", relativePath, "/", files[i].Name))) as GameObject;
            if (tank != null)
                tanksList.Add(tank);
            else
                Debug.LogWarningFormat("Instantiate Failed. Assets{0}/{1}", relativePath, files[i].Name);
        }
        return true;
    }

    /// <summary>
    /// 配置所有坦克位置
    /// </summary>
    private void SetupAllTanksPos()
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
    private void SetupTankPos(Transform tankTransform,int index)
    {
        tankTransform.SetParent(allTanksParent);
        tankTransform.localPosition = tankStartPos + (offset * index);
        tankTransform.localRotation = Quaternion.Euler(rotation);
    }

    /// <summary>
    /// 配置所有坦克预览纹理
    /// </summary>
    private IEnumerator SetupAllTankTexture()
    {
        for (int i = 0; i < textureList.Count; i++)
        {
            if (this[i] == null)
                continue;
            yield return CameraCapture(textureList[i], i, true);
        }
        yield return CameraCapture(selectedTexture, 0, false);
        intervalCam.enableSmooth = true;
    }

    /// <summary>
    /// 捕获相机的画面到指定纹理中
    /// </summary>
    /// <param name="targetTexture">目标纹理</param>
    /// <param name="index">当前索引</param>
    /// <param name="effectActive">是否开启屏幕特效</param>
    private IEnumerator CameraCapture(RenderTexture targetTexture,int index,bool effectActive = false)
    {
        intervalCam.gameObject.SetActive(false);
        cameraEffect.enabled = effectActive;
        intervalCam.camera.targetTexture = targetTexture;
        intervalCam.FollowImmediately(index);
        intervalCam.gameObject.SetActive(true);
        intervalCam.camera.Render();
        yield return delayTime;
    }

}
