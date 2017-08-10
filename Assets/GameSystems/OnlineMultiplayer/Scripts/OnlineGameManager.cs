using CameraRig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineGameManager : Photon.MonoBehaviour
{
    public GameObject playerPrefab;                         // 玩家预设
    public PointList pointList;                             // 玩家出生点列表
    public MultiplayerCameraManager cameraControl;          // 镜头控制
    //public OnlineShellPool onlineShellPool;               // 炮弹池

    private OnlineTankManager playerInstance;               // 玩家实例
    private Point spawnPoint;                               // 玩家出身点

    /// <summary>
    /// 初始化，创建实例
    /// </summary>
    private void Start()
    {
        if (!PhotonNetwork.connected)                           // 没连接，回大厅
            AllSceneManager.LoadScene(GameScene.LobbyScene);
        CreateInstance();
        cameraControl.targets = new List<Transform>();
        cameraControl.targets.Add(playerInstance.transform);
    }

    /// <summary>
    /// 创建坦克实例
    /// </summary>
    private void CreateInstance()
    {
        spawnPoint = pointList.GetRandomPoint();
        playerInstance = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.Rotation, 0).GetComponent<OnlineTankManager>();
        playerInstance.InitTank(/*onlineShellPool*/);
    }

    /// <summary>
    /// 返回大厅
    /// </summary>
    public void BackToLobby()
    {
        if (PhotonNetwork.inRoom)
            PhotonNetwork.LeaveRoom();
        AllSceneManager.LoadScene(GameScene.LobbyScene);
    }

}
