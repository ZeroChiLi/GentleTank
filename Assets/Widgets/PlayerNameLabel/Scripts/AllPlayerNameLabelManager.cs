using UnityEngine;

public class AllPlayerNameLabelManager : MonoBehaviour 
{
    public GameObject labelPerfab;

    /// <summary>
    /// 创建所有玩家标签
    /// </summary>
    /// <param name="allPlayer">所有玩家</param>
    public void CreateAllPlayerLabels(AllPlayerManager allPlayer)
    {
        for (int i = 0; i < allPlayer.Count; i++)
            Create(allPlayer[i]);
    }

    /// <summary>
    /// 创建一个玩家标签
    /// </summary>
    /// <param name="player">玩家</param>
    public void Create(PlayerManager player)
    {
        Instantiate(labelPerfab, transform).GetComponent<PlayerNameLabelManager>().Init(player);
    }
}
