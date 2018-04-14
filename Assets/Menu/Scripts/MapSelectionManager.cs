using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelectionManager : MonoBehaviour
{
    public Dropdown dropdown;
    public Image mapPreview;
    public string[] sceneNames;

    public void SelectCurrentMap()
    {
        mapPreview.sprite = dropdown.options[dropdown.value].image;
    }

    public void LoadSelectedScene()
    {
        SceneManager.LoadScene(sceneNames[dropdown.value]);
    }

}
