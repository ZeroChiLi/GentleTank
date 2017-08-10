using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayerInfo : Photon.MonoBehaviour
{
    private PlayerPanelManager syncPlayerPanel;
    private AllRoommatesPanelManager allRoomatesPanel;
    private Color temColor;

    public void Init(PlayerPanelManager playerPanel)
    {
        syncPlayerPanel = playerPanel;
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
            if (syncPlayerPanel == null)
                return;
            stream.SendNext(syncPlayerPanel.CurrentColor.r);
            stream.SendNext(syncPlayerPanel.CurrentColor.g);
            stream.SendNext(syncPlayerPanel.CurrentColor.b);
        }
        else
        {
            temColor.r = (float)stream.ReceiveNext();
            temColor.g = (float)stream.ReceiveNext();
            temColor.b = (float)stream.ReceiveNext();
            RoomManager.Instance.allRoomatesPanel.AddOrUpdatePlayer(info.sender, temColor);
        }
    }

}
