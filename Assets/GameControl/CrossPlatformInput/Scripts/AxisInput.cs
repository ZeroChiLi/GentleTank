using System.Collections;
using System.Collections.Generic;
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

        public AxisInput(string name)
        {
            AxisName = name;
        }

        /// <summary>
        /// 设置轴值
        /// </summary>
        /// <param name="value"></param>
        abstract public void SetAxis(Vector2 value);

    }
}