using UnityEngine;
using UnityEngine.UI;

public class RoomInfoLabel : MonoBehaviour 
{
    public Text roommatesCount;         // 房间人数文本
    public Text roomName;               // 房间名称文本
    public Text roomDelay;              // 房间延迟文本
    public Color fpsGoodColor;          // 延迟低颜色
    public Color fpsGeneralColor;       // 延迟一般颜色
    public Color fpsBadColor;           // 延迟严重颜色

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

    /// <summary>
    /// 设置房间延迟时间
    /// </summary>
    /// <param name="delay">反接延迟时间</param>
    public void SetRoomDelay(float delay)
    {
        roomDelay.text = delay + "ms";
        if (delay < 20f)
            roomDelay.color = fpsGoodColor;
        else if (delay < 40f)
            roomDelay.color = fpsGeneralColor;
        else
            roomDelay.color = fpsBadColor;
    }

}
