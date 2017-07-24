using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour 
{
    /// <summary>
    /// 加载单人模式场景
    /// </summary>
    public void LoadSoloScene()
    {
        SceneManager.LoadScene(1);
    }
}
