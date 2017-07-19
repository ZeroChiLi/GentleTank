using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Skill/Skill Model")]
public class SkillModel : ScriptableObject 
{
    public string skillName;                    // 技能名称
    public GameObject skillModelPerfab;         // 技能按钮模型
    public Skill skill;                         // 技能控制
    public Sprite CDFullSprite;                 // 技能滑动条充满时图片精灵
    public Sprite CDEmptySprite;                // 技能滑动条空时图片精灵

    private GameObject newSkillButton;          // 创建出来的技能按钮
    private SkillManager modelComponent;        // 模型的主要组件

    /// <summary>
    /// 通过输入的属性创建技能按钮
    /// </summary>
    /// <returns></returns>
    public GameObject CreateSkillButton(Transform parent)
    {
        newSkillButton = Instantiate(skillModelPerfab, parent);
        modelComponent = newSkillButton.GetComponent<SkillManager>();
        modelComponent.buttonImage = newSkillButton.GetComponent<Image>();
        modelComponent.fullImage.sprite = CDFullSprite;
        modelComponent.emptyImage.sprite = CDEmptySprite;

        modelComponent.skill = skill;
        modelComponent.skill.InitSkill(modelComponent.slider, modelComponent.buttonImage, modelComponent.evenTrigger);

        return newSkillButton;
    }

}
