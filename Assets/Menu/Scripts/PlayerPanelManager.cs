using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelManager : Photon.MonoBehaviour
{
    public Text playerNameText;                             // 玩家名称文本
    public Image masterIcon;                                // 房主标签
    public GameObject tankRenderers;                        // 坦克渲染模型
    public Text colorButtonText;                            // 颜色按钮文本
    public Color enableColor = Color.white;                 // 可用时背景颜色
    public Color disableColor = Color.gray;                 // 不可用时背景颜色
    public Image backgroundImage;                           // 背景颜色
    public AllRoommatesPanelManager allRoomatesPanel;       // 所有其他玩家面板管理

    private string playerName;                              // 玩家名
    private Color currentColor = Color.green;               // 当前颜色
    private bool isEnable = true;                           // 是否可用
    private bool isEmpty;                                   // 是否为空
    private Color temColor;                                 // 临时颜色变量

    public bool IsEmpty { get { return isEmpty; } }         // 是否为空面板

    /// <summary>
    /// 该是否面板可用
    /// </summary>
    public bool IsEnable
    {
        get { return isEnable; }
        set
        {
            isEnable = value;
            if (isEnable)
                backgroundImage.color = enableColor;
            else
            {
                Clear();
                backgroundImage.color = disableColor;
            }
        }
    }

    /// <summary>
    /// 清除信息
    /// </summary>
    public void Clear()
    {
        playerNameText.text = string.Empty;
        masterIcon.gameObject.SetActive(false);
        tankRenderers.gameObject.SetActive(false);
        isEmpty = true;
    }

    /// <summary>
    /// 配置名字，房主标记
    /// </summary>
    public void SetupInfo(string playerName, bool isMasterClient)
    {
        isEmpty = false;
        masterIcon.gameObject.SetActive(isMasterClient);        // 房主标记
        playerNameText.text = playerName;                       // 玩家名
        tankRenderers.gameObject.SetActive(true);
    }

    /// <summary>
    /// 设置颜色
    /// </summary>
    /// <param name="color">颜色</param>
    public void SetColor(Color color)
    {
        currentColor = color;
        ChangeColor.SelfAndChildrens(tankRenderers, currentColor);
    }

    /// <summary>
    /// 更换随机颜色，同时改变按钮普通状态颜色，并保持到PlayerPrefs
    /// </summary>
    public void SetRandomColorAndSave()
    {
        SetColor(new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)));
        if (colorButtonText != null)
            colorButtonText.color = currentColor;
        SavePlayerPrefs(currentColor);                  // 改完颜色保存起来
    }


    /// <summary>
    /// 更换房间主人时调用
    /// </summary>
    /// <param name="player">成为房主的人</param>
    public void OnMasterClientSwitched(PhotonPlayer player)
    {
        //if (player.IsLocal)
        //    masterIcon.gameObject.SetActive(true);
        //if (!IsEmpty)
        //masterIcon.gameObject.SetActive(photonView.owner.IsMasterClient);
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
    /// 同步信息，仅主面板带Photon view自动回调
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
            Debug.Log("Receive");
            temColor.r = (float)stream.ReceiveNext();
            temColor.g = (float)stream.ReceiveNext();
            temColor.b = (float)stream.ReceiveNext();
            allRoomatesPanel.AddOrUpdatePlayer(info.sender,temColor);
        }
    }

}
