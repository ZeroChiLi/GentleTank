using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour 
{
    public EasyTween title;                 // 标题
    public GameObject mainCanvas;           // 主菜单界面
    public GameObject mutiplayerCanvas;     // 多人模式菜单

    /// <summary>
    /// 只显示主菜单
    /// </summary>
    private void Awake()
    {
        BackToMainMenu();
    }

    private void Start()
    {
        title.OpenCloseObjectAnimation();
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
        mainCanvas.SetActive(false);
        mutiplayerCanvas.SetActive(true);
    }

    /// <summary>
    /// 回到主菜单
    /// </summary>
    public void BackToMainMenu()
    {
        mainCanvas.SetActive(true);
        mutiplayerCanvas.SetActive(false);
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
