using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 查找目标
    /// </summary>
    public class FindTarget
    {
        /// <summary>
        /// 查找敌人，存在返回其transform，不存在返回null
        /// </summary>
        /// <param name="source">检测源</param>
        /// <param name="player">检测者</param>
        /// <param name="controller">检测者</param>
        /// <param name="anger">检测角度（正前方为0，顺时针为正）</param>
        /// <param name="radius">检测半径</param>
        /// <param name="debugColor">调试颜色</param>
        /// <returns>返回目标的Transform</returns>
        static public Transform FindEnemy(Transform source,PlayerManager player, Quaternion anger, float radius, Color debugColor)
        {
            Debug.DrawRay(source.position, anger * source.forward.normalized * radius, debugColor);

            RaycastHit hit;
            if (Physics.Raycast(source.position, anger * source.forward, out hit, radius, LayerMask.GetMask("Level")) && hit.collider.CompareTag("Player") && !player.IsTeammate(hit.collider.GetComponent<PlayerManager>()))
                return hit.transform;
            return null;
        }
    }

}
