using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WindowPanel : MonoBehaviour 
{
    public Text titleText;                  // 窗口标题
    public Button closeButton;              // 关闭按钮
    public Text messageText;                // 文本信息
    public Button mainButton;               // 主按钮
    public Text buttonText;                 // 主按钮文本

    /// <summary>
    /// 打开窗口
    /// </summary>
    /// <param name="title">窗口标题</param>
    /// <param name="message">窗口信息</param>
    /// <param name="buttonName">按钮名称</param>
    /// <param name="showCloseBtn">是否显示关闭窗口</param>
    /// <param name="mainBtnCall">主按钮响应事件</param>
    public void OpenWindow(string title,string message,string buttonName,bool showCloseBtn = true, UnityAction mainBtnCall = null)
    {
        titleText.text = title;
        messageText.text = message;
        buttonText.text = buttonName;

        if (showCloseBtn)
            titleText.rectTransform.position = new Vector3(0,-16,0);
        else
            titleText.rectTransform.position = Vector3.zero;

        if (mainBtnCall != null)
            mainButton.onClick.AddListener(mainBtnCall);

        gameObject.SetActive(true);
    }

    /// <summary>
    /// 关闭窗口
    /// </summary>
    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 给主按钮添加事件
    /// </summary>
    public void AddOnButtonClickListener(UnityAction call)
    {
        mainButton.onClick.AddListener(call);
    }

    /// <summary>
    /// 给关闭按钮添加事件
    /// </summary>
    public void AddOnCloseButtonClickListener(UnityAction call)
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(call);
    }

}
