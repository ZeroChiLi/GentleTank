using UnityEngine;
using UnityEngine.UI;

public class ColorSelection : MonoBehaviour 
{
    public Image image;

    public Color color { get { return image.color; } set { image.color = value; } }

    public void Start()
    {
        color = MasterManager.Instance.data.representColor;
    }

    public void OnClicked()
    {
        color = ColorTool.random;
        MasterManager.Instance.data.representColor = color;
    }
}
