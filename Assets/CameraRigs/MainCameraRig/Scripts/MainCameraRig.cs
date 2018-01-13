using System.Collections.Generic;
using UnityEngine;

namespace CameraRig
{
    public class MainCameraRig : MonoBehaviour
    {
        static private MainCameraRig instance;
        static public MainCameraRig Instance { get { return instance; } }

        public enum Type { OneTarget, MultiTargets }

        public float dampTime = 0.2f;                   // 重新聚焦到的时间
        public float screenEdgeBuffer = 4f;             // 屏幕中最边界到玩家距离
        public float minSize = 6.5f;                    // 屏幕最小尺寸
        public new Camera camera;                       // 摄像机
        public Type currentType = Type.MultiTargets;    // 相机目标类型
        public Transform oneTarget;                     // 单个目标
        public List<Transform> multiTargets;            // 多个目标

        private float zoomSpeed;                        // 缩放速度
        private Vector3 moveVelocity;                   // 移动速度
        private Vector3 averagePos;                     // 需要移到到地点
        private List<Transform> actualTargets = new List<Transform>();          // 实际目标

        private void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// 配置目标
        /// </summary>
        public void Setup(Transform one, List<Transform> multi)
        {
            if (one != null)
                oneTarget = one;
            if (multi != null)
                multiTargets = multi;
        }

        /// <summary>
        /// 固定时间频率更新
        /// </summary>
        private void FixedUpdate()
        {
            if (!UpdateActualTargets())
                return;

            transform.position = Vector3.SmoothDamp(transform.position, FindAveragePosition(), ref moveVelocity, dampTime);
            camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, FindRequiredSize(), ref zoomSpeed, dampTime);
        }

        /// <summary>
        /// 更新实际目标（inactive的物体排除掉）,单个或多个目标。
        /// </summary>
        /// <returns>是否存在至少一个目标</returns>
        private bool UpdateActualTargets()
        {
            actualTargets.Clear();
            switch (currentType)
            {
                case Type.OneTarget:
                    if (oneTarget)
                        actualTargets.Add(oneTarget);
                    break;
                case Type.MultiTargets:
                    if (multiTargets != null)
                    {
                        for (int i = 0; i < multiTargets.Count; i++)
                            if (multiTargets[i].gameObject.activeSelf)
                                actualTargets.Add(multiTargets[i]);
                    }
                    break;
            }

            return actualTargets.Count > 0;
        }

        /// <summary>
        /// 找到平均点放到desiredPosition
        /// </summary>
        private Vector3 FindAveragePosition()
        {
            averagePos = Vector3.zero;

            for (int i = 0; i < actualTargets.Count; i++)
                averagePos += actualTargets[i].position;

            averagePos /= actualTargets.Count;
            return averagePos;
        }

        /// <summary>
        /// 找到合适的大小
        /// </summary>
        /// <returns></returns>
        private float FindRequiredSize()
        {
            float orthographicSize = 0f;
            Vector3 desiredPosToTarget;

            if (actualTargets.Count == 1)   // 只有一个就直接返回最小值
                return minSize;

            // 找到最大需要的修改尺寸
            for (int i = 0; i < actualTargets.Count; i++)
            {
                desiredPosToTarget = GameMathf.Abs(transform.InverseTransformVector(actualTargets[i].position - averagePos));
                orthographicSize = Mathf.Max(orthographicSize, desiredPosToTarget.y, desiredPosToTarget.x / camera.aspect);
            }

            // 加上边界
            orthographicSize += screenEdgeBuffer;
            orthographicSize = Mathf.Max(orthographicSize, minSize);
            return orthographicSize;
        }
    }
}