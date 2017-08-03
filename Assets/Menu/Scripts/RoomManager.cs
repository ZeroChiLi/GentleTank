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
    public PlayerPanelManager myPlayerPanel;                // 玩机自己的面板
    public AllRoommatesPanelManager allRoomatesPanel;       // 所有其他玩家面板管理

    private bool isReady;                                   // 是否可以开始游戏
    private PhotonView temPhotonView;                       // 同步视角组件

    /// <summary>
    /// 进入房间，如果失去连接回到大厅，没有就初始化
    /// </summary>
    private void Awake()
    {
        if (!PhotonNetwork.connected)                       // 若进入时没连接，直接回去大厅
            AllSceneManager.LoadScene(GameScene.LobbyScene);
    }

    /// <summary>
    /// 初始化房间
    /// </summary>
    private void Start()
    {
        roomTitle.text = PhotonNetwork.room.Name;
        myPlayerPanel.SetupInfo(PhotonNetwork.playerName, PhotonNetwork.isMasterClient);
        myPlayerPanel.SetRandomColorAndSave();
        allRoomatesPanel.Init(PhotonNetwork.room.MaxPlayers);
        Refresh();
        temPhotonView = myPlayerPanel.gameObject.AddComponent<PhotonView>();
        //temPhotonView.viewID = PhotonNetwork.AllocateViewID();
        temPhotonView.viewID =666;
        temPhotonView.synchronization = ViewSynchronization.UnreliableOnChange;
        temPhotonView.ObservedComponents = new List<Component>();
        temPhotonView.ObservedComponents.Add(myPlayerPanel);
    }

    /// <summary>
    /// 刷新信息
    /// </summary>
    public void Refresh()
    {
        roommatesCount.text = PhotonNetwork.room.PlayerCount + "/" + PhotonNetwork.room.MaxPlayers;

        if (PhotonNetwork.isMasterClient && PhotonNetwork.room.PlayerCount == PhotonNetwork.room.MaxPlayers && !isReady)
            StartBtnTrigger(true);
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

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        if (PhotonNetwork.isMasterClient)
            PhotonNetwork.LoadLevel("OnlineMultiplayerScene");

        PhotonNetwork.room.IsOpen = false;
    }

    /// <summary>
    /// 退出房间
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// 自己离开房间时调用
    /// </summary>
    public void OnLeftRoom()
    {
        Debug.Log("自己离开了房间");
        AllSceneManager.LoadScene(GameScene.LobbyScene);
    }

    /// <summary>
    /// 实力创建时调用
    /// </summary>
    /// <param name="info"></param>
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonInstantiate " + info.sender);    // you could use this info to store this or react
    }

    /// <summary>
    /// 失去连接时调用
    /// </summary>
    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("OnDisconnectedFromPhoton");

        windowPanel.OpenWindow("连接中断", "连接中断", "返回大厅", false, () => { AllSceneManager.LoadScene(GameScene.LobbyScene); });
        AllSceneManager.LoadScene(GameScene.LobbyScene);
    }

    /// <summary>
    /// 连接失败
    /// </summary>
    public void OnFailedToConnectToPhoton()
    {
        Debug.Log("OnFailedToConnectToPhoton");
        windowPanel.OpenWindow("连接失败", "连接失败", "返回大厅", false, () => { AllSceneManager.LoadScene(GameScene.LobbyScene); });
        AllSceneManager.LoadScene(GameScene.LobbyScene);
    }

    /// <summary>
    /// 新玩家进入时响应
    /// </summary>
    /// <param name="player">新玩家</param>
    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("新玩家加入: " + player.NickName);
        Refresh();
    }

    /// <summary>
    /// 玩家离开时响应
    /// </summary>
    /// <param name="player">离开的玩家</param>
    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("玩家离开: " + player);
        Refresh();
        allRoomatesPanel.RemovePlayer(player);
        StartBtnTrigger(false);
    }

}
