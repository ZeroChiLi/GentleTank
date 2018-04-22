using UnityEngine;

public class GameInformation : MonoBehaviour
{
    static public GameInformation Instance { get; private set; }

    public AllPlayerManager players;

    private void Awake()
    {
        Instance = this;
        players.SetupInstance();
        DontDestroyOnLoad(this);
    }

}