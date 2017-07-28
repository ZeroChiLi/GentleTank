using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button button;                   // 按钮
    public Text buttonText;                 // 按钮文本
    public Color normalColor = Color.black; // 正常字体颜色
    public Color disableColor = Color.gray; // 锁定字体颜色

    /// <summary>
    /// 锁定按钮，且改变字体颜色
    /// </summary>
    /// <param name="enable">是否锁定</param>
    public void Lock(bool enable)
    {
        button.interactable = !enable;
        buttonText.color = enable ? disableColor : normalColor;
    }
}
