using UnityEngine;

/// <summary>
/// 满足特定游戏回合状态时失活对象
/// </summary>
public class InactiveWithGameState : MonoBehaviour 
{
    public GameState gameState = GameState.Start;     

    private void Update()
    {
        if (GameRound.Instance != null && GameRound.Instance.CurrentGameState == gameState)
            gameObject.SetActive(false);
    }
}
