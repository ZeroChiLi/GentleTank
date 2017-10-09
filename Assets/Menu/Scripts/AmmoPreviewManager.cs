using System;
using Item.Ammo;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPreviewManager : ModulePreviewManager 
{
    public HeadAmmoMap headAmmoMap;             // 头部对应弹药表
    public AmmoModule targetAmmoModule;         // 目标弹药表
    public TankModuleHead targetHeadModule;     // 目标头部

    /// <summary>
    /// 通过当前坦克组合设置弹药类型
    /// </summary>
    public void SetTargetByCurrentTankAssemble()
    {
        if (AllCustomTankManager.Instance.CurrentTankAssemble == null)
        {
            Clean();
            return;
        }
        SetTarget(AllCustomTankManager.Instance.CurrentTankAssemble.head);
    }

    /// <summary>
    /// 设置目标
    /// </summary>
    /// <param name="target">目标部件</param>
    public override void SetTarget(ModuleBase target)
    {
        this.target = target;
        targetHeadModule = target as TankModuleHead;
        if (targetHeadModule == null)
            return;
        targetAmmoModule = headAmmoMap.GetAmmo(targetHeadModule);
        if (!targetAmmoModule)
        {
            Clean();
            return;
        }
        previewImage.sprite = targetAmmoModule.preview;
        previewImage.enabled = true;
    }

    /// <summary>
    /// 清除信息
    /// </summary>
    public void Clean()
    {
        target = null;
        targetAmmoModule = null;
        targetHeadModule = null;
        previewImage.enabled = false;
        previewImage.sprite = null;
    }

    public override void OnClicked()
    {
    }

    public override void OnEnter()
    {
        if (targetHeadModule == null)
            return;
        ModulePropertyPanelManager.Instance.Show(Input.mousePosition, targetAmmoModule.moduleName, targetAmmoModule.property.GetPropertiesString());
    }

    public override void OnExit()
    {
        ModulePropertyPanelManager.Instance.gameObject.SetActive(false);
    }
}
