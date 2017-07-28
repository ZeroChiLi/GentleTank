using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelManager : Photon.MonoBehaviour
{
    public bool isMaster;                                   // 是否是房主
    public Text playerNameText;                             // 玩家名称文本
    public Image masterIcon;                                // 房主标签
    public TankModelUI tankModelUI;                         // 坦克模型UI
    public GameObject controlPanel;                         // 控制面板
    public Button colorButton;                              // 坦克颜色更换按钮
    public Text colorButtonText;                            // 颜色按钮文本

    private string playerName;                              // 玩家名
    private RectTransform rectransform;                     // 变换
    private Color currentColor = Color.green;               // 当前颜色

    /// <summary>
    /// 添加到父组件，初始化名字
    /// </summary>
    private void Awake()
    {
        rectransform = GetComponent<RectTransform>();
        rectransform.SetParent(GameObject.FindGameObjectWithTag("PlayerPanelGroup").transform);
        rectransform.localPosition = Vector3.zero;
        rectransform.localScale = Vector3.one;
        masterIcon.gameObject.SetActive(photonView.owner.IsMasterClient);
        playerNameText.text = photonView.owner.NickName;

        controlPanel.SetActive(photonView.isMine);
        if (!photonView.isMine)
            return;
    }



    /// <summary>
    /// 更换随机颜色，同时改变按钮普通状态颜色
    /// </summary>
    public void SetRandomColor()
    {
        currentColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        tankModelUI.ChangeColor(currentColor);
        colorButtonText.color = currentColor;
    }

    /// <summary>
    /// 更换房间主人时调用
    /// </summary>
    /// <param name="player">成为房主的人</param>
    public void OnMasterClientSwitched(PhotonPlayer player)
    {
        masterIcon.gameObject.SetActive(photonView.owner.IsMasterClient);
    }

    /// <summary>
    /// 同步信息
    /// </summary>
    /// <param name="stream">信息流</param>
    /// <param name="info">信息介绍</param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Debug.Log("Send");
            stream.SendNext(currentColor.r);
            stream.SendNext(currentColor.g);
            stream.SendNext(currentColor.b);
        }
        else
        {
            Debug.Log("Get");
            tankModelUI.ChangeColor(new Color((float)stream.ReceiveNext(), (float)stream.ReceiveNext(), (float)stream.ReceiveNext()));
        }
    }
}
