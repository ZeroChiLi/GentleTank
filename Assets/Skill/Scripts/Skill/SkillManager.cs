using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour 
{
    public Image buttonImage;           // 按钮图片
    public Slider slider;               // 滑动条
    public Image fullImage;             // 滑动条图片
    public Image emptyImage;            // 滑动条背景图片
    public EventTrigger evenTrigger;    // 事件触发器
    public Skill skill;                 // 技能
}
