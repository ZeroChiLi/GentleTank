using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ChargeArea : MonoBehaviour 
{
    public Canvas areaCanvas;               // 区域画布
    public Slider slider;                   // 滑动条
    public float radius = 5f;               // 区域半径
    public float maxValue = 100f;           // 滑动条最大值
    public float chargeValue = 5f;          // 每秒变化量

    private Dictionary<GameObject,TankInformation> playerDic;    // 在区域内的所有玩家及其对应信息
    private float currentValue;             // 当前值
    private List<GameObject> majorPlayers;  // 占主力的玩家们
    private Dictionary<int,int> majorTeam;  // 区域内每个团队所占人数（团队ID：团队人数），没队的为1

    /// <summary>
    /// 设置滑动条大小、碰撞体大小
    /// </summary>
    private void Awake()
    {
        areaCanvas.GetComponent<RectTransform>().sizeDelta = Vector2.one * radius * 2;
        GetComponent<SphereCollider>().radius = radius;
        playerDic = new Dictionary<GameObject, TankInformation>();
        majorPlayers = new List<GameObject>();
        majorTeam = new Dictionary<int, int>();
    }

    /// <summary>
    /// 玩家进入，添加到列表
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        playerDic.Add(other.gameObject, other.GetComponent<TankInformation>());
    }

    /// <summary>
    /// 玩家出去，移除出列表
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.activeSelf)
            return;
        playerDic.Remove(other.gameObject);
    }

    private void Update()
    {
        if (playerDic.Count == 0)
            return;
        if (FindMajorPlayers())
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < majorPlayers.Count; i++)
            {
                str.Append(playerDic[majorPlayers[i]].playerName);
                str.Append("  ");
            }
            Debug.Log(str);
        }
    }

    /// <summary>
    /// 找到当前时间占多数的玩家们
    /// </summary>
    private bool FindMajorPlayers()
    {
        int maxCount = 0;
        int maxCountTeam = 0;
        int temTeamID = -1;
        majorPlayers.Clear();
        majorTeam.Clear();
        foreach (var item in playerDic)
        {
            temTeamID = item.Value.playerTeam.TeamID;
            // 如果没队玩家超过1个，直接跳过，因为没队玩家大于一个就不可能成为区域内占多数的玩家
            if (temTeamID == -1 && majorTeam[-1] != 0)      
                continue;
            if (!majorTeam.ContainsKey(temTeamID))
                majorTeam.Add(temTeamID, 0);
            majorTeam[temTeamID]++;
            if (majorTeam[temTeamID] > maxCount)            // 获取占多数的团队
            {
                maxCount = majorTeam[temTeamID];
                maxCountTeam = temTeamID;
            }
        }

        if (playerDic.Count == 1 && temTeamID == -1)        // 只有一个玩家在圈内，且是没有队伍的
            foreach (var item in playerDic)                 // 用遍历方法获取玩家（暂时没找到其他更好的方法获取这个唯一的Item）
            {
                majorPlayers.Add(item.Key);
                return true;
            }

        bool onlyOneTeamMax = false;
        foreach (var item in majorTeam)                     // 判断是否只有一个队伍达到最大值（没有两个队伍的人数相同）
            if (item.Value == maxCount)
            {
                if (onlyOneTeamMax == true)
                    return false;
                onlyOneTeamMax = true;
            }

        foreach (var item in playerDic)                     // 最后把占多数的玩家们加入队列中
            if (item.Value.playerTeam.TeamID == maxCountTeam)
                majorPlayers.Add(item.Key);

        return true;
    }
}



