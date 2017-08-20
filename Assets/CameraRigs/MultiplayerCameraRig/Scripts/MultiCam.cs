using System.Collections.Generic;
using UnityEngine;

namespace CameraRig
{
    public class MultiCam : MonoBehaviour
    {
        public float dampTime = 0.2f;                   // 重新聚焦到的时间
        public float screenEdgeBuffer = 4f;             // 屏幕中最边界到玩家距离
        public float minSize = 6.5f;                    // 屏幕最小尺寸
        public Camera controlCamera;                    // 摄像机
        public List<Transform> targets;                 // 所有参与显示到屏幕的transform

        private float zoomSpeed;                        // 缩放速度
        private Vector3 moveVelocity;                   // 移动速度
        private Vector3 desiredPosition;                // 需要移到到地点

        /// <summary>
        /// 设置初始位置和大小
        /// </summary>
        /// <param name="targets">对象列表</param>
        public void InitStartPositionAndSize(List<Transform> targets = null)
        {
            if (targets != null)
                this.targets = targets;
            FindAveragePosition();
            transform.position = desiredPosition;
            controlCamera.orthographicSize = FindRequiredSize();
        }

        /// <summary>
        /// 固定时间频率更新
        /// </summary>
        private void FixedUpdate()
        {
            if (targets == null)
                return;
            Move();                                    // 移动
            Zoom();                                    // 缩放
        }

        /// <summary>
        /// 移动相机
        /// </summary>
        private void Move()
        {
            FindAveragePosition();
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);
        }

        /// <summary>
        /// 缩放镜头
        /// </summary>
        private void Zoom()
        {
            float requiredSize = FindRequiredSize();
            controlCamera.orthographicSize = Mathf.SmoothDamp(controlCamera.orthographicSize, requiredSize, ref zoomSpeed, dampTime);
        }

        /// <summary>
        /// 找到平均点放到desiredPosition
        /// </summary>
        private void FindAveragePosition()
        {
            Vector3 averagePos = new Vector3();
            int numTargets = 0;

            for (int i = 0; i < targets.Count; i++)
            {
                // 死掉的不计算
                if (!targets[i].gameObject.activeSelf)
                    continue;

                averagePos += targets[i].position;
                numTargets++;
            }

            if (numTargets > 0)
                averagePos /= numTargets;
            averagePos.y = transform.position.y;
            desiredPosition = averagePos;
        }

        /// <summary>
        /// 找到合适的大小
        /// </summary>
        /// <returns></returns>
        private float FindRequiredSize()
        {
            // 世界坐标转到本地坐标
            Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPosition);
            float size = 0f;

            // 找到最大需要的修改尺寸
            for (int i = 0; i < targets.Count; i++)
            {
                if (!targets[i].gameObject.activeSelf)
                    continue;

                Vector3 targetLocalPos = transform.InverseTransformPoint(targets[i].position);
                Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / controlCamera.aspect);
            }

            // 加上边界
            size += screenEdgeBuffer;
            size = Mathf.Max(size, minSize);
            return size;
        }
    }
}