using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 在最小和最大范围内的浮动值
    /// </summary>
    [System.Serializable]
    public struct MinMaxValue
    {
        public float minLimit;
        public float maxLimit;
        public float minValue;
        public float maxValue;

        public MinMaxValue(float minLimit, float maxLimit, float minValue, float maxValue)
        {
            this.minLimit = minLimit;
            this.maxLimit = maxLimit;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        /// <summary>
        /// 获取浮动范围内的平均值
        /// </summary>
        /// <returns>浮动范围内的平均值</returns>
        public float GetAverageValue()
        {
            return (minValue + maxValue) / 2;
        }

        /// <summary>
        /// 获取浮动范围内的随机值
        /// </summary>
        /// <returns>浮动范围内的随机值</returns>
        public float GetRandomValue()
        {
            return Random.Range(minValue, maxValue);
        }
    }
}