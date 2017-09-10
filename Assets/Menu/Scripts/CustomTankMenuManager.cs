using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomTankMenuManager : MonoBehaviour 
{
    static private CustomTankMenuManager instance;
    [HideInInspector]
    static public CustomTankMenuManager Instance { get { return instance; } }

    public AllCustomTankManager allCustomTank;
    public TextMeshProUGUI menuText;
    public Button changeBtn;
    public List<TankModulesTableManager> moduleTables;

    private int currentIndex;
    private GameObject temPreviewTank;

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
    }

    /// <summary>
    /// 换页，参数为true为换下一页，否则为上一页，循环的
    /// </summary>
    /// <param name="positive">是否下一页</param>
    public void SkipTable(bool positive)
    {
        if (temPreviewTank != null)
        {
            allCustomTank.CurrentTank.SetActive(true);
            allCustomTank.ResetTemTankAssemble();
            Destroy(temPreviewTank);
            temPreviewTank = null;
        }
        SetAllTableInActive();
        currentIndex += (positive ? 1 : -1) + moduleTables.Count;       // 避免小于0
        currentIndex %= moduleTables.Count;
        menuText.text = moduleTables[currentIndex].tableName;
        moduleTables[currentIndex].gameObject.SetActive(true);
    }

    /// <summary>
    /// 设置当前选择的部件
    /// </summary>
    /// <param name="modulePreview">部件预览对象</param>
    public void SetSelectedModule(TankModulePreviewManager modulePreview)
    {
        switch (TankModule.GetModuleType(modulePreview.module))
        {
            case TankModule.TankModuleType.Head:
                if (modulePreview.module == allCustomTank.CurrentTemAssemble.head)
                    return;
                else
                    allCustomTank.CurrentTemAssemble.head = modulePreview.module as TankModuleHead;
                break;
            case TankModule.TankModuleType.Body:
                if (modulePreview.module == allCustomTank.CurrentTemAssemble.body)
                    return;
                else
                    allCustomTank.CurrentTemAssemble.body = modulePreview.module as TankModuleBody;
                break;
            case TankModule.TankModuleType.Wheel:
                if (modulePreview.module == allCustomTank.CurrentTemAssemble.leftWheel)
                    return;
                else
                    allCustomTank.CurrentTemAssemble.leftWheel = modulePreview.module as TankModuleWheel;
                break;
            //case TankModule.TankModuleType.Other:
            //    break;
            default:
                return;
        }

        PreviewChange();
    }

    public void PreviewChange()
    {
        if (temPreviewTank != null)
        {
            Destroy(temPreviewTank);
            temPreviewTank = null;
        }
        allCustomTank.CurrentTank.SetActive(false);
        temPreviewTank = allCustomTank.CurrentTemAssemble.CreateTank(allCustomTank.tankExhibition.transform);
        allCustomTank.ResetCurrentTankAnimation();
        //GameMathf.ResetTransform(temPreviewTank.transform);
        allCustomTank.SetupTankPos(temPreviewTank.transform,allCustomTank.CurrentIndex);
    }

    /// <summary>
    /// 提交修改
    /// </summary>
    public void CommitChange()
    {
        //AllCustomTankPreviewManager.Instance.CurrentTank.GetComponent<TankModuleManager>().CommitChange();
        //allCustomTank.CurrentTankAssemble
    }

}
