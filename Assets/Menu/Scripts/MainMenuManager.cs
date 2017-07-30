using UnityEngine;

public class MainMenuManager : MonoBehaviour 
{
    public GameObject MainCanvas;           // 主菜单
    public GameObject MutiplayerCanvas;     // 多人模式菜单

    /// <summary>
    /// 只显示主菜单
    /// </summary>
    private void Awake()
    {
        BackToMainMenu();
    }

    /// <summary>
    /// 加载单人模式场景
    /// </summary>
    public void LoadSoloScene()
    {
        AllSceneManager.LoadScene(GameScene.SoloScene);
    }

    /// <summary>
    /// 转换到多人菜单
    /// </summary>
    public void TurnToMutiplayerMenu()
    {
        MainCanvas.SetActive(false);
        MutiplayerCanvas.SetActive(true);
    }

    /// <summary>
    /// 回到主菜单
    /// </summary>
    public void BackToMainMenu()
    {
        MainCanvas.SetActive(true);
        MutiplayerCanvas.SetActive(false);
    }

    /// <summary>
    /// 加载游戏大厅场景
    /// </summary>
    public void LoadLobbyScene()
    {
        AllSceneManager.LoadScene(GameScene.LobbyScene);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
