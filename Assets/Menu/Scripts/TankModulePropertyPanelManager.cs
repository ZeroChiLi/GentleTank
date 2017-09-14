using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TankModulePropertyPanelManager : MonoBehaviour 
{
    static private TankModulePropertyPanelManager instance;
    static public TankModulePropertyPanelManager Instance { get { return instance; } }

    public RectTransform rectTransform;
    public Text titleText;
    public Text propertyText;
    public List<string> propertyStrList;

    private StringBuilder str;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

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
