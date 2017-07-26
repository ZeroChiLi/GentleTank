using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomLabelsManager : MonoBehaviour 
{
    public ObjectPool roomInfoPool;                                 // 房间标签池
    public GameObject roomGroup;                                    // 房间组对象
    public float roomLabelHeight = 64f;                             // 房间标签高度
    public Dictionary<RoomInfo, RoomInfoLabel> roomInfoLabelDic;    // 房间信息对应标签列表

    public RoomInfoLabel SelectedLabel { get { return selectedLabel; } }    // 获取选中的标签

    private RectTransform roomGroupRectTransform;                   // 房间组的矩形变换
    private RoomInfo[] currentRoomArray;                            // 当前房间信息数组
    private List<RoomInfo> roomInfoList;                            // 房间信息列表
    private List<RoomInfo> needCleanedRooms;                        // 需要被删除的信息列表
    private RoomInfoLabel roomInfoLabel;                            // 单个房间信息标签
    private RoomInfoLabel selectedLabel;                            // 被选中的标签

    /// <summary>
    /// 创建房间信息对象池
    /// </summary>
    private void Start()
    {
        roomGroupRectTransform = roomGroup.GetComponent<RectTransform>();
        roomInfoLabelDic = new Dictionary<RoomInfo, RoomInfoLabel>();
        needCleanedRooms = new List<RoomInfo>();
        roomInfoPool.CreateObjectPool(roomGroup);
    }

    /// <summary>
    /// 刷新房间列表
    /// </summary>
    /// <param name="roomInfoArray">房间组</param>
    public void Refresh(RoomInfo[] roomInfoArray)
    {
        CleanRooms(roomInfoArray);
        UpdateRoomInfoLabel(roomInfoArray);
        ResizeRoomGroup(roomInfoArray);
    }

    /// <summary>
    /// 清除无效房间
    /// </summary>
    /// <param name="roomInfoArray">房间组</param>
    public void CleanRooms(RoomInfo[] roomInfoArray)
    {
        roomInfoList = new List<RoomInfo>(roomInfoArray);
        needCleanedRooms.Clear();
        foreach (var item in roomInfoLabelDic)
            if (!roomInfoList.Contains(item.Key))
                needCleanedRooms.Add(item.Key);
        for (int i = 0; i < needCleanedRooms.Count; i++)
        {
            roomInfoLabelDic[needCleanedRooms[i]].gameObject.SetActive(false);
            roomInfoLabelDic.Remove(needCleanedRooms[i]);
        }
    }

    /// <summary>
    /// 更新所有房间信息标签
    /// </summary>
    /// <param name="roomInfoArray">房间组</param>
    public void UpdateRoomInfoLabel(RoomInfo[] roomInfoArray)
    {
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
            AddRoomLabelEvent(roomInfoLabel);
        }
    }

    /// <summary>
    /// 重置组高度
    /// </summary>
    /// <param name="roomInfoArray">房间组</param>
    public void ResizeRoomGroup(RoomInfo[] roomInfoArray)
    {
        roomGroupRectTransform.sizeDelta = new Vector2(0, roomLabelHeight * roomInfoArray.Length);
    }

    /// <summary>
    /// 为标签添加点击监听事件
    /// </summary>
    /// <param name="roomInfoLabel">房间信息标签</param>
    public void AddRoomLabelEvent(RoomInfoLabel roomInfoLabel)
    {
        Button roomLabelBtn = roomInfoLabel.GetComponent<Button>();
        roomLabelBtn.onClick.AddListener(() => { selectedLabel = roomInfoLabel; });
    }

    /// <summary>
    /// 清除当前选择的标签
    /// </summary>
    public void CleanSelectedLabel()
    {
        selectedLabel = null;
    }
}
