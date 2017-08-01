using UnityEngine;
using UnityEngine.UI;

public class RoomInfoLabel : MonoBehaviour 
{
    public Text roommatesCount;                     // 房间人数文本
    public Text roomName;                           // 房间名称文本
    public Color normalColor = Color.black;         // 房间字体正常颜色
    public Color unopenedColor = Color.gray;        // 房间未开启颜色

    public string RoomName { get { return roomName.text; } }        // 获取房间名

    private Color currentColor;
    private string playingText = "正在游戏";

    /// <summary>
    /// 更新房间信息
    /// </summary>
    public void UpdateRoomInfo(RoomInfo roomInfo)
    {
        if (roomInfo == null)
            return;

        currentColor = roomInfo.IsOpen ? normalColor : unopenedColor;

        roommatesCount.text = roomInfo.IsOpen ? (roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers) : playingText;
        roommatesCount.color = currentColor;

        roomName.text = roomInfo.Name;
        roomName.color = currentColor;
    }

    /// <summary>
    /// 清除信息
    /// </summary>
    public void CleanRoomInfo(RoomInfo roomInfo)
    {
        if (roomInfo == null)
            return;

        roommatesCount.text = "0/0";
        roomName.text = "null";
        roomInfo = null;
    }
}
