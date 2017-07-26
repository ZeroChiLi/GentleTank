using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : Photon.MonoBehaviour
{
    public Text roomTitle;
    public Text roommatesCount;
    public Button startButton;
    public Button backButton;
    public WindowPanel windowPanel;
    public Toast toast;

    private void Awake()
    {
        if (!PhotonNetwork.connected)                       // 若进入时没连接，直接回去大厅
        {
            AllSceneManager.LoadScene(GameScene.LobbyScene);
            return;
        }
    }

    public void OnMasterClientSwitched(PhotonPlayer player)
    {
        Debug.Log("OnMasterClientSwitched: " + player);

        string message;
        InRoomChat chatComponent = GetComponent<InRoomChat>();  // if we find a InRoomChat component, we print out a short message

        if (chatComponent != null)
        {
            // to check if this client is the new master...
            if (player.IsLocal)
            {
                message = "You are Master Client now.";
            }
            else
            {
                message = player.NickName + " is Master Client now.";
            }


            chatComponent.AddLine(message); // the Chat method is a RPC. as we don't want to send an RPC and neither create a PhotonMessageInfo, lets call AddLine()
        }
    }

    /// <summary>
    /// 退出房间
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom (local)");

        AllSceneManager.LoadScene(GameScene.LobbyScene);
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("OnDisconnectedFromPhoton");

        windowPanel.OpenWindow("连接中断", "连接中断", "返回大厅", false, () => { AllSceneManager.LoadScene(GameScene.LobbyScene); });
        AllSceneManager.LoadScene(GameScene.LobbyScene);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonInstantiate " + info.sender);    // you could use this info to store this or react
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerConnected: " + player);
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("OnPlayerDisconneced: " + player);
    }

    public void OnFailedToConnectToPhoton()
    {
        Debug.Log("OnFailedToConnectToPhoton");

        // back to main menu
        AllSceneManager.LoadScene(GameScene.LobbyScene);
    }

    /// <summary>
    /// 连接成功，房间加入到大厅
    /// </summary>
    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster." + PhotonNetwork.room.Name + " Join Lobby");
        PhotonNetwork.JoinLobby();
    }
}
