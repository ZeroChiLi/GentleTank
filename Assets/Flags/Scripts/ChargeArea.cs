using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

public enum OccupyState
{
    Empty,              // 完全没被占有
    Partly,             // 部分被占用
    Full                // 完全被占有
}

public class ChargeArea : MonoBehaviour
{
    public Canvas areaCanvas;               // 区域画布
    public Slider slider;                   // 滑动条
    public Image fillImage;                 // 滑动条填充图片
    public FlagManager flagManager;         // 旗帜管理
    [Range(0f, 1f)]
    public float fillAlpha = 0.5f;          // 填充透明度
    public float radius = 5f;               // 区域半径
    public float maxValue = 100f;           // 滑动条最大值
    public float chargeValue = 5f;          // 每秒变化量
    public ObjectPool fillPool;             // 填充扇区池（用于僵持状态显示）

    private OccupyState occupyState = OccupyState.Empty;    // 占有状态
    private List<TankInformation> playerInfoList;           // 在区域内的所有玩家及其对应信息
    private List<TankInformation> inactivePlayers;          // 失活的坦克
    private float updateTime = 0.5f;                        // 更新时间
    private float elapsedTime = 0f;                         // 计时器
    private TankInformation occupyPlayer;                   // 占领区域玩家代表信息。（用于判断区域上一次占有的占有信息）
    private Dictionary<TeamManager, int> occupyTeamDic;     // 旗帜范围内所有团队，及其对应团队权重
    private List<TankInformation> occupyIndependentPlayer;  // 旗帜范围内所有个人，权重为1
    private bool updateOccupyRate = false;                  // 是否已经更新占有扇区
    private int effectRotateDiection = 1;                   // 特效旋转方向（1为顺时针，-1逆时针）
    private List<Image> fillList;                           // 填充扇区列表
    private bool triggerState;                              // 僵持和变化之间是否变化
    private float nextColorTime;                            // 僵持特性：下一次变化颜色的时间
    private int currentIndex;                               // 僵持特性：当前颜色索引
    private EffectController effectController;              // 粒子特性控制器

    private float TotalWeight { get { return playerInfoList.Count; } }                  // 总权重
    private TankInformation RepresentativePlayer { get { return playerInfoList[0]; } }  //圈内玩家代表（只有一支队伍情况下）

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
        fillList = new List<Image>();
        effectController = GetComponent<EffectController>();
        effectController.SetParticleShapeRaidus(radius);
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

