using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllRoomsManager : MonoBehaviour 
{
    public ObjectPool roomInfoPool;                                 // 房间标签池
    public GameObject roomGroup;                                    // 房间组对象
    public Dictionary<RoomInfo, RoomInfoLabel> roomInfoLabelDic;    // 房间信息对应标签列表

    private RoomInfo[] currentRoomArray;                            // 当前房间信息数组
    private List<RoomInfo> roomInfoList;                            // 房间信息列表
    private RoomInfoLabel roomInfoLabel;                            // 单个房间信息标签

    /// <summary>
    /// 创建房间信息对象池
    /// </summary>
    private void Start()
    {
        roomInfoLabelDic = new Dictionary<RoomInfo, RoomInfoLabel>();
        roomInfoPool.CreateObjectPool(roomGroup);
    }

    /// <summary>
    /// 刷新房间列表
    /// </summary>
    public void Refresh(RoomInfo[] roomInfoArray)
    {
        CleanRooms(roomInfoArray);
        for (int i = 0; i < roomInfoArray.Length; i++)
        {
            if (!roomInfoLabelDic.ContainsKey(roomInfoArray[i]))
            {
                Debug.Log("New Room : " + roomInfoArray[i].Name);
                roomInfoLabelDic.Add(roomInfoArray[i], roomInfoPool.GetNextObject().GetComponent<RoomInfoLabel>());
            }

            roomInfoLabel = roomInfoLabelDic[roomInfoArray[i]];
            roomInfoLabel.SetRoommates(roomInfoArray[i].PlayerCount, roomInfoArray[i].MaxPlayers);
            roomInfoLabel.SetRoomName(roomInfoArray[i].Name);
        }
    }

    /// <summary>
    /// 清除无效房间
    /// </summary>
    public void CleanRooms(RoomInfo[] roomInfoArray)
    {
        roomInfoList = new List<RoomInfo>(roomInfoArray);
        foreach (var item in roomInfoLabelDic)
            if (!roomInfoList.Contains(item.Key))
            {
                item.Value.gameObject.SetActive(false);
                roomInfoLabelDic.Remove(item.Key);
            }
    }
}
