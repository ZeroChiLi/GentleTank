using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelManager : MonoBehaviour
{
    public Text playerNameText;                             // 玩家名称文本
    public Image masterIcon;                                // 房主标签
    public GameObject tankRenderers;                        // 坦克渲染模型
    public Text colorButtonText;                            // 颜色按钮文本
    public Color enableColor = Color.white;                 // 可用时背景颜色
    public Color disableColor = Color.gray;                 // 不可用时背景颜色
    public Image backgroundImage;                           // 背景颜色
    public AllRoommatesPanelManager allRoomatesPanel;       // 所有其他玩家面板管理

    public Color CurrentColor { get { return currentColor; } }

    private PhotonPlayer photonPlayer;                      // 面板对应玩家
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
        photonPlayer = null;
        playerNameText.text = string.Empty;
        masterIcon.gameObject.SetActive(false);
        tankRenderers.gameObject.SetActive(false);
        isEmpty = true;
    }

    /// <summary>
    /// 配置名字，房主标记
    /// </summary>
    public void SetupInfo(PhotonPlayer player, bool isMasterClient)
    {
        photonPlayer = player;
        isEmpty = false;
        masterIcon.gameObject.SetActive(isMasterClient);        // 房主标记
        playerNameText.text = player.NickName;                  // 玩家名
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
    /// 更换房间主人时调用
    /// </summary>
    /// <param name="player">成为房主的人</param>
    public void OnMasterClientSwitched(PhotonPlayer player)
    {
        if (photonPlayer != null)
            masterIcon.gameObject.SetActive(photonPlayer.IsMasterClient);
    }
}
