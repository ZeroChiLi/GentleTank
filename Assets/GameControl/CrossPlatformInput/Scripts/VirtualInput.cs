using System.Collections;
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
                Debug.LogError("RegisterAxis Error: " + axisInput.AxisName + " have been exists.");
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
        /// 获取轴值
        /// </summary>
        /// <param name="name">输入轴的名称</param>
        /// <returns></returns>
        static Vector2 GetAxis(string name)
        {
            if (axisInputDic.ContainsKey(name))
                return axisInputDic[name].AxisValue;
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
            if (buttonInputDic.ContainsKey(buttonInput.ButtonName))
                Debug.LogError("RegisterButton Error: " + buttonInput.ButtonName + " have been exists.");
            else
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
        /// 获取按钮状态
        /// </summary>
        /// <param name="buttonName">按钮输入对象的名称</param>
        /// <returns>按钮状态</returns>
        static ButtonInput.ButtonState GetButton(string buttonName)
        {
            if (buttonInputDic.ContainsKey(buttonName))
                return buttonInputDic[buttonName].State;
            return ButtonInput.ButtonState.None;
        }

        #endregion

    }
}