using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour 
{
    public Text toastText;          // 文本信息

    private bool isShowed;          // 是否正在显示
    private float elapsed;          // 结束显示时间

    /// <summary>
    /// 规定时间后隐藏提示
    /// </summary>
    private void Update()
    {
        if (!isShowed)
            return;
        elapsed -= Time.deltaTime;
        if (elapsed <= 0f)
        {
            gameObject.SetActive(false);
            isShowed = false;
        }
    }

    /// <summary>
    /// 显示提示
    /// </summary>
    /// <param name="duration">持续时间</param>
    /// <param name="content">显示内容</param>
    public void ShowToast(float duration, string content)
    {
        elapsed = duration;
        isShowed = true;
        toastText.text = content;
        gameObject.SetActive(true);
    }


}
