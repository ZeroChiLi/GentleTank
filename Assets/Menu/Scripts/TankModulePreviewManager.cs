using UnityEngine;
using UnityEngine.UI;

public class TankModulePreviewManager : MonoBehaviour 
{
    public TankModule targetModule;
    public Image backgroundImage;
    public Image previewImage;
    public bool isLocked = false;
    public Color normalColor = new Color(1, 1, 1, 0.4f);
    public Color hightlightColor = new Color(0.4f, 1, 0.4f, 0.4f);
    public Color pressedColor = new Color(0.2f, 0.5f, 0.2f, 0.4f);
    public Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private void OnEnable()
    {
        ResetPreview();
    }

    public void ResetPreview()
    {
        backgroundImage.color = isLocked ? disabledColor : normalColor;
    }

    /// <summary>
    /// 绑定对应坦克部件
    /// </summary>
    /// <param name="module">目标部件</param>
    public void SetTargetModule(TankModule module)
    {
        targetModule = module;
        previewImage.sprite = module.preview;
    }

    /// <summary>
    /// 鼠标进入
    /// </summary>
    public void OnPointerEnter()
    {
        if (!isLocked && backgroundImage.color == normalColor && backgroundImage.color != pressedColor)
            backgroundImage.color = hightlightColor;
    }

    /// <summary>
    /// 鼠标出去
    /// </summary>
    public void OnPointerExit()
    {
        if (!isLocked && backgroundImage.color == hightlightColor)
            backgroundImage.color = normalColor;
    }

    /// <summary>
    /// 鼠标点击
    /// </summary>
    public void OnPointerClicked()
    {
        if (isLocked)
            return;
        backgroundImage.color = pressedColor;
        CustomTankPanelManager.Instance.SetSelectedModule(this);
    }


}
