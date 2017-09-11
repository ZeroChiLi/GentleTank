using UnityEngine;
using UnityEngine.UI;

public class TankModulePreviewManager : MonoBehaviour 
{
    public TankModule module;
    public Image backgroundImage;
    public Image previewImage;

    private TankModuleManager currentTank;
    private TankModule temModule;
    private GameObject temModuleObj;

    /// <summary>
    /// 绑定对应坦克部件
    /// </summary>
    /// <param name="module">目标部件</param>
    public void SetTargetModule(TankModule module)
    {
        this.module = module;
        previewImage.sprite = module.preview;
    }

    /// <summary>
    /// 鼠标点击
    /// </summary>
    public void OnClicked()
    {
        //if (CustomTankMenuManager.Instance.selectedModule == this)
        //    return;
        AllCustomTankManager.Instance.PreviewNewModule(this);
        //PreviewTankModule();
    }

    /// <summary>
    /// 预览坦克部件
    /// </summary>
    private void PreviewTankModule()
    {
        //if (AllCustomTankPreviewManager.Instance.CurrentTank == null || module == null)
        //    return;
        //currentTank = AllCustomTankPreviewManager.Instance.CurrentTank.GetComponent<TankModuleManager>();
        //currentTank.PreviewModule(module);
    }


}
