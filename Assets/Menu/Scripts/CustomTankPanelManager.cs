using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomTankPanelManager : MonoBehaviour 
{
    static private CustomTankPanelManager instance;
    [HideInInspector]
    static public CustomTankPanelManager Instance { get { return instance; } }

    public TextMeshProUGUI menuText;
    public Button changeBtn;
    public TankModulePreviewManager selectedModule;
    public List<TankModulesTableManager> moduleTables;

    private int currentIndex;

    private void Awake()
    {
        instance = this;
        currentIndex = 0;
        menuText.text = moduleTables[currentIndex].tableName;
        changeBtn.interactable = false;
    }

    /// <summary>
    /// 隐藏所有表
    /// </summary>
    private void SetAllTableInActive()
    {
        for (int i = 0; i < moduleTables.Count; i++)
            moduleTables[i].gameObject.SetActive(false);
        selectedModule = null;
    }

    /// <summary>
    /// 换页，参数为true为换下一页，否则为上一页
    /// </summary>
    /// <param name="positive">是否下一页</param>
    public void SkipTable(bool positive)
    {
        SetAllTableInActive();
        currentIndex += (positive ? 1 : -1) + moduleTables.Count;       // 避免小于0
        currentIndex %= moduleTables.Count;
        menuText.text = moduleTables[currentIndex].tableName;
        moduleTables[currentIndex].gameObject.SetActive(true);
    }

    /// <summary>
    /// 设置当前选择的部件
    /// </summary>
    /// <param name="module">部件预览对象</param>
    public void SetSelectedModule(TankModulePreviewManager module)
    {
        CleanSelectedModule();
        selectedModule = module;
        changeBtn.interactable = selectedModule == null ? false : true;
    }

    /// <summary>
    /// 清除当前选中的部件
    /// </summary>
    public void CleanSelectedModule()
    {
        changeBtn.interactable = false;
        if (selectedModule != null)
            selectedModule.ResetPreview();
    }


}
