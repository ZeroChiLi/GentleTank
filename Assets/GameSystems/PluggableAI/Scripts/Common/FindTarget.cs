using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 查找目标
    /// </summary>
    public class FindTarget
    {
        /// <summary>
        /// 查找敌人（使用Physics.Raycast），存在返回其transform，不存在返回null
        /// </summary>
        /// <param name="hit">射线捕获</param>
        /// <param name="source">检测源</param>
        /// <param name="player">检测者</param>
        /// <param name="controller">检测者</param>
        /// <param name="angle">检测角度（正前方为0，顺时针为正）</param>
        /// <param name="radius">检测半径</param>
        /// <param name="debugColor">调试颜色</param>
        /// <returns>返回目标的Transform</returns>
        static public Transform FindEnemy(out RaycastHit hit, Transform source, PlayerManager player, Quaternion angle, float radius, Color debugColor)
        {
            DebugDarwRay(source, angle, radius, debugColor);
            if (Physics.Raycast(source.position, angle * source.forward, out hit, radius) && HitEnemy(hit, player))
                return hit.transform;
            return null;
        }

        /// <summary>
        /// 调试绘制射线
        /// </summary>
        /// <param name="src">源</param>
        /// <param name="angle">角度</param>
        /// <param name="radius">半径</param>
        /// <param name="color">颜色</param>
        static public void DebugDarwRay(Transform src, Quaternion angle, float radius, Color color)
        {
            Debug.DrawRay(src.position, angle * src.forward * radius, color);
        }

        /// <summary>
        /// 判断射线击中目标是否是敌人
        /// </summary>
        /// <param name="hit">击中的物体</param>
        /// <param name="player">自己玩家</param>
        /// <returns>是否是敌人</returns>
        static public bool HitEnemy(RaycastHit hit, PlayerManager player)
        {
            if (hit.collider.CompareTag("Player") && !player.IsTeammate(hit.collider.GetComponent<PlayerManager>()))
                return true;
            return false;
        }

    }

}
