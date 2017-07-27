using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour 
{
    public Image image;                     // 等待图片
    public float anglePerRotate = -27.5f;   // 每次旋转角度
    public float rate = 0.1f;               // 每次旋转时间周期

    private float elapsed;                  // 计时器

    /// <summary>
    /// 旋转等待图片
    /// </summary>
    private void Update()
    {
        elapsed -= Time.deltaTime;
        if (elapsed < 0f)
        {
            elapsed = rate;
            image.rectTransform.Rotate(0, 0, anglePerRotate);
        }
    }

    /// <summary>
    /// 开始等待
    /// </summary>
    public void StartLoading()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 结束等待
    /// </summary>
    public void EndLoading()
    {
        gameObject.SetActive(false);
    }

}
