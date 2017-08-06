using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : MonoBehaviour 
{
    /// <summary>
    /// 摇杆控制选项
    /// </summary>
    public enum AxisOption
    {
        Both,               // 同时控制竖直和水平方向
        OnlyHorizontal,     // 只控制水平方向
        OnlyVertical        // 只控制垂直方向
    }

    public Transform joystick;                              // 摇杆
    public int MovementRange = 100;                         // 摇杆移动范围
    public AxisOption axesToUse = AxisOption.Both;          // 摇杆控制选项
    public string horizontalAxisName = "Horizontal";        // 水平方向名称
    public string verticalAxisName = "Vertical";            // 竖直方向名称

    private Vector3 startPos;                               // 原点位置
    private bool useX;                                      // 是否使用X
    private bool useY;                                      // 是否使用Y

    void OnEnable()
    {
        useX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
        useY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);
    }

    void Start()
    {
        startPos = transform.position;
    }

    //void UpdateVirtualAxes(Vector3 value)
    //{
    //    var delta = startPos - value;
    //    delta.y = -delta.y;
    //    delta /= MovementRange;
    //    if (useX)
    //    {
    //        m_HorizontalVirtualAxis.Update(-delta.x);
    //    }

    //    if (useY)
    //    {
    //        m_VerticalVirtualAxis.Update(delta.y);
    //    }
    //}

    //public void OnDrag(PointerEventData data)
    //{
    //    Vector3 newPos = Vector3.zero;

    //    if (useX)
    //    {
    //        int delta = (int)(data.position.x - startPos.x);
    //        delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
    //        newPos.x = delta;
    //    }

    //    if (useY)
    //    {
    //        int delta = (int)(data.position.y - startPos.y);
    //        delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
    //        newPos.y = delta;
    //    }
    //    transform.position = new Vector3(startPos.x + newPos.x, startPos.y + newPos.y, startPos.z + newPos.z);
    //    UpdateVirtualAxes(transform.position);
    //}


    //public void OnPointerUp(PointerEventData data)
    //{
    //    transform.position = startPos;
    //    UpdateVirtualAxes(startPos);
    //}


    //public void OnPointerDown(PointerEventData data) { }

    //void OnDisable()
    //{
    //    // remove the joysticks from the cross platform input
    //    if (useX)
    //    {
    //        m_HorizontalVirtualAxis.Remove();
    //    }
    //    if (useY)
    //    {
    //        m_VerticalVirtualAxis.Remove();
    //    }
    //}
}
