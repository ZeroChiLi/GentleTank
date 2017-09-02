using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TankModulesTableManager : MonoBehaviour
{
    public string tableName;
    public string path = "/Items/Tank/TankModule/";
    public TankModuleType moduleType;
    public GameObject modulePreviewPrefab;
    public List<TankModulePreviewManager> modulePreviewList;

    private List<TankModule> moduleList;
    private string fullPath { get { return Application.dataPath + path; } }
    private TankModule temModule;
    private TankModulePreviewManager temModulePreview;

    /// <summary>
    /// 读取文件列表，获取部件并填充到自定义坦克部件表中
    /// </summary>
    private void Awake()
    {
        modulePreviewList = new List<TankModulePreviewManager>();
        moduleList = new List<TankModule>();
        GetModuleList();
        SetupModulePreview();
    }

    /// <summary>
    /// 获取文件目录下所有坦克部件
    /// </summary>
    private void GetModuleList()
    {
        if (!Directory.Exists(fullPath))
        {
            Debug.Log(fullPath + " Doesn't Exists");
            return;
        }

        FileInfo[] files = new DirectoryInfo(fullPath).GetFiles("*.asset", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            temModule = AssetDatabase.LoadAssetAtPath<TankModule>(string.Format("{0}{1}{2}{3}", "Assets", path, "/", files[i].Name));
            if (temModule != null)
                moduleList.Add(temModule);
        }
    }

    /// <summary>
    /// 配置所有模型预览
    /// </summary>
    private void SetupModulePreview()
    {
        for (int i = 0; i < moduleList.Count; i++)
        {
            temModulePreview = Instantiate(modulePreviewPrefab, transform).GetComponent<TankModulePreviewManager>();
            if (temModulePreview == null)
                continue;
            temModulePreview.SetTargetModule(moduleList[i]);
            modulePreviewList.Add(temModulePreview);
        }
    }

}
