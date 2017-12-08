using UnityEngine;
using UnityEngine.UI;

namespace Widget
{
    public class ArrowPopUpManager : MonoBehaviour
    {
        public Image arrowImage;
        public TextMesh arrowText;               //文本

        public void Init(Vector3 pos,Color color,string text)
        {
            transform.position = pos;
            arrowImage.color = color;
            arrowText.text = text;
        }

        /// <summary>
        /// 上下浮动箭头
        /// </summary>
        private void Update()
        {
            if (Camera.current != null)
                transform.rotation = Quaternion.Euler(Camera.current.transform.eulerAngles + Vector3.right * 180);
        }
    }
}
