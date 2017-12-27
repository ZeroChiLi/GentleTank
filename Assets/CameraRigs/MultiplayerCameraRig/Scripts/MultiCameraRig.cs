using System.Collections.Generic;
using UnityEngine;

namespace CameraRig
{
    [ExecuteInEditMode]
    public class MultiCameraRig : MonoBehaviour
    {
        public float dampTime = 0.2f;                   // 重新聚焦到的时间
        public float screenEdgeBuffer = 4f;             // 屏幕中最边界到玩家距离
        public float minSize = 6.5f;                    // 屏幕最小尺寸
        public Camera controlCamera;                    // 摄像机
        public List<Transform> targets;                 // 所有参与显示到屏幕的transform

        private float zoomSpeed;                        // 缩放速度
        private Vector3 moveVelocity;                   // 移动速度
        private Vector3 averagePos;                     // 需要移到到地点
        private List<Transform> actualTargets = new List<Transform>();          // 实际目标
        private Vector3 localAveragePos { get { return transform.InverseTransformPoint(averagePos); } } // 本地坐标的平均位置值
        private float orthographicSize;
        private Vector3 desiredPosToTarget;

        /// <summary>
        /// 设置初始位置和大小
        /// </summary>
        /// <param name="targets">对象列表</param>
        public void InitStartPositionAndSize(List<Transform> targets = null)
        {
            if (targets != null)
                this.targets = targets;
            transform.position = FindAveragePosition();
            controlCamera.orthographicSize = FindRequiredSize();
        }

        /// <summary>
        /// 固定时间频率更新
        /// </summary>
        private void Update()
        {
            if (targets == null || !UpdateActualTargets())
                return;
            transform.position = Vector3.SmoothDamp(transform.position, FindAveragePosition(), ref moveVelocity, dampTime);
            controlCamera.orthographicSize = Mathf.SmoothDamp(controlCamera.orthographicSize, FindRequiredSize(), ref zoomSpeed, dampTime);
        }

        /// <summary>
        /// 更新实际目标（inactive的物体排除掉）
        /// </summary>
        /// <returns>是否存在至少一个目标</returns>
        private bool UpdateActualTargets()
        {
            actualTargets.Clear();
            for (int i = 0; i < targets.Count; i++)
                if (targets[i].gameObject.activeSelf)
                    actualTargets.Add(targets[i]);
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

            if (actualTargets.Count > 0)
                averagePos /= actualTargets.Count;
            averagePos.y = transform.position.y;
            return averagePos;
        }

        /// <summary>
        /// 找到合适的大小
        /// </summary>
        /// <returns></returns>
        private float FindRequiredSize()
        {
            orthographicSize = 0f;

            // 找到最大需要的修改尺寸
            for (int i = 0; i < actualTargets.Count; i++)
            {
                //transform.InverseTransformVector
                //desiredPosToTarget = transform.InverseTransformPoint(actualTargets[i].position) - localAveragePos;
                desiredPosToTarget = transform.InverseTransformVector(actualTargets[i].position - averagePos);
                orthographicSize = Mathf.Max(orthographicSize, Mathf.Abs(desiredPosToTarget.y), Mathf.Abs(desiredPosToTarget.x) / controlCamera.aspect);
            }

            // 加上边界
            orthographicSize += screenEdgeBuffer;
            orthographicSize = Mathf.Max(orthographicSize, minSize);
            return orthographicSize;
        }
    }
}