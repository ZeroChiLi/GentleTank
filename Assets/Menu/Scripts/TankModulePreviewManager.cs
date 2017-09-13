using UnityEngine;
using UnityEngine.UI;

public class TankModulePreviewManager : MonoBehaviour 
{
    public TankModule module;
    public Image backgroundImage;
    public Image previewImage;

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
        if (AllCustomTankManager.Instance.CurrentTank == null)
            return;
        AllCustomTankManager.Instance.PreviewNewModule(this);
        CustomTankMenuManager.Instance.changeBtn.interactable = true;
    }

}
