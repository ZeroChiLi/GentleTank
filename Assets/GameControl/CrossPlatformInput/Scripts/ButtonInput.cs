using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossPlatformInput
{
    /// <summary>
    /// 按钮输入控制
    /// </summary>
    public abstract class ButtonInput : MonoBehaviour
    {
        public enum ButtonState
        {
            None,Pressed
        }
        public string ButtonName;
        public ButtonState State { get; protected set; }

        public ButtonInput(string name)
        {
            ButtonName = name;
        }

        /// <summary>
        /// 按钮按下
        /// </summary>
        public void ButtonDown()
        {
            OnButtonDown();
            State = ButtonState.Pressed;
        }

        /// <summary>
        /// 按钮松开
        /// </summary>
        public void ButtonUp()
        {
            OnButtonUp();
            State = ButtonState.None;
        }

        /// <summary>
        /// 按下时事件
        /// </summary>
        abstract public void OnButtonDown();

        /// <summary>
        /// 起来时事件
        /// </summary>
        abstract public void OnButtonUp();
    }

}
