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
    private Vector2 leftDownPosition;
    private Vector2 cellSize;
    private Vector2 spacing;
    private int currentSkillIndex;

    // 获取大小位置，每个技能按键大小，间隔
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        cellSize = gridLayoutGroup.cellSize;
        spacing = gridLayoutGroup.spacing;
        skillList = new List<Skill>();
    }

    private void Start()
    {
        for (int i = 0; i < skillObjectList.Count; i++)
        {
            skillObjectList[i] = Instantiate(skillObjectList[i], transform);
            skillList.Add(skillObjectList[i].GetComponent<Skill>());
        }
    }

    private void Update()
    {
        int currentSkillIndex = GetSkillIndex(Input.mousePosition);
        if (currentSkillIndex == -1)
            return;
        //skillList[currentSkillIndex].OnMouseOnButton(true);
        if (Input.GetMouseButtonUp(0))
            skillList[currentSkillIndex].OnClicked();
    }

    /// <summary>
    /// 获取点击的技能的索引值，无效返回-1
    /// </summary>
    /// <param name="position">点击的位置</param>
    /// <returns></returns>
    public int GetSkillIndex(Vector3 position)
    {
        Rect cellRect = new Rect(rectTransform.offsetMin, cellSize);
        for (int i = 0; i < skillObjectList.Count; i++)
        {
            if (cellRect.Contains(position))
            {
                //Debug.Log("On Button " + i);
                return i;
            }
            cellRect.position += spacing + new Vector2(cellSize.x,0f);
        }
        return -1;
    }


}
