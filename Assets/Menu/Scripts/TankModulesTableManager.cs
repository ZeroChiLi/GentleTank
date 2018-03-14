using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankModulesTableManager : MonoBehaviour
{
    public string tableName;                                    // 部件表名
    public string resourcePath;                                 // 部件所在资源目录相对地址
    public GameObject modulePreviewPrefab;                      // 部件预览图标
    public List<TankModulePreviewManager> modulePreviewList;    // 部件预览列表    
    public RectTransform rectTransform;                         // 对应转换

    private TankModule[] moduleArray;
    private TankModule temModule;                               // 临时坦克部件
    private TankModulePreviewManager temModulePreview;          // 临时部件预览
    private GridLayoutGroup girdLayoutGroup;                    // 网格布局

    /// <summary>
    /// 读取文件列表，获取部件并填充到自定义坦克部件表中
    /// </summary>
    private void Awake()
    {
        modulePreviewList = new List<TankModulePreviewManager>();
        rectTransform = GetComponent<RectTransform>();
        girdLayoutGroup = GetComponent<GridLayoutGroup>();
        GetModuleList();
        SetupModulePreview();
    }

    /// <summary>
    /// 根据物品数量重置活动窗口大小
    /// </summary>
    private void LateUpdate()
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, girdLayoutGroup.preferredHeight);
    }

    /// <summary>
    /// 获取文件目录下所有坦克部件
    /// </summary>
    private void GetModuleList()
    {
        moduleArray = Resources.LoadAll<TankModule>(resourcePath);
    }

    /// <summary>
    /// 配置所有模型预览
    /// </summary>
    private void SetupModulePreview()
    {
        for (int i = 0; i < moduleArray.Length; i++)
        {
            temModulePreview = Instantiate(modulePreviewPrefab, transform).GetComponent<TankModulePreviewManager>();
            if (temModulePreview == null)
                continue;
            temModulePreview.SetTarget(moduleArray[i]);
            modulePreviewList.Add(temModulePreview);
        }
    }

}
