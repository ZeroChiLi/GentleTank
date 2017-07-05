using UnityEngine;
using UnityEngine.UI;

public class ArrowPopUp : MonoBehaviour 
{
    public float moveDistance = 0.5f;       //上下浮动范围
    public TextMesh textMesh;               //文本

    /// <summary>
    /// 上下浮动箭头
    /// </summary>
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - moveDistance / 2 + Mathf.PingPong(Time.time, moveDistance), transform.position.z);
    }

    /// <summary>
    /// 设置初位置
    /// </summary>
    /// <param name="position">位置</param>
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    
    /// <summary>
    /// 设置旋转角度
    /// </summary>
    /// <param name="rotation">旋转角</param>
    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
    
    /// <summary>
    /// 设置箭头文本
    /// </summary>
    /// <param name="text">文本内容</param>
    public void SetText(string text)
    {
        textMesh.text = text;
    }
    
    /// <summary>
    /// 设置箭头颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        GetComponent<Image>().color = color;
    }
}
