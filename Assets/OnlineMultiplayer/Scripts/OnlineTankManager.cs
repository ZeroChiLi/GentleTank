using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineTankManager : Photon.MonoBehaviour
{
    public TankInformation tankInfo;            // 坦克信息
    public TankMovement tankMovement;           // 坦克移动
    public TankShooting tankShooting;           // 坦克攻击

    public Color PlayerColor { get { return playerColor; } }

    private bool isMine;
    private string playerName;
    private Color playerColor;

    /// <summary>
    /// 初始化坦克信息
    /// </summary>
    public void InitTank()
    {
        isMine = photonView.isMine;
        playerName = PhotonNetwork.playerName;
        playerColor = new Color(PlayerPrefs.GetFloat("colorR"), PlayerPrefs.GetFloat("colorG"), PlayerPrefs.GetFloat("colorB"));
        tankInfo.SetupTankInfo(-1, playerName, true, false, playerColor);
        ChangeColor.SelfAndChildrens(gameObject, playerColor);
        tankMovement.enabled = isMine;
        tankShooting.enabled = isMine;
    }

    /// <summary>
    /// 同步客户端信息
    /// </summary>
    /// <param name="stream">信息流</param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(playerColor.r);
            stream.SendNext(playerColor.g);
            stream.SendNext(playerColor.b);
        }
        else
        {
            playerColor.r = (float)stream.ReceiveNext();
            playerColor.g = (float)stream.ReceiveNext();
            playerColor.b = (float)stream.ReceiveNext();
            ChangeColor.SelfAndChildrens(gameObject, playerColor);
        }
    }
}
