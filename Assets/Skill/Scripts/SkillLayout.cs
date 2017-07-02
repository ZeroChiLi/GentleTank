using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillLayout : MonoBehaviour
{
    public List<GameObject> skillObjectList;                        // 所有需要显示的技能

    private List<Skill> skillList;                                  // 所有技能对应的技能脚本
    private GridLayoutGroup gridLayoutGroup;                        // 技能布局
    private RectTransform rectTransform;                            // 技能布局位置大小
    private int currentSkillIndex = -1;                             // 当前鼠标指向技能索引
    private int readySkillIndex = -1;                               // 准备释放的技能索引

    /// <summary>
    /// 获取大小位置，每个技能按键大小，距离间隔
    /// </summary>
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        skillList = new List<Skill>();
    }

    /// <summary>
    /// 创建技能列表到这个对象里面
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < skillObjectList.Count; i++)
        {
            skillObjectList[i] = Instantiate(skillObjectList[i], transform);
            skillList.Add(skillObjectList[i].GetComponent<Skill>());
        }
    }

    /// <summary>
    /// 获取鼠标位置，当点击时，改变技能状态
    /// </summary>
    private void Update()
    {
        UpdateCurrentSkillIndex(Input.mousePosition);

        // 如果技能进入准备状态，随着鼠标位置是否在按钮上，若在，取消瞄准激活
        if (readySkillIndex != -1)
        {
            if (OnSkillButton())
                skillList[readySkillIndex].SetAimActive(false);
            else
                skillList[readySkillIndex].SetAimActive(true);
        }

        if (!Input.GetMouseButtonUp(0))                 // 点击了才响应
            return;
        if (OnSkillButton())                            // 根据点击位置做相应的响应
            SkillOnClicked();
        else
            SceneOnClicked();
    }

    /// <summary>
    /// 获取点击的技能的索引值，无效返回-1
    /// </summary>
    /// <param name="position">点击的位置</param>
    /// <returns></returns>
    private void UpdateCurrentSkillIndex(Vector3 position)
    {
        Rect cellRect = new Rect(rectTransform.offsetMin, gridLayoutGroup.cellSize);
        for (int i = 0; i < skillObjectList.Count; i++)
        {
            if (cellRect.Contains(position))
            {
                currentSkillIndex = i;
                return;
            }
            cellRect.position += gridLayoutGroup.spacing + new Vector2(gridLayoutGroup.cellSize.x, 0f);
        }
        currentSkillIndex = -1;
    }

    /// <summary>
    /// 判断鼠标是否在任意技能按钮上面，就是判断currentSkillIndex是不是不等于-1
    /// </summary>
    /// <returns>返回True如果鼠标在任意技能上</returns>
    public bool OnSkillButton()
    {
        return currentSkillIndex != -1;
    }

    /// <summary>
    /// 点击了技能按钮
    /// </summary>
    private void SkillOnClicked()
    {
        // 如果这是第一次点击按钮
        if (readySkillIndex == -1)
        {
            //且可以进入准备状态（过了冷却时间），那就进入准备状态
            if (skillList[currentSkillIndex].CanReady())
            {
                skillList[currentSkillIndex].Ready();
                readySkillIndex = currentSkillIndex;
            }
        }
        // 第二次点击任意技能，都会取消上一次技能的准备状态
        else
        {
            skillList[readySkillIndex].Cancel();
            readySkillIndex = -1;
        }
    }

    /// <summary>
    /// 点击了场景（除了技能按钮）
    /// </summary>
    private void SceneOnClicked()
    {
        //已经点击了技能，且满足释放条件（过了冷却时间和满足自定义条件）就释放该技能
        if (readySkillIndex != -1 && skillList[readySkillIndex].CanRelease())
        {
            skillList[readySkillIndex].Release();
            readySkillIndex = -1;
        }
    }

}
