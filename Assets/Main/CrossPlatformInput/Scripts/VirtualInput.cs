using System.Collections.Generic;
using UnityEngine;

namespace CrossPlatformInput
{
    public static class VirtualInput
    {
        static private Dictionary<string, AxisInput> axisInputDic = new Dictionary<string, AxisInput>();
        static private Dictionary<string, ButtonInput> buttonInputDic = new Dictionary<string, ButtonInput>();

        #region AxisInput

        /// <summary>
        /// 注册输入轴
        /// </summary>
        /// <param name="axisInput">输入轴对象</param>
        static public void RegisterAxis(AxisInput axisInput)
        {
            if (axisInputDic.ContainsKey(axisInput.AxisName))
                Debug.LogErrorFormat("RegisterAxis Error: '{0}' have been exists.",axisInput.AxisName);
            else
                axisInputDic.Add(axisInput.AxisName, axisInput);
        }

        /// <summary>
        /// 移除输入轴对象
        /// </summary>
        /// <param name="axisInput">输入轴对象</param>
        static public void RemoveAxis(AxisInput axisInput)
        {
            if (axisInputDic.ContainsKey(axisInput.AxisName))
                axisInputDic.Remove(axisInput.AxisName);
        }

        /// <summary>
        /// 移除输入轴对象（通过轴名称）
        /// </summary>
        /// <param name="axisName">输入轴对象的名称</param>
        static public void RemoveAxis(string axisName)
        {
            if (axisInputDic.ContainsKey(axisName))
                axisInputDic.Remove(axisName);
        }

        /// <summary>
        /// 获取输入轴
        /// </summary>
        /// <param name="axisName">轴名</param>
        /// <returns>输入轴</returns>
        static public AxisInput GetAxis(string axisName)
        {
            if (axisInputDic.ContainsKey(axisName))
                return axisInputDic[axisName];
            Debug.LogErrorFormat("Can't Find '{0}' Axis", axisName);
            return null;
        }

        /// <summary>
        /// 获取轴值
        /// </summary>
        /// <param name="axisName">输入轴的名称</param>
        /// <returns></returns>
        static public Vector2 GetAxisValue(string axisName)
        {
            if (axisInputDic.ContainsKey(axisName))
                return axisInputDic[axisName].AxisValue;
            return Vector2.zero;
        }

        #endregion

        #region ButtonInput

        /// <summary>
        /// 注册按钮
        /// </summary>
        /// <param name="buttonInput">按钮输入对象</param>
        static public void RegisterButton(ButtonInput buttonInput)
        {
            if (!buttonInputDic.ContainsKey(buttonInput.ButtonName))
                buttonInputDic.Add(buttonInput.ButtonName, buttonInput);
        }

        /// <summary>
        /// 移除按钮输入对象
        /// </summary>
        /// <param name="buttonInput">按钮输入对象</param>
        static public void RemoveButton(ButtonInput buttonInput)
        {
            if (buttonInputDic.ContainsKey(buttonInput.ButtonName))
                buttonInputDic.Remove(buttonInput.ButtonName);
        }

        /// <summary>
        /// 移除按钮输入对象（通过名称）
        /// </summary>
        /// <param name="buttonName">按钮输入对象的名称</param>
        static public void RemoveButton(string buttonName)
        {
            if (buttonInputDic.ContainsKey(buttonName))
                buttonInputDic.Remove(buttonName);
        }

        /// <summary>
        /// 获取按钮
        /// </summary>
        /// <param name="buttonName">按钮名称</param>
        /// <returns>按钮</returns>
        static public ButtonInput GetButton(string buttonName)
        {
            if (buttonInputDic.ContainsKey(buttonName))
                return buttonInputDic[buttonName];
            Debug.LogWarningFormat("Can't Find '{0}' Button", buttonName);
            return null;
        }

        /// <summary>
        /// 获取按钮状态
        /// </summary>
        /// <param name="buttonName">按钮输入对象的名称</param>
        /// <returns>按钮状态</returns>
        static public ButtonState GetButtonState(string buttonName)
        {
            if (buttonInputDic.ContainsKey(buttonName))
                return buttonInputDic[buttonName].State;
            return ButtonState.None;
        }

        /// <summary>
        /// 获取按钮值
        /// </summary>
        /// <param name="buttonName">按钮名称</param>
        /// <returns>按钮值</returns>
        static public float GetButtonValue(string buttonName)
        {
            if (buttonInputDic.ContainsKey(buttonName))
                return buttonInputDic[buttonName].Value;
            return 0f;
        }

        #endregion

    }
}