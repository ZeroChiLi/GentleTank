using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Text infoText;                                   // 信息文本
    public Toast toast;                                     // 提示信息
    public GameObject createRoomWindow;                     // 创建房间窗口
    public InputField playerName;                           // 玩家名输入字段
    public float refreshTime = 3f;                          // 刷新时间间隔

    private bool connectFailed = false;                     // 是否连接失败
    private float elapsed;                                  // 下一次刷新剩余时间

    /// <summary>
    /// 连接客户端
    /// </summary>
    private void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;        // 定义所有客户端要转换场景时，需要同步主客户端。

        // 如果已经创建了客户端，但是还没有上线，那就连接上线呗。
        if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
            PhotonNetwork.ConnectUsingSettings("1.0");

        //PhotonNetwork.logLevel = PhotonLogLevel.Full;     // 连接信息
    }

    private void Start()
    {
        playerName.text = "玩家" + Random.Range(1, 9999);
    }

    private void Update()
    {
        if (!IsConnected())
            return;
        elapsed -= Time.deltaTime;
        if (elapsed < 0f)
        {
            elapsed = refreshTime;
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
            infoText.text = "Connecting to: " + PhotonNetwork.ServerAddress;
        else
            infoText.text = "Not connected. Connection state: " + PhotonNetwork.connectionStateDetailed + " Server: " + PhotonNetwork.ServerAddress;

        if (connectFailed)
            infoText.text = "Connected failed.";

        return false;
    }

    /// <summary>
    /// 刷新信息
    /// </summary>
    public void Refresh()
    {
        ShowServerInfo();
    }

    /// <summary>
    /// 显示服务器信息：在线玩家人数，房间总数
    /// </summary>
    public void ShowServerInfo()
    {
        StringBuilder str = new StringBuilder();
        str.Append("当前玩家总数： ");
        str.Append(PhotonNetwork.countOfPlayers);
        str.Append("  当前房间总数： ");
        str.Append(PhotonNetwork.countOfRooms);
        infoText.text = str.ToString();
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
        createRoomWindow.SetActive(show);
    }

    /// <summary>
    /// 返回上一个菜单（多人模式）
    /// </summary>
    public void BackToLastScene()
    {
        AllSceneManager.LoadScene(GameScene.MultiMenuScene);
    }
}
