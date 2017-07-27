using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelManager : Photon.MonoBehaviour
{
    public bool isMaster;                                   // 是否是房主
    public Text playerNameText;                             // 玩家名称文本
    public Image masterIcon;                                // 房主标签
    public Camera tankCamera;                               // 对着坦克的镜头

    private string playerName;                              // 玩家名

    /// <summary>
    /// 添加到父组件，初始化名字
    /// </summary>
    private void Awake()
    {
        transform.parent = GameObject.FindGameObjectWithTag("PlayerPanelGroup").transform;
        masterIcon.gameObject.SetActive(photonView.owner.IsMasterClient);
        playerNameText.text = photonView.owner.NickName;

        if (!photonView.isMine)
            return;
        tankCamera.gameObject.SetActive(true);
    }

    /// <summary>
    /// 更换房间主人时调用
    /// </summary>
    /// <param name="player"></param>
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
        //if (stream.isWriting)
        //{
        //}
        //else
        //{
        //}
    }
}
