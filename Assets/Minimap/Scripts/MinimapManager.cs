using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour 
{
    public float zoom = 1;                                  // 缩放大小
    public GameObject minimapContent;                       // 小地图内容
    public GameObject playerIcon;                           // 代表玩家图标
    public AllTanksManager allTanksManager;                 // 所有坦克管理
    public AllTeamsManager allTeamsManager;                 // 所有团队管理

    private bool isSetup = false;                           // 是否已经配置好
    private Dictionary<Transform,GameObject> allPlayerIcon; // 所有玩家位置及对应图标
    private Transform target;                               // 追随目标

    private void Update()
    {
        if (!isSetup)
            return;
        if (target == null)
            SetTargetRandomly();
        UpdateCamera();
        UpdateAllIconPosition();
    }

    // 添加玩家图标列表
    public void SetupPlayerIconDic()
    {
        isSetup = true;
        allPlayerIcon = new Dictionary<Transform, GameObject>();
        for (int i = 0; i < allTanksManager.Length; i++)
        {
            GameObject icon = Instantiate(playerIcon, minimapContent.transform);
            icon.GetComponent<Image>().color = allTeamsManager.GetTeamByPlayerID(allTanksManager[i].playerID).TeamColor;
            allPlayerIcon[allTanksManager[i].Instance.transform] = icon;
        }
    }

    // 设置一个有效的坦克作为目标
    public void SetTargetRandomly()
    {
        for (int i = 0; i < allTanksManager.Length; i++)
            if (allTanksManager[i].Instance.activeSelf)
            {
                target = allTanksManager[i].Instance.transform;
                return;
            }
        target = new GameObject().transform;
    }

    // 设置目标
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    // 更新镜头位置
    public void UpdateCamera()
    {
        if (target == null)
            return;
        //transform.position = target.transform.position + CameraDistance;  
    }

    // 更新所有玩家对应小地图位置
    public void UpdateAllIconPosition()
    {
        foreach (var item in allPlayerIcon)
        {
            Vector3 realDistance = item.Key.transform.position - target.position;
            Vector3 showDistance = new Vector3(realDistance.x, realDistance.z, 0) * zoom;
            item.Value.transform.position = showDistance + allPlayerIcon[target].transform.position;
        }
    }





}
