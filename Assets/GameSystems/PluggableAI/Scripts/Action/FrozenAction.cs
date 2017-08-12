using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 判断一段时间后，是否还在原来位置的半径范围内
    /// </summary>
    /// 
    [CreateAssetMenu(menuName = "PluggableAI/Actions/Frozen")]
    public class FrozenAction : Action
    {
        [Range(0.1f,60f)]
        public float checkTime = 5f;
        [Range(0.1f,20f)]
        public float maxRadius = 5f;

        public override void Act(StateController controller)
        {
            if (controller.statePrefs["SoucePos"] == null)           // 第一次进入决定时，设置初始位置，初始化倒计时
                InitPrefs(controller);

            // 更新倒计时
            controller.statePrefs["FrozenCountDown"] = (float)controller.statePrefs["FrozenCountDown"] - Time.deltaTime;
            if ((float)controller.statePrefs["FrozenCountDown"] > 0)
                return;

            // 判断如果过了检测时间后，当前位置和之前位置的距离是否小于限定距离
            if (Vector3.SqrMagnitude((Vector3)controller.statePrefs["SoucePos"] - controller.transform.position) <= maxRadius * maxRadius)
                controller.UpdateNextWayPoint(true);

            CleanPrefs(controller);
        }

        /// <summary>
        /// 初始化信息字典
        /// </summary>
        /// <param name="controller"></param>
        private void InitPrefs(StateController controller)
        {
            controller.statePrefs["SoucePos"] = controller.transform.position;
            controller.statePrefs["FrozenCountDown"] = checkTime;
        }

        /// <summary>
        /// 清空信息字典（超过检测时间后，重新计时检测）
        /// </summary>
        /// <param name="controller"></param>
        private void CleanPrefs(StateController controller)
        {
            controller.statePrefs.Remove("SoucePos");
            controller.statePrefs.Remove("FrozenCountDown");
        }

    }
}
