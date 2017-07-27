using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelManager : MonoBehaviour
{
    public bool isMaster;                                   // 是否是房主
    public Color activePlayerColor = Color.black;           // 有效玩家颜色
    public Color inactivePlayerColor = Color.grey;          // 无效玩家颜色
    public Text playerNameText;                             // 玩家名称文本
    public Image masterIcon;                                // 房主标签

    public bool IsUsed { get { return isUsed; } }
    public PhotonPlayer Player { get { return photonPlayer; } }

    private PhotonPlayer photonPlayer;                      // 对应玩家
    private bool isUsed;                                    // 是否在使用中
    private int index;                                      // 索引值

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="index">索引值</param>
    public void Init(int index)
    {
        this.index = index;
        Clean();
    }

    /// <summary>
    /// 填充玩家信息
    /// </summary>
    /// <param name="player">玩家</param>
    public void Fill(PhotonPlayer player)
    {
        //PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
        isUsed = true;
        photonPlayer = player;
        playerNameText.text = player.NickName;
        playerNameText.color = activePlayerColor;
        Debug.Log("Fill " + index + "  Name: " + player.NickName);
    }

    /// <summary>
    /// 清空面板
    /// </summary>
    public void Clean()
    {
        isUsed = false;
        photonPlayer = null;
        playerNameText.text = "玩家 " + index;
        playerNameText.color = inactivePlayerColor;
    }

    /// <summary>
    /// 设置是否为房主
    /// </summary>
    /// <param name="isMaster"></param>
    public void SetMaster(bool isMaster)
    {
        masterIcon.gameObject.SetActive(isMaster);
    }

}
