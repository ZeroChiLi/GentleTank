using UnityEngine;

/// <summary>
/// 在游戏开始时失效
/// </summary>
public class InactiveWhileGameStart : MonoBehaviour 
{
    private void Update()
    {
        if (GameRound.Instance.CurrentGameState == GameState.Start)
            gameObject.SetActive(false);
    }
}
