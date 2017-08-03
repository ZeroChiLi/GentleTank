using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelManager : Photon.MonoBehaviour
{
    public Text playerNameText;                             // 玩家名称文本
    public Image masterIcon;                                // 房主标签
    public GameObject tankRenderers;                        // 坦克渲染模型
    public Text colorButtonText;                            // 颜色按钮文本

    private string playerName;                              // 玩家名
    private Color currentColor = Color.green;               // 当前颜色

    /// <summary>
    /// 清除信息
    /// </summary>
    public void Clear()
    {
        playerNameText.text = string.Empty;
        tankRenderers.gameObject.SetActive(false);
    }

    /// <summary>
    /// 配置名字，房主标记
    /// </summary>
    public void SetupInfo(string playerName,bool isMasterClient)
    {
        masterIcon.gameObject.SetActive(isMasterClient);   // 房主标记
        playerNameText.text = playerName;                    // 玩家名
        tankRenderers.gameObject.SetActive(true);
    }

    /// <summary>
    /// 更换随机颜色，同时改变按钮普通状态颜色
    /// </summary>
    public void SetRandomColor()
    {
        currentColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        ChangeColor.SelfAndChildrens(tankRenderers, currentColor);
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
            ChangeColor.SelfAndChildrens(tankRenderers, currentColor);
        }
    }

}
