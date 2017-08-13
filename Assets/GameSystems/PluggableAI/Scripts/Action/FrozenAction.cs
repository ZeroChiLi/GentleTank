using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 判断一段时间后，是否还在原来位置的半径范围内
    /// </summary>
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
                controller.statePrefs["SoucePos"] = controller.transform.position;

            // 更新倒计时
            if (!CountDownTimer.UpdateTimer(controller.statePrefs, "FrozenActionCD", checkTime, Time.deltaTime, true))
                return;

            // 判断如果过了检测时间后，当前位置和之前位置的距离是否小于限定距离

            if (GameMathf.TwoPosInRange((Vector3)controller.statePrefs["SoucePos"],controller.transform.position,maxRadius))
                controller.UpdateNextWayPoint(true);

            controller.statePrefs.Remove("SoucePos");
        }
    }
}
