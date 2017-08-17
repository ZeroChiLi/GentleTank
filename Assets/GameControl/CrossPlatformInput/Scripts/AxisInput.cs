using UnityEngine;

namespace CrossPlatformInput
{
    public abstract class AxisInput :MonoBehaviour
    {
        public string AxisName;
        public float AxisValueX { get { return axisValue.x; } }
        public float AxisValueY { get { return axisValue.y; } }
        protected Vector2 axisValue;
        public Vector2 AxisValue
        {
            get { return axisValue; }
            set { axisValue = new Vector2(Mathf.Clamp01(value.x), Mathf.Clamp01(value.y)); }
        }

        /// <summary>
        /// 登记摇杆
        /// </summary>
        private void Awake()
        {
            Register();
        }

        /// <summary>
        /// 登记摇杆
        /// </summary>
        public void Register()
        {
            VirtualInput.RegisterAxis(this);
        }

        /// <summary>
        /// 设置轴值
        /// </summary>
        /// <param name="value"></param>
        abstract public void SetAxis(Vector2 value);

    }
}