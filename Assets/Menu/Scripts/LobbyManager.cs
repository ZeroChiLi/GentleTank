using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Text infoText;                                   // 信息文本

    private bool connectFailed = false;                     // 是否连接失败
    private string playerName;                              // 玩家名

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

    private void Update()
    {
        if (!IsConnected())
            return;

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
    /// 返回上一个菜单（多人模式）
    /// </summary>
    public void BackToLastScene()
    {
        AllSceneManager.LoadScene(GameScene.MultiMenuScene);
    }
}
