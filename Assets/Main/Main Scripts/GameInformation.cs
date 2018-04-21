using UnityEngine;

[System.Serializable]
public struct PlayerInfo
{
    public int id;
    public bool isAI;
    public int selectedIndex;
    public Color color;

    public PlayerInfo(int id, bool isAI,int selectedIndex,Color color)
    {
        this.id = id;
        this.isAI = isAI;
        this.selectedIndex = selectedIndex;
        this.color = color;
    }
};

public class GameInformation : MonoBehaviour
{
    // 四位玩家的信息
    public PlayerInfo[] players = new PlayerInfo[4]{
        new PlayerInfo ( 0, false, 0, Color.blue ),
        new PlayerInfo ( 1, false, 1, Color.green),
        new PlayerInfo( 2, false, 2, Color.red),
        new PlayerInfo( 3, false, 3, Color.yellow),
    };

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

}