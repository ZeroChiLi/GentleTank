
using UnityEngine;

namespace CameraRig
{
    /// <summary>
    /// 固定距离时刻跟随目标
    /// </summary>
    public class FixedTargetFollower : AbstractTargetFollower
    {
        public Vector3 fixedDistance;

        protected override void FollowTarget(float deltaTime)
        {
            if (Target == null)
                return;
            transform.position = Target.position + fixedDistance;
            transform.rotation = Quaternion.Euler(0,Target.rotation.eulerAngles.y,0);
        }
    }
}