using UnityEngine;
using UnityEngine.UI;

public class EmptyPanelManager : MonoBehaviour 
{
    public Color enableColor = Color.white;                 // 可用时背景颜色
    public Color disableColor = Color.gray;                 // 不可用时背景颜色
    public Image backgroundImage;                           // 背景颜色

    private bool isEnable = true;                           // 是否可用

    /// <summary>
    /// 该是否面板可用
    /// </summary>
    public bool IsEnable
    {
        get { return isEnable; }
        set
        {
            isEnable = value;
            backgroundImage.color = isEnable ? enableColor : disableColor;
        }
    }

}
