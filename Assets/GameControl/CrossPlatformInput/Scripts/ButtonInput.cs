using UnityEngine;

namespace CrossPlatformInput
{
    public enum ButtonState
    {
        None,Down, Pressed , Up
    }
    /// <summary>
    /// 按钮输入控制
    /// </summary>
    public abstract class ButtonInput : MonoBehaviour
    {
        public string ButtonName;
        public float Value { get; protected set; }

        protected float downFrame = -1;
        protected float pressedFrame = -1;
        protected float upFrame = -1;

        public ButtonState State
        { 
            get
            {
                if (Time.frameCount == downFrame)
                    return ButtonState.Down;
                else if (Time.frameCount == upFrame)
                    return ButtonState.Up;
                else if (downFrame < pressedFrame && upFrame < downFrame)
                    return ButtonState.Pressed;
                else
                    return ButtonState.None;
            }
        }

        /// <summary>
        /// 登记按钮
        /// </summary>
        public void Register()
        {
            VirtualInput.RegisterButton(this);
        }

        /// <summary>
        /// 按钮按下
        /// </summary>
        public void OnButtonDown()
        {
            downFrame = Time.frameCount;
            ButtonDownHandle();
        }

        /// <summary>
        /// 按钮按住
        /// </summary>
        public void OnButtonPressed()
        {
            pressedFrame = Time.frameCount;
            ButtonPressedHandle();
        }

        /// <summary>
        /// 按钮松开
        /// </summary>
        public void OnButtonUp()
        {
            upFrame = Time.frameCount;
            ButtonUpHandle();
        }

        /// <summary>
        /// 按下时事件
        /// </summary>
        abstract public void ButtonDownHandle();

        /// <summary>
        /// 按住事件
        /// </summary>
        abstract public void ButtonPressedHandle();

        /// <summary>
        /// 起来时事件
        /// </summary>
        abstract public void ButtonUpHandle();
    }

}
