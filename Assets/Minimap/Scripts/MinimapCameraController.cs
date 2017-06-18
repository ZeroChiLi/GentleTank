using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour 
{
    public Vector3 distance = new Vector3(0, 50, 0);        // 镜头距离
    public GameObject playerIcon;                           // 代表玩家图标

    private List<GameObject> allPlayer;                     // 所有目标
    private GameObject target;                              // 追随目标

    private void Awake()
    {
        allPlayer = new List<GameObject>();
    }

    private void Update()
    {
        UpdateCamera();
    }

    // 设置目标
    public void SetTarget(GameObject target,Color color)
    {
        this.target = target;
    }

    // 更新镜头位置
    public void UpdateCamera()
    {
        if (target == null)
            return;
        transform.position = target.transform.position + distance;  
    }

    // 添加目标列表
    public void SetPlayerList(List<GameObject> playerList)
    {
        allPlayer = playerList;
    }


}
