using UnityEngine;

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
        if (CurrentTankInfoUIManager.Instance)
            CurrentTankInfoUIManager.Instance.UpdateTemporaryTankInfo();
    }

    /// <summary>
    /// 鼠标进入，显示信息
    /// </summary>
    public override void OnEnter()
    {
        ModulePropertyPanelManager.Instance.Show(Input.mousePosition, target.moduleName, target.GetProperties());
    }

    /// <summary>
    /// 鼠标出去，关闭信息
    /// </summary>
    public override void OnExit()
    {
        ModulePropertyPanelManager.Instance.gameObject.SetActive(false);
    }
}