    /// <summary>
    /// 根据updateTime时间进行更新
    /// </summary>
    private void Update()
    {
        ChargeAreaEffect();                 //旋转特效
        if (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            return;
        }
        elapsedTime = updateTime;
        if (!ContainAnyPlayer())            // 不包含任何玩家，直接返回
        {
            if (triggerState == true)       // 但需要更新扇区的话，更新扇区（僵持状态到没有任何玩家状态）
                triggerState = FillTrigger(false);
            effectController.CloseEffect();
            return;
        }
        if (!CleanInactivePlayer())         // 清除所有失效玩家,如果都清除完就不执行更新
            UpdateChargeArea();             // 更新占领区
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
    /// 移除所有在圈内且已经死掉的玩家,如果有清除后不存在任意玩家返回true
    /// </summary>
    /// <returns>如果有清除后不存在任意玩家返回true</returns>
    private bool CleanInactivePlayer()
    {
        inactivePlayers.Clear();

        for (int i = 0; i < playerInfoList.Count; i++)              // 先获取所有失效玩家
            if (!playerInfoList[i].gameObject.activeSelf)
                inactivePlayers.Add(playerInfoList[i]);

        for (int i = 0; i < inactivePlayers.Count; i++)             // 再删除所有失效玩家
            playerInfoList.Remove(inactivePlayers[i]);

        if (inactivePlayers.Count != 0)                             // 如果修改（删除）了玩家列表，更新权重
            UpdateAreaWeight();

        if (playerInfoList.Count == 0)                              // 一个都不剩返回true
            return true;
        return false;
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
        SetStalemateFillActive(false);  // 先清除所有僵持状态
        fillList.Clear();
    }

    /// <summary>
    /// 更新占领区域
    /// </summary>
    private void UpdateChargeArea()
    {
        if (OnlyOneTeamHere())                      // 只有一支队伍，更新扇区            
        {
            effectRotateDiection = 1;
            if (triggerState == true)
                triggerState = FillTrigger(false);
            UpdateOccupy();
        }
        else                                        // 不止一支队伍，保持僵持状态
        {
            effectRotateDiection = -1;
            if (triggerState == false)
                triggerState = FillTrigger(true);
            KeepStalemate();
            ShowStalemateEffect(updateTime,2f);
            effectController.SetEffect(EffectState.Chaos,transform,Color.white);
            effectController.SetParticleColor(GetPlayerColor(playerInfoList[Random.Range(0, playerInfoList.Count)]));
        }
    }


    /// <summary>
    /// 扇区填充选择（是僵持状态的填充，还是变化状态的填充）
    /// </summary>
    /// <param name="isStalemate">是否是僵持状态</param>
    private bool FillTrigger(bool isStalemate)
    {
        SetStalemateFillActive(isStalemate);
        fillImage.gameObject.SetActive(!isStalemate);
        return isStalemate;
    }

    /// <summary>
    /// 设置僵持状态的扇区激活状态
    /// </summary>
    /// <param name="active"></param>
    private void SetStalemateFillActive(bool active)
    {
        for (int i = 0; i < fillList.Count; i++)
            fillList[i].gameObject.SetActive(active);
    }

    /// <summary>
    /// 区域旋转的特效
    /// </summary>
    private void ChargeAreaEffect()
    {
        slider.transform.Rotate(0, 0, effectRotateDiection * 90f * Time.deltaTime);
    }

    #region 只有一支队伍，更新占领区域面积

    /// <summary>
    /// 更新占领区
    /// </summary>
    private void UpdateOccupy()
    {
        switch (occupyState)
        {
            case OccupyState.Empty:
                UpdateOccupyEmpty();
                break;
            case OccupyState.Partly:
                UpdateOccupyPartly();
                break;
            case OccupyState.Full:
                UpdateOccupyFull();
                break;
        }
    }

    /// <summary>
    /// 占有区完全空白时
    /// </summary>
    private void UpdateOccupyEmpty()
    {
        occupyPlayer = RepresentativePlayer;
        fillImage.color = GetPlayerColor(RepresentativePlayer);     // 没被占有，进行占有并修改为自己团队的颜色
        UpdateOccupationValue(true);
        effectController.SetEffect(EffectState.Crack, transform,fillImage.color);
    }

    /// <summary>
    /// 占有区部分被占有时
    /// </summary>
    private void UpdateOccupyPartly()
    {
        if (OccupiedByPlayer(RepresentativePlayer))     // 占有玩家是否是本队的，增长
        {
            UpdateOccupationValue(true);
            effectController.SetEffect(EffectState.Absorb, transform, fillImage.color);
        }
        else
        {
            UpdateOccupationValue(false);               // 非本队的，减小
            effectController.SetEffect(EffectState.Release, transform, fillImage.color);
        }
    }

    /// <summary>
    /// 占有区完全占被有时
    /// </summary>
    private void UpdateOccupyFull()
    {
        if (OccupiedByPlayer(RepresentativePlayer))     // 是本队就保持，非本队就减小
            effectController.SetEffect(EffectState.Completed, transform, fillImage.color);
        else
        {
            UpdateOccupationValue(false);
            effectController.SetEffect(EffectState.Release, transform, fillImage.color);
        }
    }

    /// <summary>
    /// 是否只有一个队伍（或个人）在区域内
    /// </summary>
    /// <returns>是否只有一个队伍（或个人）在区域内</returns>
    private bool OnlyOneTeamHere()
    {
        if (playerInfoList.Count == 0)
            return true;
        int teamID = RepresentativePlayer.playerTeamID;                // 先获取第一个玩家团队ID
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
    /// 更新占有权值，通过isAdd来控制增加还是减小
    /// </summary>
    /// <param name="isAdd">是否增加</param>
    private void UpdateOccupationValue(bool isAdd)
    {
        if (isAdd && slider.value >= slider.maxValue)           // 增长时到爆表
            occupyState = OccupyState.Full;
        else if (!isAdd && slider.value <= slider.minValue)     // 减小时到最低点
            occupyState = OccupyState.Empty;
        else
        {
            occupyState = OccupyState.Partly;                   // 修改当前扇区值
            slider.value += (isAdd ? 1 : -1) * TotalWeight * chargeValue * updateTime;
            flagManager.SetFlagColor(Color.Lerp(Color.white, fillImage.color, slider.value / slider.maxValue));
        }
    }

    /// <summary>
    /// 判断占有是否与玩家同队
    /// </summary>
    /// <param name="player">玩家信息</param>
    /// <returns>是否被该玩家占有</returns>
    private bool OccupiedByPlayer(TankInformation player)
    {
        if (occupyPlayer == null || occupyPlayer == player)     // 不存在上一次最后离开玩家或者上一次玩家等于传入玩家
            return true;
        if (occupyPlayer.playerTeamID == -1)                    // 上一次玩家没有队，那肯定不是传入占有的
            return false;
        if (occupyPlayer.playerTeam == player.playerTeam)       // 上一次玩家团队等于这次传入团队
            return true;
        return false;
    }

    /// <summary>
    /// 获取占有颜色，有队伍的话返回队伍颜色，否则返回自己的颜色
    /// </summary>
    /// <returns>返回占有颜色</returns>
    private Color GetPlayerColor(TankInformation player)
    {
        if (playerInfoList.Count == 0)
            return new Color(1, 1, 1, fillAlpha);

        // 没有队伍的颜色设置为个人颜色
        if (player.playerTeamID == -1)
            return new Color(player.playerColor.r, player.playerColor.g, player.playerColor.b, fillAlpha);

        // 有队伍的设置为队伍颜色
        return new Color(player.playerTeam.TeamColor.r, player.playerTeam.TeamColor.g, player.playerTeam.TeamColor.b, fillAlpha);
    }

    #endregion

    #region 多于一支队伍，保持僵局

    /// <summary>
    /// 保持僵局，按人数分配扇区颜色
    /// </summary>
    private void KeepStalemate()
    {
        if (!updateOccupyRate)                  // 改变权值的时候才计算
            return;
        float lastFillAmountAngle = 0f;         // 上一次填充后的的旋转角

        for (int i = 0; i < occupyIndependentPlayer.Count; i++)         // 填充个人
            FillWithWeight(ref lastFillAmountAngle, occupyIndependentPlayer[i].playerColor, 1.0f);
        foreach (var item in occupyTeamDic)                             // 填充团队
            FillWithWeight(ref lastFillAmountAngle, item.Key.TeamColor, item.Value);

        updateOccupyRate = false;
    }

    /// <summary>
    /// 通过权重填充扇区，填充完角度会变成填充后的下一个开始角度
    /// </summary>
    /// <param name="fillAmountAngle">填充开始的角度</param>
    /// <param name="fillColor">填充颜色</param>
    /// <param name="weight">填充权重</param>
    private void FillWithWeight(ref float fillAmountAngle, Color fillColor, float weight)
    {
        Image fillImage = fillPool.GetNextObject().GetComponent<Image>();
        fillList.Add(fillImage);
        fillImage.transform.SetParent(slider.transform);
        fillImage.rectTransform.localPosition = Vector3.zero;
        fillImage.rectTransform.sizeDelta = Vector2.zero;
        fillImage.rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, fillAmountAngle));
        fillImage.color = new Color(fillColor.r, fillColor.g, fillColor.b, fillAlpha);
        fillImage.fillAmount = weight / TotalWeight;
        fillAmountAngle += fillImage.fillAmount * 360;
    }

