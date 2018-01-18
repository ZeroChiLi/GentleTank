using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    [System.Serializable]
    public class TagWithColor
    {
        public string tag;          // 标签
        public Color color;         // 颜色
    }

    [System.Serializable]
    [CreateAssetMenu(menuName = "GameSystem/Skill/Aim Mode")]
    public class AimMode : ScriptableObject
    {
        public bool showInGround = false;                   // 是否显示在世界空间
        public Sprite groundSprite;                         // 显示在世界空间的图片精灵
        public float groundSpriteRadius = 5f;               // 世界空间的图片半径
        public bool showInScreen = true;                    // 是否显示在屏幕
        public Sprite screenSprite;                         // 显示在屏幕的精灵
        public float screenSpriteRadius = 32f;              // 瞄准图片半径
        public Color normalColor = Color.black;             // 正常状态颜色
        public Color disableColor = Color.gray;             // 无效状态颜色
        public List<TagWithColor> tagColorList = new List<TagWithColor>();  // 标签颜色列表

        public TagWithColor this[int index]
        {
            get { return tagColorList[index]; }
            set { tagColorList[index] = value; }
        }

        /// <summary>
        /// 获取标签及其颜色数据，失败返回空
        /// </summary>
        /// <param name="tag">标签名</param>
        /// <returns>标签及其颜色数据</returns>
        public TagWithColor GetTagWithColorByTag(string tag)
        {
            for (int i = 0; i < tagColorList.Count; i++)
                if (tagColorList[i].tag == tag)
                    return tagColorList[i];
            return null;
        }
    }
}