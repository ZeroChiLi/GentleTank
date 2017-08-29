using UnityEngine;

public class AllKillMessageManager : MonoBehaviour 
{
    static private AllKillMessageManager instance;
    static public AllKillMessageManager Instance { get { return instance; } }

    public ObjectPool messagePool;

    private KillMessageManager message;

    private void Awake()
    {
        instance = this;
        messagePool.CreateObjectPool(gameObject);
    }

    /// <summary>
    /// 添加新的杀伤信息标签，每次添加重置位置到下方
    /// </summary>
    /// <param name="murderer">谋杀者</param>
    /// <param name="killed">被害者</param>
    public void AddKillMessage(PlayerManager murderer, PlayerManager killed)
    {
        message = messagePool.GetNextObject().GetComponent<KillMessageManager>();
        message.transform.SetAsLastSibling();
        message.Setup(murderer, killed);
    }

}
