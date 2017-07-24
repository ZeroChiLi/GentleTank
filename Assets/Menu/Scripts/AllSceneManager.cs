using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏所有场景对应索引值
/// </summary>
public enum GameScene : byte
{
    MainMenuScene = 0,
    SoloScene = 1,
    MultiMenuScene = 2,
    LobbyScene = 3
}

/// <summary>
/// 所有游戏场景管理
/// </summary>
static public class AllSceneManager
{
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="scene">场景枚举值</param>
    static public void LoadScene(GameScene scene)
    {
        SceneManager.LoadScene((byte)scene);
    }
}
