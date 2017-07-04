using UnityEngine;
using UnityEngine.UI;

public class ArrowPopUp : MonoBehaviour 
{
    public float moveDistance = 0.5f;       //上下浮动范围
    public TextMesh textMesh;               //文本

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - moveDistance / 2 + Mathf.PingPong(Time.time, moveDistance), transform.position.z);
    }

    //设置初始位置
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    // 设置旋转角度
    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    //设置箭头文本
    public void SetText(string text)
    {
        textMesh.text = text;
    }

    //设置箭头颜色
    public void SetColor(Color color)
    {
        GetComponent<Image>().color = color;
    }
}
