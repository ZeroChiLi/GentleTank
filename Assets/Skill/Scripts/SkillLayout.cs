using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillLayout : MonoBehaviour
{
    public List<GameObject> skillObjectList;

    public int SkillCount { get { return skillObjectList.Count; } }

    private List<Skill> skillList;
    private GridLayoutGroup gridLayoutGroup;
    private RectTransform rectTransform;
    private int currentSkillIndex = -1;
    private int readySkillIndex = -1;

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
        if (Input.GetMouseButtonUp(0))                  // 点击时响应
        {
            if (currentSkillIndex == -1)                // 如果点了按钮以外的位置
            {
                // 如果已经有按钮被点击一次，就释放该技能，否则直接return
                if (readySkillIndex != -1 && skillList[readySkillIndex].CanRelease())              
                {
                    skillList[readySkillIndex].Release();
                    readySkillIndex = -1;
                }
            }
            else                                        // 如果点击了技能按钮
            {
                if (readySkillIndex != -1)              // 如果之前已经点击过按钮，那就取消该技能准备
                {
                    skillList[readySkillIndex].Cancel();
                    readySkillIndex = -1;
                }
                else                                    // 如果之前没点击按钮（就是这是第一次点击按钮）
                {
                    if (skillList[currentSkillIndex].OnClicked())       // 如果点击成功（进入技能准备释放状态）
                        readySkillIndex = currentSkillIndex;
                    else                                // 点击准备失败（没过完冷却时间）
                        readySkillIndex = -1;
                }
            }
        }
    }

    /// <summary>
    /// 获取点击的技能的索引值，无效返回-1
    /// </summary>
    /// <param name="position">点击的位置</param>
    /// <returns></returns>
    public void UpdateCurrentSkillIndex(Vector3 position)
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


}
