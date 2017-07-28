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
    private bool isReady;                                   // 是否可以开始游戏

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
        PhotonNetwork.Instantiate(playerPanelPerfab.name, transform.position, Quaternion.identity, 0);
    }

    /// <summary>
    /// 初始化房间，房主
    /// </summary>
    private void Start()
    {
        InitRoom();
    }

    /// <summary>
    /// 初始化房间信息
    /// </summary>
    public void InitRoom()
    {
        roomTitle.text = PhotonNetwork.room.Name;
        maxPlayers = PhotonNetwork.room.MaxPlayers;
        roommatesCount.text = "1/" + maxPlayers;
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
    /// 刷新信息
    /// </summary>
    public void Refresh(PhotonPlayer[] photonPlayers)
    {
        roommatesCount.text = PhotonNetwork.room.PlayerCount + "/" + maxPlayers;

        if (PhotonNetwork.isMasterClient && PhotonNetwork.room.PlayerCount == maxPlayers && !isReady)
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
        Debug.Log("OnPhotonPlayerConnected: " + player.NickName);
    }

    /// <summary>
    /// 玩家离开时响应
    /// </summary>
    /// <param name="player">离开的玩家</param>
    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("OnPlayerDisconneced: " + player);
        StartBtnTrigger(false);
    }

}
