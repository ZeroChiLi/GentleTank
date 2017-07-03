using UnityEditor;
using UnityEngine;

public class PlayerInfoUI : MonoBehaviour
{
    public bool showPlayerInfo;                     // 是否显示玩家信息

    private Camera targetCamera;                    // 显示对着的镜头（默认：主相机）
    private GUIStyle style;                         // GUI风格  
    private string playerName;                      // 要显示的文本 
    private Collider playerCollider;                // 玩家的碰撞体（就用来发出射线到相机计算距离）

    /// <summary>
    /// 获取目标镜头和玩家碰撞体
    /// </summary>
    private void Awake()
    {
        targetCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerCollider = GetComponent<Collider>();
        SetupGUIStyle();
    }

    /// <summary>
    /// 初始化GUI风格
    /// </summary>
    private void SetupGUIStyle()
    {
        style = new GUIStyle(EditorStyles.largeLabel);
        style.alignment = TextAnchor.MiddleCenter;                  // 文本居中
        style.fontSize = 30;                                        // 字体大小
        style.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 1f);   // 字体颜色 
    }

    /// <summary>
    /// 设置显示到镜头玩家名字
    /// </summary>
    /// <param name="name">名字</param>
    public void SetNameText(string name)
    {
        playerName = name;
    }

    /// <summary>
    /// 设置名字颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetNameColor(Color color)
    {
        style.normal.textColor = color;
    }

    /// <summary>
    /// 根据距离计算并显示玩家信息
    /// </summary>
    private void OnGUI()
    {
        if (showPlayerInfo)
        {
            Ray ray = new Ray(transform.position + targetCamera.transform.up * 2f, -targetCamera.transform.up);
            RaycastHit raycastHit;
            playerCollider.Raycast(ray, out raycastHit, Mathf.Infinity);

            //计算距离，为当前摄像机位置减去碰撞位置的长度  
            float distance = (targetCamera.transform.position - raycastHit.point).magnitude;
            //设置字体大小，在26到12之间插值  
            float fontSize = Mathf.Lerp(26, 12, distance / 10f);
            //将得到的字体大小赋给Style.fontSize  
            style.fontSize = (int)fontSize;
            //将文字位置取为得到的光线碰撞位置上方一点  
            Vector3 worldPositon = raycastHit.point + targetCamera.transform.up * distance * 0.03f;
            //世界坐标转屏幕坐标  
            Vector3 screenPosition = targetCamera.WorldToScreenPoint(worldPositon);
            //z坐标值的判断，z值小于零就返回  
            if (screenPosition.z <= 0) { return; }
            //翻转Y坐标值  
            screenPosition.y = Screen.height - screenPosition.y;

            //获取文本尺寸  
            Vector2 stringSize = style.CalcSize(new GUIContent(playerName));
            //计算文本框坐标  
            Rect rect = new Rect(0f, 0f, stringSize.x + 6, stringSize.y + 4);
            //设定文本框中心坐标  
            rect.center = screenPosition - Vector3.up * rect.height * 0.5f;


            //----------------------------------【2.GUI绘制】---------------------------------------------  
            //开始绘制一个简单的文本框  
            Handles.BeginGUI();
            //绘制灰底背景  
            //GUI.color = new Color(0f, 0f, 0f, 0.8f);
            //GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
            //绘制文字  
            GUI.color = new Color(1, 1, 1, 0.8f);
            GUI.Label(rect, playerName, style);
            //结束绘制  
            Handles.EndGUI();
        }
    }
}
