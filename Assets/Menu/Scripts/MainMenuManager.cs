using UnityEngine;

public class MainMenuManager : MonoBehaviour 
{
    /// <summary>
    /// 加载单人模式场景
    /// </summary>
    public void LoadSoloScene()
    {
        AllSceneManager.LoadScene(GameScene.SoloScene);
    }

    /// <summary>
    /// 加载多人游戏菜单
    /// </summary>
    public void LoadMultiplayerMenuScene()
    {
        AllSceneManager.LoadScene(GameScene.MultiMenuScene);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
