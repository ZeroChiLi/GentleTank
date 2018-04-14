using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 所有游戏场景管理
/// </summary>
static public class AllSceneManager
{
    public enum GameSceneType : byte
    {
        MainMenuScene = 0,
        SoloScene = 1,
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="scene">场景枚举值</param>
    static public void LoadScene(GameSceneType scene)
    {
        SceneManager.LoadScene((byte)scene);
    }
}
