using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TankModulesTableManager : MonoBehaviour
{
    public string tableName;                                    // 部件表名
    public string path = "/Items/Tank/TankModule/";             // 部件相对地址
    public GameObject modulePreviewPrefab;                      // 部件预览图标
    public List<TankModulePreviewManager> modulePreviewList;    // 部件预览列表    

    private string fullPath { get { return Application.dataPath + path; } }     // 绝对地址
    private List<TankModule> moduleList;                        // 部件列表
    private TankModule temModule;                               // 临时坦克部件
    private TankModulePreviewManager temModulePreview;          // 临时部件预览

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
            Debug.LogError(fullPath + " Doesn't Exists");
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
