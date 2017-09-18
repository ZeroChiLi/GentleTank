using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ModulePropertyPanelManager : MonoBehaviour 
{
    static private ModulePropertyPanelManager instance;
    static public ModulePropertyPanelManager Instance { get { return instance; } }

    public RectTransform rectTransform;
    public Text titleText;
    public Text propertyText;
    public List<string> propertyStrList;

    private StringBuilder str;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示信息，面板锚点为左上角
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="title">标题</param>
    /// <param name="properties">信息字符串数组</param>
    public void Show(Vector3 pos, string title,params string[] properties)
    {
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
    }
}
