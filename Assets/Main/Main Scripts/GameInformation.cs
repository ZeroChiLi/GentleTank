using UnityEngine;

[System.Serializable]
public struct PlayerInfo
{
    public int id;
    public bool isJoin;
    public bool isAI;
    public int selectedIndex;
    public Color color;

    public PlayerInfo(int id, bool isJoin, bool isAI, int selectedIndex, Color color)
    {
        this.id = id;
        this.isJoin = isJoin;
        this.isAI = isAI;
        this.selectedIndex = selectedIndex;
        this.color = color;
    }
};

public class GameInformation : MonoBehaviour
{
    static public GameInformation Instance { get; private set; }

    // 四位玩家的信息
    public PlayerInfo[] players = new PlayerInfo[4]{
        new PlayerInfo ( 0, false, false, 0, Color.blue ),
        new PlayerInfo ( 1, false, false, 1, Color.green),
        new PlayerInfo( 2, false, false, 2, Color.red),
        new PlayerInfo( 3, false, false, 3, Color.yellow),
    };

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

}