using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour 
{
    [Range(0,5)]
    public float zoom = 1;                                  // 缩放大小
    public GameObject minimapContent;                       // 小地图内容
    public GameObject playerIcon;                           // 代表玩家图标
    public GameObject cameraRig;                            // 相机设备

    private bool isSetup = false;                           // 是否已经配置好
    private Dictionary<Transform,GameObject> allPlayerIcon; // 所有玩家位置及对应图标
    private Transform target;                               // 追随目标
    private Quaternion contentRotate;                       // 小地图旋转角

    /// <summary>
    /// 更新目标，所有图标位置
    /// </summary>
    private void Update()
    {
        if (!isSetup)
            return;
        if (target == null)
            SetTargetRandomly();
        UpdateAllIconPosition();
    }

    /// <summary>
    /// 初始化玩家图标列表
    /// </summary>
    public void SetupPlayerIconDic()
    {
        allPlayerIcon = new Dictionary<Transform, GameObject>();
        for (int i = 0; i < AllTanksManager.Instance.Count; i++)
        {
            GameObject icon = Instantiate(playerIcon, minimapContent.transform);
            TeamManager team = AllTeamsManager.Instance.GetTeamByPlayerID(AllTanksManager.Instance[i].PlayerID);
            if (team != null)
                icon.GetComponent<Image>().color = team.TeamColor;
            allPlayerIcon[AllTanksManager.Instance[i].Instance.transform] = icon;
        } 
        // 旋转角，通过相机位置
        contentRotate = Quaternion.Euler(new Vector3(0,0,cameraRig.transform.rotation.eulerAngles.y));   
        isSetup = true;
    }


    /// <summary>
    /// 设置小地图中心目标
    /// </summary>
    /// <param name="target">中心目标</param>
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    /// <summary>
    /// 随机设置目标
    /// </summary>
    public void SetTargetRandomly()
    {
        for (int i = 0; i < AllTanksManager.Instance.Count; i++)
            if (AllTanksManager.Instance[i].Instance.activeSelf)
            {
                SetTarget(AllTanksManager.Instance[i].Instance.transform);
                return;
            }
        SetTarget(new GameObject().transform);
    }

    /// <summary>
    /// 更新所有玩家对应小地图位置
    /// </summary>
    public void UpdateAllIconPosition()
    {
        foreach (var item in allPlayerIcon)
        {
            // 同步玩家生死状态到图标
            item.Value.gameObject.SetActive(item.Key.gameObject.activeSelf);
            if (!item.Key.gameObject.activeSelf)
                continue;

            Vector3 realDistance = item.Key.transform.position - target.position;
            Vector3 showDistance = contentRotate * new Vector3(realDistance.x, realDistance.z, 0) * zoom;
            item.Value.transform.position = showDistance + allPlayerIcon[target].transform.position;
        }
    }

}
