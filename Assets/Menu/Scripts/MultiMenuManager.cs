using UnityEngine;

public class MultiMenuManager : MonoBehaviour 
{
    /// <summary>
    /// 加载游戏大厅场景
    /// </summary>
    public void LoadLobbyScene()
    {
        AllSceneManager.LoadScene(GameScene.LobbyScene);
    }

    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void BackToMainMenuScene()
    {
        AllSceneManager.LoadScene(GameScene.MainMenuScene);
    }

}
