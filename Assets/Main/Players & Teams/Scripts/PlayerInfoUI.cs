using UnityEngine;

public class PlayerInfoUI : MonoBehaviour
{
    public bool showPlayerInfo = true;              // 是否显示玩家信息
    public Font font;                               // 文本字体
    public int fontSize = 18;                       // 字体大小
    public float offset = 5;                        // 文本相对玩家偏移量
    public float vibrateRange = 2;                  // 文本抖动范围的平方（在范围内位置平滑移动，预防抖动）
    public float labelMoveSpeed = 0.1f;             // 标签移动速度

    private PlayerManager playerManage;               // 玩家信息
    private Camera targetCamera { get { return AllCameraRigManager.Instance.CurrentCamera; } }
    private GUIStyle style;                         // GUI风格  
    private Vector2 nameLabelSize;                  // 文本大小
    private Vector3 lastScreenPosition = Vector3.zero;  // 上一帧文本对应屏幕位置

    /// <summary>
    /// 获取目标镜头和玩家碰撞体
    /// </summary>
    private void Awake()
    {
        playerManage = GetComponent<PlayerManager>();
        SetupGUIStyle();
    }

    /// <summary>
    /// 初始化GUI风格
    /// </summary>
    private void SetupGUIStyle()
    {
        style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;                  // 文本锚点左下角
        style.fontSize = fontSize;                                  // 字体大小
        style.font = font;                                          // 文本字体
        style.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 1f);   // 字体默认颜色 
    }

    /// <summary>
    /// 配置名字还有颜色
    /// </summary>
    public void SetupNameAndColor()
    {
        nameLabelSize = style.CalcSize(new GUIContent(playerManage.PlayerName)); // 计算获取文本标签大小
        if(playerManage.Team != null)
            style.normal.textColor = playerManage.Team.TeamColor;
    }

    /// <summary>
    /// 根据距离计算并显示玩家信息
    /// </summary>
    private void OnGUI()
    {
        if (targetCamera == null || !showPlayerInfo)
            return;

        //绘制名字
        GUI.Label(CalculatePosition(), playerManage.PlayerName, style);
    }

    /// <summary>
    /// 计算文本位置，并返回位置对应Rect
    /// </summary>
    /// <returns>位置</returns>
    private Rect CalculatePosition()
    {
        // 计算获取文本对应屏幕位置
        Vector3 labelScreenPosition = targetCamera.WorldToScreenPoint(transform.position + offset * targetCamera.transform.up);
        labelScreenPosition.y = Screen.height - labelScreenPosition.y;    //翻转Y坐标值（screenPosition原点在左上角？？）

        Rect rect = new Rect(Vector2.zero, nameLabelSize);

        //和上一次位置距离在浮动范围，就平滑移动到上一次的位置
        if ((lastScreenPosition - labelScreenPosition).sqrMagnitude < vibrateRange)
        {
            lastScreenPosition = Vector3.MoveTowards(lastScreenPosition, labelScreenPosition, labelMoveSpeed);
            rect.center = lastScreenPosition;
        }
        else
        {
            rect.center = labelScreenPosition;
            lastScreenPosition = labelScreenPosition;
        }
        // 根据文本大小设置位置
        return rect;
    }

}
