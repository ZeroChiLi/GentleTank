using UnityEngine;
using UnityEngine.UI;

public class RoomInfoLabel : MonoBehaviour 
{
    public Text roommatesCount;         // 房间人数文本
    public Text roomName;               // 房间名称文本

    /// <summary>
    /// 设置房间人数文本
    /// </summary>
    /// <param name="current">当前人数</param>
    /// <param name="max">最大人数</param>
    public void SetRoommates(int current,int max)
    {
        roommatesCount.text = current + "/" + max;
    }

    /// <summary>
    /// 设置房间名称
    /// </summary>
    /// <param name="name">房间名称</param>
    public void SetRoomName(string name)
    {
        roomName.text = name;
    }


}
