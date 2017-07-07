using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public enum OccupyState
{
    Empty, Partly, Full
}

public class ChargeArea : MonoBehaviour
{
    public Canvas areaCanvas;               // 区域画布
    public Slider slider;                   // 滑动条
    public Image fillImage;                 // 滑动条填充图片
    public float radius = 5f;               // 区域半径
    public float maxValue = 100f;           // 滑动条最大值
    public float chargeValue = 5f;          // 每秒变化量

    private OccupyState occupyState = OccupyState.Empty;    // 占有状态
    private List<TankInformation> playerInfoList;           // 在区域内的所有玩家及其对应信息
    private List<TankInformation> inactivePlayers;          // 失活的坦克
    private float currentValue = 0f;                        // 当前值
    private float updateTime = 0.5f;                        // 更新时间
    private float elapsedTime = 0f;                         // 计时器
    private bool stalemate = false;                         // 是否保持僵局（区域内不止一个队伍）
    private TankInformation occupyPlayer;                   // 范围内占有玩家代表
    private Dictionary<TeamManager, int> occupyTeamDic;     // 旗帜范围内所有团队，及其对应团队权重
    private List<TankInformation> occupyIndependentPlayer;  // 旗帜范围内所有个人，权重为1
    private bool updateOccupyRate = false;                  // 是否已经更新占有扇区
    private Color occupyColor = Color.white;                // 占有颜色
    private bool updateOccupyColor = false;                 // 是否已经更新占有颜色

    private float TotalWeight { get { return playerInfoList.Count; } }     // 总权重

    /// <summary>
    /// 设置滑动条大小、碰撞体大小
    /// </summary>
    private void Awake()
    {
        GetComponent<SphereCollider>().radius = radius;
        areaCanvas.GetComponent<RectTransform>().sizeDelta = Vector2.one * radius * 2;
        playerInfoList = new List<TankInformation>();
        inactivePlayers = new List<TankInformation>();
        occupyTeamDic = new Dictionary<TeamManager, int>();
        occupyIndependentPlayer = new List<TankInformation>();
        slider.maxValue = maxValue;
    }

