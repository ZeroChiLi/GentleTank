using UnityEngine;

namespace Item.Tank
{
    public class TankInformation : MonoBehaviour
    {
        [HideInInspector]
        public int playerID;                            // 玩家ID
        [HideInInspector]
        public string playerName;                       // 玩家名
        [HideInInspector]
        public bool playerActive;                       // 玩家是否激活
        [HideInInspector]
        public bool playerAI;                           // 玩家是否是AI
        [HideInInspector]
        public Color playerColor;                       // 玩家的颜色
        [HideInInspector]
        public TeamManager playerTeam;                  // 玩家所在团队
        [HideInInspector]
        public int playerTeamID = -1;                   // 玩家所在团队ID（没队的-1）
        [HideInInspector]
        public string playerColoredName;                // 带颜色的玩家名

        public void SetupTankInfo(int id, string name, bool active, bool isAI, Color color, TeamManager team = null, string coloredName = null)
        {
            playerID = id;
            playerName = name;
            playerActive = active;
            playerAI = isAI;
            playerColor = color;
            playerTeam = team;
            playerColoredName = coloredName == null ? name : coloredName;
            if (playerTeam != null)
                playerTeamID = playerTeam.TeamID;
        }
    }
}
