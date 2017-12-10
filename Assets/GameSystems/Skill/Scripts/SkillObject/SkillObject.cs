using System.Collections;
using UnityEngine;

namespace GameSystem.Skill
{
    public abstract class SkillObject : ScriptableObject
    {
        public string skillName;                        // 技能名称
        public GameObject skillModelPerfab;             // 技能按钮模型
        public float coolDownTime = 1f;                 // 冷却时间
        public Sprite CDFullSprite;                     // 技能滑动条充满时图片精灵
        public Sprite CDEmptySprite;                    // 技能滑动条空时图片精灵
        public AimMode aimMode;                         // 技能对应的瞄准模式

        /// <summary>
        /// 通过属性配置创建技能按钮
        /// </summary>
        /// <returns>返回创建好的按钮</returns>
        public GameObject CreateSkillButton(Transform parent)
        {
            GameObject newSkillButton = Instantiate(skillModelPerfab, parent);
            newSkillButton.name = skillName;
            newSkillButton.GetComponent<SkillManager>().InitSkillManager(this, CDFullSprite, CDEmptySprite);
            Init();
            return newSkillButton;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        abstract public void Init();

        /// <summary>
        /// 技能效果
        /// </summary>
        abstract public IEnumerator SkillEffect(PlayerManager launcher = null);

        /// <summary>
        /// 自定义条件判断是否释放技能
        /// </summary>
        /// <returns>返回是否满足自定义条件</returns>
        abstract public bool ReleaseCondition();

    }
}