    /// <summary>
    /// 玩家进入，添加到列表
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        playerInfoList.Add(other.GetComponent<TankInformation>());
        UpdateAreaWeight();
    }

    /// <summary>
    /// 玩家出去，移除出列表
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") || !other.gameObject.activeSelf)
            return;
        playerInfoList.Remove(other.GetComponent<TankInformation>());
        UpdateAreaWeight();
    }

    private void Update()
    {
        if (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            return;
        }
        elapsedTime = updateTime;
        if (!ContainAnyPlayer())
            return;
        CleanInactivePlayer();
        UpdateCurrentValue();
    }

    /// <summary>
    /// 区域内是否包含任意玩家
    /// </summary>
    /// <returns>是否包含任意玩家</returns>
    private bool ContainAnyPlayer()
    {
        return playerInfoList.Count != 0;
    }

    /// <summary>
    /// 移除所有在圈内且已经死掉的玩家
    /// </summary>
    private void CleanInactivePlayer()
    {
        inactivePlayers.Clear();

        for (int i = 0; i < playerInfoList.Count; i++)              // 先获取所有失效玩家
            if (!playerInfoList[i].gameObject.activeSelf)
                inactivePlayers.Add(playerInfoList[i]);

        for (int i = 0; i < inactivePlayers.Count; i++)             // 再删除所有失效玩家
            playerInfoList.Remove(inactivePlayers[i]);

        if (inactivePlayers.Count != 0)                             // 如果修改（删除）了玩家列表，更新权重
            UpdateAreaWeight();
    }

    /// <summary>
    /// 更新当前数值
    /// </summary>
    private void UpdateCurrentValue()
    {
        if (!OnlyOneTeamHere())                                 // 不止一支队伍，保持僵持状态
        {
            //occupyState = OccupyState.Empty;
            KeepStalemate();
            return;
        }

        UpdateOccupiedColor();                                  // 更新占有颜色

        switch (occupyState)
        {
            case OccupyState.Empty:
                {
                    occupyPlayer = playerInfoList[0];
                    fillImage.color = occupyColor;                      // 没被占有，进行占有并修改为自己团队的颜色
                    UpdateOccupation(true);
                    break;
                }
            case OccupyState.Partly:
                {
                    if (OccupiedByPlayer(playerInfoList[0]))
                        UpdateOccupation(true);
                    else
                        UpdateOccupation(false);
                    break;
                }
            case OccupyState.Full:
                {
                    if (!OccupiedByPlayer(playerInfoList[0]))
                        UpdateOccupation(false);
                    break;
                }
        }

    }

    /// <summary>
    /// 更新圈内所有团队的权重
    /// </summary>
    private void UpdateAreaWeight()
    {
        occupyTeamDic.Clear();
        occupyIndependentPlayer.Clear();
        for (int i = 0; i < playerInfoList.Count; ++i)
        {
            if (playerInfoList[i].playerTeamID == -1)                           // 独立玩家（没有队伍），直接加到独立队列
                occupyIndependentPlayer.Add(playerInfoList[i]);
            else
            {
                if (!occupyTeamDic.ContainsKey(playerInfoList[i].playerTeam))   // 团队，重复就加1权重
                    occupyTeamDic.Add(playerInfoList[i].playerTeam, 1);
                else
                    ++occupyTeamDic[playerInfoList[i].playerTeam];
            }
        }
        updateOccupyRate = true;        // 顺便更新僵局颜色
        updateOccupyColor = true;       // 更新进行占有颜色
    }

    /// <summary>
    /// 是否只有一个队伍（或个人）在区域内
    /// </summary>
    /// <returns>是否只有一个队伍（或个人）在区域内</returns>
    private bool OnlyOneTeamHere()
    {
        int teamID = playerInfoList[0].playerTeamID;                // 先获取第一个玩家团队ID
        for (int i = 1; i < playerInfoList.Count; i++)
        {
            // 1.只要其他玩家存在没有队的，就不止一支队伍
            // 2.只要和第一个玩家团队id不同就不止一支队伍。
            if (playerInfoList[i].playerTeamID == -1 || teamID != playerInfoList[i].playerTeamID)
                return false;
        }
        return true;
    }

    /// <summary>
    /// 保持僵局，按人数分配扇区颜色
    /// </summary>
    private void KeepStalemate()
    {
        if (!updateOccupyRate)
            return;
        StringBuilder debugStr = new StringBuilder();
        debugStr.Append("Independent Player : ");
        for (int i = 0; i < occupyIndependentPlayer.Count; i++)
            debugStr.AppendFormat("{0}  w:{1} , ", occupyIndependentPlayer[i].playerName, 1f / TotalWeight);
        debugStr.Append("  Team : ");
        foreach (var item in occupyTeamDic)
            debugStr.AppendFormat("{0}  w:{1} , ", item.Key.TeamName, item.Value / TotalWeight);
        Debug.Log(debugStr);
        updateOccupyRate = false;
    }

    /// <summary>
    /// 更新占有权值，通过isAdd来控制增加还是减小
    /// </summary>
    /// <param name="isAdd">是否增加</param>
    private void UpdateOccupation(bool isAdd)
    {
        if (isAdd && slider.value >= slider.maxValue)
            occupyState = OccupyState.Full;
        else if (!isAdd && slider.value <= slider.minValue)
            occupyState = OccupyState.Empty;
        else
        {
            occupyState = OccupyState.Partly;
            slider.value += (isAdd ? 1 : -1) * TotalWeight * chargeValue * updateTime;
        }
    }

    /// <summary>
    /// 占有属于玩家
    /// </summary>
    /// <param name="player">玩家信息</param>
    /// <returns>是否被该玩家占有</returns>
    private bool OccupiedByPlayer(TankInformation player)
    {
        if (occupyPlayer == null || occupyPlayer == player)     // 不存在上一次最后离开玩家或者上一次玩家等于传入玩家
            return true;
        if (occupyPlayer.playerTeamID == -1)                  // 上一次玩家没有队，那肯定不是传入占有的
            return false;
        if (occupyPlayer.playerTeam == player.playerTeam)     // 上一次玩家团队等于这次传入团队
            return true;
        return false;
    }

    /// <summary>
    /// 获取占有颜色
    /// </summary>
    /// <returns>返回占有颜色</returns>
    private void UpdateOccupiedColor()
    {
        if (!updateOccupyColor)
            return;
        if (playerInfoList.Count == 0)
            occupyColor = Color.white;
        if (playerInfoList[0].playerTeamID == -1)
            occupyColor = playerInfoList[0].playerColor;
        else
            occupyColor = playerInfoList[0].playerTeam.TeamColor;
        updateOccupyColor = false;
    }

}



