using System;
using Item.Ammo;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPreviewManager : ModulePreviewManager 
{
    public HeadAmmoMap headAmmoMap;
    public AmmoModule targetAmmoModule;
    public TankModuleHead targetHeadModule;

    private void Start()
    {
        AllCustomTankPreviewManager.Instance.allTankSetupHandle += (object sender, EventArgs e) => { SetTargetByCurrentTankAssemble(); };
    }

    public void SetTargetByCurrentTankAssemble()
    {
        if (AllCustomTankManager.Instance.CurrentTankAssemble == null)
            return;
        SetTarget(AllCustomTankManager.Instance.CurrentTankAssemble.head);
    }

    public override void SetTarget(ModuleBase target)
    {
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
    }

    public void Clean()
    {

    }

    public override void OnClicked()
    {
    }

    public override void OnEnter()
    {
        if (targetHeadModule == null)
            return;
        ModulePropertyPanelManager.Instance.Show(Input.mousePosition, targetAmmoModule.property.moduleName, targetAmmoModule.property.GetAllProperties());
    }

    public override void OnExit()
    {
        ModulePropertyPanelManager.Instance.gameObject.SetActive(false);
    }
}
