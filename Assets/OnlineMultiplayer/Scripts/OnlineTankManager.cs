using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineTankManager : Photon.MonoBehaviour
{
    public TankInformation tankInfo;                // 坦克信息
    public TankMovement tankMovement;               // 坦克移动
    public OnlineTankShooting tankShooting;         // 坦克攻击
    public OnlineTankHealth tankHealth;             // 坦克血量

    public Color PlayerColor { get { return playerColor; } }

    private bool isMine;
    private string playerName;
    private Color playerColor;

    /// <summary>
    /// 初始化坦克信息
    /// </summary>
    public void InitTank(/*OnlineShellPool shellPool*/)
    {
        isMine = photonView.isMine;
        playerName = PhotonNetwork.playerName;
        playerColor = new Color(PlayerPrefs.GetFloat("colorR"), PlayerPrefs.GetFloat("colorG"), PlayerPrefs.GetFloat("colorB"));
        tankInfo.SetupTankInfo(-1, playerName, true, false, playerColor);
        ChangeColor.SelfAndChildrens(gameObject, playerColor);
        EnableControl(isMine);
        //tankShooting.shellPool = shellPool;
    }

    /// <summary>
    /// 判断是否正式开始来控制坦克控制权
    /// </summary>
    private void Update()
    {
        if (isMine && CountDown.IsStartGame())
            EnableControl(true);
        else
            EnableControl(false);
    }

    /// <summary>
    /// 设置坦克控制权
    /// </summary>
    /// <param name="enable">是否可控制</param>
    public void EnableControl(bool enable)
    {
        tankMovement.enabled = enable;
        tankShooting.enabled = enable;
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
