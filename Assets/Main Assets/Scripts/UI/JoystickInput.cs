using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickInput : MonoBehaviour
{
    static private JoystickInput instance;
    static public JoystickInput Instance { get { return instance; } }   // 单例

    public bool showJoystick = true;                        // 是否显示摇杆按钮
    public RectTransform joystick;                          // 摇杆
    public float movementRange = 100;                       // 摇杆移动范围
    public string horizontalAxisName = "Horizontal";        // 水平方向名称
    public string verticalAxisName = "Vertical";            // 竖直方向名称
    public CanvasScaler canvasScaler;                       // 画布缩放

    public bool IsActive { get { return isActive; } }       // 摇杆是否激活

    private float screenRatio;                              // 屏幕缩放比(依据屏幕高)
    private bool isActive;                                  // 是否激活
    private RectTransform rectTransfrom;                    // 本身的矩形变换
    private Vector3 center;                                 // 原点位置
    private bool useX;                                      // 是否使用X
    private bool useY;                                      // 是否使用Y
    private Vector3 relativeDistance;                       // 摇杆相对原点距离

    /// <summary>
    /// 获取矩形变换
    /// </summary>
    private void Awake()
    {
        rectTransfrom = GetComponent<RectTransform>();
        movementRange = Mathf.Max(0.01f, movementRange);        // 避免范围小于等于0
    }

    /// <summary>
    /// 激活摇杆
    /// </summary>
    private void OnEnable()
    {
        instance = this;
        isActive = true;
    }

    /// <summary>
    /// 关闭摇杆
    /// </summary>
    private void OnDisable()
    {
        if (instance == this)
            instance = null;
        isActive = false;
    }

    /// <summary>
    /// 初始化原点和遥控控制
    /// </summary>
    private void Start()
    {
        screenRatio = Screen.height /canvasScaler.referenceResolution.y;
        joystick.GetComponent<Image>().enabled = showJoystick;
        rectTransfrom.sizeDelta = Vector2.one * movementRange;
        movementRange *= screenRatio;
        center = rectTransfrom.position + new Vector3(movementRange/2, movementRange/2,0);
        //Debug.Log(rectTransfrom.sizeDelta + "  " + rectTransfrom.position + "  " + center + "  " + screenRatio +"  " +joystick.transform.position);
    }

    /// <summary>
    /// 获取鼠标输入位置，计算出相对摇杆原点位置
    /// </summary>
    /// <returns>获取的位置</returns>
    public void InputPosition()
    {
        joystick.position = Vector3.ClampMagnitude(Input.mousePosition - center,movementRange /2) +center;
    }

    /// <summary>
    /// 摇杆回到原点
    /// </summary>
    public void BackToCenter()
    {
        joystick.position = center;
    }

    /// <summary>
    /// 获取输入轴大小，失败返回(0,0)
    /// </summary>
    /// <returns>输入轴大小</returns>
    public Vector2 GetAxis()
    {
        if (!isActive)
            return Vector2.zero;
        return (joystick.position - center) / (movementRange / 2);
    }
}
