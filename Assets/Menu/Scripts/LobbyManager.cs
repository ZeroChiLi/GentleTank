using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Text infoText;                                   // 信息文本
    public InputField playerName;                           // 玩家名输入字段
    public float refreshRate = 3f;                          // 刷新时间间隔
    public RoomLabelsManager roomLabelsManager;             // 所有房间标签管理
    public Toast toast;                                     // 提示信息
    public GameObject createRoomWindow;                     // 创建房间窗口
    public ButtonController createRoomButton;               // 创建房间按钮
    public ButtonController joinRoomButton;                 // 加入房间按钮
    public ButtonController joinRandomRoomButton;           // 随机加入房间按钮

    public Color fpsGoodColor;                              // 延迟低颜色
    public Color fpsGeneralColor;                           // 延迟一般颜色
    public Color fpsBadColor;                               // 延迟严重颜色

    private bool connectFailed = false;                     // 是否连接失败
    private float elapsed;                                  // 下一次刷新剩余时间
    private float currentPing;                              // 当前Ping值

    /// <summary>
    /// 连接客户端
    /// </summary>
    private void Awake()
    {
        LockButtons(!PhotonNetwork.connected);
        PhotonNetwork.automaticallySyncScene = true;        // 定义所有客户端要转换场景时，需要同步主客户端。
        ConnectToServer();
    }

    /// <summary>
    /// 初始化玩家名字，若已经有，不修改
    /// </summary>
    private void Start()
    {
        playerName.text = PhotonNetwork.playerName;
        if (string.IsNullOrEmpty(playerName.text))
            playerName.text = "玩家" + Random.Range(1, 9999);
    }

    /// <summary>
    /// 刷新
    /// </summary>
    private void Update()
    {
        if (!IsConnected())
            return;
        elapsed -= Time.deltaTime;
        if (elapsed < 0f)
        {
            elapsed = refreshRate;
            Refresh();
        }
    }

    /// <summary>
    /// 是否连接成功，显示相关信息
    /// </summary>
    /// <returns>连接成功true</returns>
    private bool IsConnected()
    {
        if (PhotonNetwork.connected)
            return true;

        if (PhotonNetwork.connecting)
            infoText.text = "正在连接： " + PhotonNetwork.ServerAddress;
        else
            infoText.text = "未连接，尝试手动刷新。 " + PhotonNetwork.connectionStateDetailed + " 服务器： " + PhotonNetwork.ServerAddress;

        if (connectFailed)
            infoText.text = "连接失败，尝试手动刷新。";

        return false;
    }

    /// <summary>
    /// 刷新信息
    /// </summary>
    public void Refresh()
    {
        ConnectToServer();
        roomLabelsManager.Refresh(PhotonNetwork.GetRoomList());
        ShowServerInfo();
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    private void ConnectToServer()
    {
        if (PhotonNetwork.connected || PhotonNetwork.connecting)
            return;
        // 如果已经创建了客户端，但是还没有上线，那就连接上线呗。
        if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
            PhotonNetwork.ConnectUsingSettings("1.0");
    }

    /// <summary>
    /// 锁定一些按钮（创建、加入、随即加入）
    /// </summary>
    /// <param name="enable">是否锁定</param>
    private void LockButtons(bool enable)
    {
        createRoomButton.Lock(enable);
        joinRoomButton.Lock(enable);
        joinRandomRoomButton.Lock(enable);
    }

    /// <summary>
    /// 设置玩家名称
    /// </summary>
    public void SetPlayerName()
    {
        PhotonNetwork.playerName = playerName.text;
        PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
    }


    /// <summary>
    /// 显示服务器信息：在线玩家人数，房间总数
    /// </summary>
    public void ShowServerInfo()
    {
        StringBuilder str = new StringBuilder();
        str.Append("当前玩家总数： ");
        str.Append(PhotonNetwork.countOfPlayers);
        str.Append("    当前房间总数： ");
        str.Append(PhotonNetwork.countOfRooms);
        str.Append("    延迟： ");
        currentPing = PhotonNetwork.GetPing();
        str.Append("<color=#");
        str.Append(ColorUtility.ToHtmlStringRGB(GetPingColor(currentPing)));
        str.Append(">");
        str.Append(currentPing);
        str.Append("ms</color>");
        infoText.text = str.ToString();
    }

    /// <summary>
    /// 获取延迟时间对应颜色
    /// </summary>
    /// <param name="ping">ping值</param>
    /// <returns>返回延迟值对应颜色</returns>
    public Color GetPingColor(float ping)
    {
        if (ping < 20f)
            return fpsGoodColor;
        else if (ping < 40f)
            return fpsGeneralColor;
        else
            return fpsBadColor;
    }
    
    /// <summary>
    /// 显示或关闭创建房间窗口
    /// </summary>
    /// <param name="show">是否显示</param>
    public void ShowCreateRoomWindow(bool show)
    {
        if (string.IsNullOrEmpty(playerName.text))
        {
            toast.ShowToast(3f, "请输入玩家名称");
            return;
        }
        SetPlayerName();
        createRoomWindow.SetActive(show);
    }

    /// <summary>
    /// 加入房间
    /// </summary>
    public void JoinRoom()
    {
        if (roomLabelsManager.SelectedLabel == null)
        {
            toast.ShowToast(3f, "未选中任何房间。");
            return;
        }
        SetPlayerName();
        PhotonNetwork.JoinRoom(roomLabelsManager.SelectedLabel.RoomName);
        LockButtons(true);
    }

    /// <summary>
    /// 随机加入房间
    /// </summary>
    public void JoinRoomRandomly()
    {
        SetPlayerName();
        PhotonNetwork.JoinRandomRoom();
        LockButtons(true);
    }

    /// <summary>
    /// 返回上一个菜单（多人模式）
    /// </summary>
    public void BackToLastScene()
    {
        AllSceneManager.LoadScene(GameScene.MultiMenuScene);
    }

    /// <summary>
    /// 加入房间时调用
    /// </summary>
    public void OnJoinedRoom()
    {
        toast.ShowToast(3f, "成功加入房间。");
        LockButtons(false);
    }

    /// <summary>
    /// 加入房间失败
    /// </summary>
    /// <param name="cause">失败信息</param>
    public void OnPhotonJoinRoomFailed(object[] cause)
    {
        LockButtons(false);
        switch (int.Parse(cause[0].ToString()))
        {
            case ErrorCode.GameFull:
                toast.ShowToast(3f, "加入失败，房间已满。");
                break;
            case ErrorCode.GameDoesNotExist:
                toast.ShowToast(3f, "加入失败，房间不存在。");
                break;
            case ErrorCode.GameClosed:
                toast.ShowToast(3f, "加入失败，房间已关闭。");
                break;
            default:
                toast.ShowToast(3f, "加入失败。" + cause[1]);
                break;
        }
    }

    /// <summary>
    /// 随机加入失败
    /// </summary>
    public void OnPhotonRandomJoinFailed()
    {
        LockButtons(false);
        toast.ShowToast(3f, "随机加入失败。");
    }

    /// <summary>
    /// 连接中断
    /// </summary>
    public void OnDisconnectedFromPhoton()
    {
        LockButtons(true);
        toast.ShowToast(3f, "连接中断。");
    }

    /// <summary>
    /// 连接失败
    /// </summary>
    /// <param name="parameters">失败信息</param>
    public void OnFailedToConnectToPhoton(object parameters)
    {
        LockButtons(true);
        connectFailed = true;
        toast.ShowToast(3f, "连接失败。 StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.ServerAddress);
    }

    /// <summary>
    /// 加入到大厅
    /// </summary>
    public void OnConnectedToMaster()
    {
        toast.ShowToast(3f, "连接服务器成功，加入大厅。");
        LockButtons(false);
        PhotonNetwork.JoinLobby();
    }
}