    /// <summary>
    /// 显示僵持状态下旗帜颜色变幻特效
    /// </summary>
    /// <param name="updateTime">每次调用时间间隔</param>
    /// <param name="period">颜色变化周期</param>
    private void ShowStalemateEffect(float updateTime, float period)
    {
        if (Time.time > nextColorTime)
        {
            nextColorTime = Time.time + period;
            currentIndex = currentIndex + 1;            //循环获取玩家列表
        }
        currentIndex %= playerInfoList.Count;           //避免越界，每次使用前先预防一下
        flagManager.SetFlagColor(Color.Lerp(GetPlayerColor(playerInfoList[currentIndex]), GetPlayerColor(playerInfoList[(currentIndex + 1) % playerInfoList.Count]), (nextColorTime - Time.time) / period));
    }

    #endregion

    /// <summary>
    /// 重置充电区域
    /// </summary>
    public void ResetChargeArea()
    {
        slider.value = 0;
        occupyState = OccupyState.Empty;
        playerInfoList.Clear();
        occupyIndependentPlayer.Clear();
        occupyTeamDic.Clear();
        SetStalemateFillActive(false);
        occupyPlayer = null;
        updateOccupyRate = false;
        flagManager.SetFlagColor(Color.white);
        effectController.CloseEffect();
    }
}



