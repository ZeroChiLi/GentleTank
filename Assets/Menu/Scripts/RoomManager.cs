using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : Photon.MonoBehaviour
{
    public GameObject playerPanelPerfab;                    // 玩家面板预设
    public Transform playerGroup;                           // 玩家面板组
    public Text roomTitle;                                  // 房间标题
    public Text roommatesCount;                             // 房间成员数
    public Button startButton;                              // 开始按钮
    public WindowPanel windowPanel;                         // 窗口按钮
    public Toast toast;                                     // 提示
    public float refreshRate = 1f;                          // 刷新信息时间

    private float elapsed;                                  // 计时器
    private int maxPlayers;                                 // 房间玩家总容量
    private List<PlayerPanelManager> playerPanelList;       // 玩家面板列表
    private bool isReady;                                   // 是否可以开始游戏
    private Dictionary<PhotonPlayer, PlayerPanelManager> playerDic;     // 玩家信息对应面板
    private List<PhotonPlayer> temPlayerList;                           // 临时玩家列表

    /// <summary>
    /// 进入房间，如果失去连接回到大厅，没有就初始化
    /// </summary>
    private void Awake()
    {
        if (!PhotonNetwork.connected)                       // 若进入时没连接，直接回去大厅
        {
            AllSceneManager.LoadScene(GameScene.LobbyScene);
            return;
        }
        playerDic = new Dictionary<PhotonPlayer, PlayerPanelManager>();
    }

    /// <summary>
    /// 初始化房间
    /// </summary>
    private void Start()
    {
        InitRoom();
    }

    /// <summary>
    /// 周期刷新信息
    /// </summary>
    private void Update()
    {
        elapsed -= Time.deltaTime;
        if (elapsed < 0f)
        {
            elapsed = refreshRate;
            Refresh(PhotonNetwork.playerList);
        }
    }

    /// <summary>
    /// 初始化房间信息
    /// </summary>
    public void InitRoom()
    {
        roomTitle.text = PhotonNetwork.room.Name;
        maxPlayers = PhotonNetwork.room.MaxPlayers;
        roommatesCount.text = "1/" + maxPlayers;
        playerPanelList = new List<PlayerPanelManager>();
        for (int i = 0; i < maxPlayers; i++)
        {
            playerPanelList.Add(Instantiate(playerPanelPerfab, playerGroup).GetComponent<PlayerPanelManager>());
            playerPanelList[i].Init(i + 1);
        }
        RefeshPlayerDic(PhotonNetwork.playerList);
    }

    /// <summary>
    /// 清除无效玩家
    /// </summary>
    /// <param name="photonPlayers">服务器发来的玩家列表</param>
    public void CleanInvalidPlayerPanels(PhotonPlayer[] photonPlayers)
    {
        temPlayerList = new List<PhotonPlayer>(photonPlayers);
        for (int i = 0; i < playerPanelList.Count; i++)
        {
            // 如果面板没人再用，或者有人但这人在列表里面，跳过。
            if (!playerPanelList[i].IsUsed || temPlayerList.Contains(playerPanelList[i].Player))
                continue;

            Debug.Log("Cleaned " + playerPanelList[i].Player.NickName);
            playerDic.Remove(playerPanelList[i].Player);
            playerPanelList[i].Clean();
            StartBtnTrigger(false);
        }
    }

    /// <summary>
    /// 刷新信息
    /// </summary>
    public void Refresh(PhotonPlayer[] photonPlayers)
    {
        roommatesCount.text = PhotonNetwork.room.PlayerCount + "/" + maxPlayers;

        if (PhotonNetwork.isMasterClient && PhotonNetwork.room.PlayerCount == maxPlayers && !isReady)
            StartBtnTrigger(true);
    }

    /// <summary>
    /// 刷新玩家字典，玩家进入房间时
    /// </summary>
    public void RefeshPlayerDic(PhotonPlayer[] photonPlayers)
    {
        for (int i = 0; i < photonPlayers.Length; i++)
            if (!playerDic.ContainsKey(photonPlayers[i]))
            {
                playerDic[photonPlayers[i]] = GetEmptyPlayerPanel();
                playerDic[photonPlayers[i]].Fill(photonPlayers[i]);
            }
    }

    /// <summary>
    /// 获取空的玩家面板
    /// </summary>
    /// <returns>空玩家面板</returns>
    public PlayerPanelManager GetEmptyPlayerPanel()
    {
        for (int i = 0; i < playerPanelList.Count; i++)
            if (!playerPanelList[i].IsUsed)
                return playerPanelList[i];
        return null;
    }

    /// <summary>
    /// 开始按钮触发
    /// </summary>
    /// <param name="ready">是否准备好</param>
    public void StartBtnTrigger(bool ready)
    {
        roommatesCount.gameObject.SetActive(!ready);
        startButton.gameObject.SetActive(ready);
        isReady = ready;
    }

    public void OnMasterClientSwitched(PhotonPlayer player)
    {
        Debug.Log("OnMasterClientSwitched: " + player);

        string message;
        InRoomChat chatComponent = GetComponent<InRoomChat>();  // if we find a InRoomChat component, we print out a short message

        if (chatComponent != null)
        {
            // to check if this client is the new master...
            if (player.IsLocal)
                message = "You are Master Client now.";
            else
                message = player.NickName + " is Master Client now.";

            chatComponent.AddLine(message); // the Chat method is a RPC. as we don't want to send an RPC and neither create a PhotonMessageInfo, lets call AddLine()
        }
    }

    /// <summary>
    /// 退出房间
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        //if (PhotonNetwork.player != null && playerDic.ContainsKey(PhotonNetwork.player))
        //{
        //}
    }

    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom (local)");

        AllSceneManager.LoadScene(GameScene.LobbyScene);
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("OnDisconnectedFromPhoton");

        windowPanel.OpenWindow("连接中断", "连接中断", "返回大厅", false, () => { AllSceneManager.LoadScene(GameScene.LobbyScene); });
        AllSceneManager.LoadScene(GameScene.LobbyScene);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonInstantiate " + info.sender);    // you could use this info to store this or react
    }

    /// <summary>
    /// 新玩家进入是更新玩家字典
    /// </summary>
    /// <param name="player">新玩家</param>
    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        RefeshPlayerDic(PhotonNetwork.playerList);
    }

    /// <summary>
    /// 玩家离开时清除掉
    /// </summary>
    /// <param name="player"></param>
    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        CleanInvalidPlayerPanels(PhotonNetwork.playerList);
    }

    public void OnFailedToConnectToPhoton()
    {
        Debug.Log("OnFailedToConnectToPhoton");

        // back to main menu
        AllSceneManager.LoadScene(GameScene.LobbyScene);
    }

}
