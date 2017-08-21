using UnityEngine;
using UnityEngine.UI;

public class PlayerNameLabelManager : MonoBehaviour 
{
    public PlayerManager player;                    // 绑定玩家
    public RectTransform labelRect;                 // 标签位置信息
    public Image arrowIcon;                         // 箭头图标
    public Text text;                               // 标签文本
    public Color playerColor = Color.black;         // 玩家颜色
    public Color InactiveColor = Color.gray;        // 失效颜色
    public Vector3 offset = new Vector3(0,96,0);    // 偏移量

    private bool lastActive = true;                 // 玩家上一帧激活状态（状态变化时只改变一次颜色）

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="player">玩家</param>
    public void Init(PlayerManager player)
    {
        this.player = player;
        text.text = player.PlayerName;
        if (player.Team != null)
            playerColor = player.Team.TeamColor;
        ActiveColor(false);
    }

    /// <summary>
    /// 激活颜色
    /// </summary>
    /// <param name="active">是否激活</param>
    public void ActiveColor(bool active)
    {
        lastActive = active;
        if (active)
            SetColor(playerColor);
        else
            SetColor(InactiveColor);
    }

    /// <summary>
    /// 设置文本和箭头颜色
    /// </summary>
    /// <param name="color">颜色值</param>
    public void SetColor(Color color)
    {
        text.color = color;
        arrowIcon.color = color;
    }

    /// <summary>
    /// 在每一帧最后更新，标签位置
    /// </summary>
    private void LateUpdate()
    {
        if (player == null || AllCameraRigManager.Instance == null)
            return;

        labelRect.position = AllCameraRigManager.Instance.CurrentCamera.WorldToScreenPoint(player.transform.position);
        labelRect.position += offset;

        if (player.gameObject.activeInHierarchy != lastActive)  // 只有变化时才改变颜色
            ActiveColor(!lastActive);
    }
}
