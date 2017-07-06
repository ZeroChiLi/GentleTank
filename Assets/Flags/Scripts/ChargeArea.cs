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

    private List<TankInformation> playerInfoList;    // 在区域内的所有玩家及其对应信息
    private float currentValue;             // 当前值
    private List<GameObject> majorPlayers;  // 占主力的玩家们
    private Dictionary<int,int> majorTeam;  // 区域内每个团队所占人数（团队ID：团队人数），没队的为1
    private float updateTime = 0.1f;        // 更新时间
    private float elapsedTime = 0f;         // 计时器
    private List<TankInformation> inactivePlayers;  // 失活的坦克

    /// <summary>
    /// 设置滑动条大小、碰撞体大小
    /// </summary>
    private void Awake()
    {
        areaCanvas.GetComponent<RectTransform>().sizeDelta = Vector2.one * radius * 2;
        GetComponent<SphereCollider>().radius = radius;
        playerInfoList = new List<TankInformation>();
        majorPlayers = new List<GameObject>();
        majorTeam = new Dictionary<int, int>();
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
    }

    private void UpdateMembers()
    {
        
    }

    /// <summary>
    /// 移除所有在圈内且已经死掉的玩家
    /// </summary>
    private void CleanInactivePlayer()
    {
        inactivePlayers.Clear();

        for (int i = 0; i < playerInfoList.Count; i++)
            if (!playerInfoList[i].gameObject.activeSelf)
                inactivePlayers.Add(playerInfoList[i]);

        for (int i = 0; i < inactivePlayers.Count; i++)
            playerInfoList.Remove(inactivePlayers[i]);
    }

    /// <summary>
    /// 区域内是否包含任意玩家
    /// </summary>
    /// <returns>是否包含任意玩家</returns>
    private bool ContainAnyPlayer()
    {
        return playerInfoList.Count != 0;
    }

}



