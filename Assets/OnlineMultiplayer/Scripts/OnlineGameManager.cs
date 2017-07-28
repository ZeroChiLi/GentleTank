using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineGameManager : Photon.MonoBehaviour
{
    public GameObject playerPrefab;
    public PointList pointList;
    public CameraControl cameraControl;

    private OnlineTankManager playerInstance;
    private Point spawnPoint;

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
        playerInstance.RendererColor(new Color(PlayerPrefs.GetFloat("colorR"), PlayerPrefs.GetFloat("colorG"), PlayerPrefs.GetFloat("colorB")));
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
