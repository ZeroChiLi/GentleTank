using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ModulePropertyPanelManager : MonoBehaviour 
{
    static private ModulePropertyPanelManager instance;
    static public ModulePropertyPanelManager Instance { get { return instance; } }

    public RectTransform rectTransform;         // 自身面板的转换
    public Text titleText;                      // 标题文本
    public Text propertyText;                   // 属性列表文本
    public bool autoAdapt = true;               // 是否自适应
    public List<string> propertyStrList;        // 属性字符串列表
    public CanvasScaler scaler;                 // 所在面板缩放大小

    private StringBuilder str;
    private Vector2 scalerFator;
    private Vector2 fixedPos;
    private Vector2 offset;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
        if (scaler == null)
            scaler = GetComponentInParent<CanvasScaler>();
    }

    /// <summary>
    /// 显示信息，面板锚点为左上角
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="title">标题</param>
    /// <param name="properties">信息字符串数组</param>
    public void Show(Vector3 pos, string title,params string[] properties)
    {
        SetDefaultAdaptation();
        gameObject.SetActive(true);
        rectTransform.position = pos;
        titleText.text = title;
        str = new StringBuilder();
        for (int i = 0; i < properties.Length; i++)
        {
            str.Append(properties[i]);
            str.Append("\n");
        }
        propertyText.text = str.ToString();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, titleText.preferredHeight + propertyText.preferredHeight);
        if (autoAdapt)
            AdaptToScreen();
    }

    /// <summary>
    /// 设置默认适配
    /// </summary>
    public void SetDefaultAdaptation()
    {
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
    }

    /// <summary>
    /// 适应到屏幕
    /// </summary>
    public void AdaptToScreen()
    {
        scalerFator = new Vector2(Screen.width / scaler.referenceResolution.x, Screen.height / scaler.referenceResolution.y);
        fixedPos = rectTransform.position;
        offset.x = rectTransform.position.x + rectTransform.sizeDelta.x * scalerFator.x - Screen.width;
        offset.y = rectTransform.position.y - rectTransform.sizeDelta.y * scalerFator.y;
        if (offset.x > 0)
            fixedPos.x -= offset.x;
        if (offset.y < 0)
            fixedPos.y -= offset.y;
        rectTransform.position = fixedPos;
    }
}
