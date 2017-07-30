using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelManager : Photon.MonoBehaviour
{
    public bool isMaster;                                   // 是否是房主
    public Text playerNameText;                             // 玩家名称文本
    public Image masterIcon;                                // 房主标签
    public GameObject tankModelUI;                          // 坦克模型UI
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

        masterIcon.gameObject.SetActive(photonView.owner.IsMasterClient);   // 房主标记
        playerNameText.text = photonView.owner.NickName;                    // 玩家名
        controlPanel.SetActive(photonView.isMine);                          // 控制面板

        if (!photonView.isMine)
            return;
        SetRandomColor();                                                   // 随机更改颜色
    }

    /// <summary>
    /// 更换随机颜色，同时改变按钮普通状态颜色
    /// </summary>
    public void SetRandomColor()
    {
        currentColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        ChangeColor.SelfAndChildrens(tankModelUI, currentColor);
        colorButtonText.color = currentColor;
        SavePlayerPrefs(currentColor);                  // 改完颜色保存起来
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
    /// 保持玩家信息
    /// </summary>
    /// <param name="color">玩家颜色</param>
    public void SavePlayerPrefs(Color color)
    {
        PlayerPrefs.SetFloat("colorR", color.r);
        PlayerPrefs.SetFloat("colorG", color.g);
        PlayerPrefs.SetFloat("colorB", color.b);
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
            stream.SendNext(currentColor.r);
            stream.SendNext(currentColor.g);
            stream.SendNext(currentColor.b);
        }
        else
        {
            currentColor.r = (float)stream.ReceiveNext();
            currentColor.g = (float)stream.ReceiveNext();
            currentColor.b = (float)stream.ReceiveNext();
            ChangeColor.SelfAndChildrens(tankModelUI, currentColor);
        }
    }

}
