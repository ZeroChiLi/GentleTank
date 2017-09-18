using System;
using UnityEngine;
using UnityEngine.UI;

public class TankModulePreviewManager : ModulePreviewManager 
{

    /// <summary>
    /// 鼠标点击
    /// </summary>
    public override void OnClicked()
    {
        if (AllCustomTankManager.Instance.CurrentTank == null)
            return;
        AllCustomTankManager.Instance.PreviewNewModule(this);
        CustomTankMenuManager.Instance.changeBtn.interactable = true;
        CurrentTankInfoUIManager.Instance.UpdateTemporaryTankInfo();
    }

    /// <summary>
    /// 鼠标进入，显示信息
    /// </summary>
    public override void OnEnter()
    {
        ModulePropertyPanelManager.Instance.Show(Input.mousePosition,target.property.moduleName,target.property.GetAllProperties());
    }

    /// <summary>
    /// 鼠标出去，关闭信息
    /// </summary>
    public override void OnExit()
    {
        ModulePropertyPanelManager.Instance.gameObject.SetActive(false);
    }

}
