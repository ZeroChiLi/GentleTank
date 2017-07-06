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

    private List<TankInformation> playerInfoList;   // 在区域内的所有玩家及其对应信息
    private List<TankInformation> inactivePlayers;  // 失活的坦克
    private float currentValue;                     // 当前值
    private float updateTime = 0.1f;                // 更新时间
    private float elapsedTime = 0f;                 // 计时器
    private bool occupied = false;                  // 旗帜区域是否已经被占有
    private bool stalemate = false;                 // 是否保持僵局（区域内不止一个队伍）
    private List<TankInformation> occupyPlayerList; // 占有旗帜队伍的所有圈内成员

    /// <summary>
    /// 设置滑动条大小、碰撞体大小
    /// </summary>
    private void Awake()
    {
        GetComponent<SphereCollider>().radius = radius;
        areaCanvas.GetComponent<RectTransform>().sizeDelta = Vector2.one * radius * 2;
        playerInfoList = new List<TankInformation>();
        inactivePlayers = new List<TankInformation>();
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

        for (int i = 0; i < playerInfoList.Count; i++)              //先获取所有失效玩家
            if (!playerInfoList[i].gameObject.activeSelf)
                inactivePlayers.Add(playerInfoList[i]);

        for (int i = 0; i < inactivePlayers.Count; i++)             //再删除所有失效玩家
            playerInfoList.Remove(inactivePlayers[i]);
    }

    /// <summary>
    /// 更新当前数值
    /// </summary>
    private void UpdateCurrentValue()
    {

    }

    /// <summary>
    /// 更新占有权值，通过rank来控制更新倍数
    /// </summary>
    /// <param name="rank">更新倍数</param>
    private void UpdateOccupation(float rank)
    {

    }

    /// <summary>
    /// 是否只有一个队伍（或个人）在区域内
    /// </summary>
    /// <returns>是否只有一个队伍（或个人）在区域内</returns>
    private bool OnlyOneTeamHere()
    {

        return true;
    }

    /// <summary>
    /// 保持僵局，按人数分配扇区颜色
    /// </summary>
    private void KeepStalemate()
    {

    }

    /// <summary>
    /// 占有属于玩家
    /// </summary>
    /// <param name="player">玩家信息</param>
    /// <returns>是否被该玩家占有</returns>
    private bool OccupiedByPlayer(TankInformation player)
    {
        return true;
    }
}



