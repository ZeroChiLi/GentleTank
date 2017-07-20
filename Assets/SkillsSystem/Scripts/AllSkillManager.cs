using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllSkillManager : MonoBehaviour
{
    public static AllSkillManager Instance;                    // 技能管理单例

    public Aim aim;                                         // 瞄准光标
    public List<Skill> skillList;                           // 所有技能对应的技能脚本

    private List<Rect> skillRectList;                       // 所有技能方块
    private GridLayoutGroup gridLayoutGroup;                // 技能布局
    private RectTransform rectTransform;                    // 技能布局位置大小
    private int currentSkillIndex = -1;                     // 当前鼠标指向技能索引
    private int readySkillIndex = -1;                       // 准备释放的技能索引
    private bool allSkillEnable = false;                    // 所有技能是否有效

    /// <summary>
    /// 获取大小位置，每个技能按键大小，距离间隔
    /// </summary>
    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        skillRectList = new List<Rect>();
    }

    ///// <summary>
    ///// 创建技能列表到这个对象里面
    ///// </summary>
    private void Start()
    {
        Vector2 currentPosition = rectTransform.offsetMin;
        for (int i = 0; i < skillList.Count; i++)
        {
            //创建技能按钮
            skillList[i].CreateSkillButton(transform);

            //计算每个按钮所占区域
            skillRectList.Add(new Rect(currentPosition, gridLayoutGroup.cellSize));
            currentPosition += gridLayoutGroup.spacing + new Vector2(gridLayoutGroup.cellSize.x, 0f);
        }
    }

    /// <summary>
    /// 获取鼠标位置，当点击时，改变技能状态
    /// </summary>
    private void Update()
    {
        //不在回合中就跳出
        if (GameRecord.Instance.CurrentGameState != GameState.Playing)
        {
            readySkillIndex = -1;
            aim.SetActive(false);
            SetAllSkillEnable(false);
            return;
        }

        SetAllSkillEnable(true);
        UpdateAllSkillCoolDown();
        UpdateCurrentSkillIndex(Input.mousePosition);


        // 如果技能进入准备状态，随着鼠标位置是否在按钮上，若在，取消瞄准激活
        if (readySkillIndex != -1)
        {
            if (OnSkillButton())
                aim.SetEnable(false);
            else
                aim.SetEnable(true);
        }

        if (!Input.GetMouseButtonUp(0))                 // 点击了才响应
            return;
        if (OnSkillButton())                            // 根据点击位置做相应的响应
            SkillOnClicked();
        else
            SceneOnClicked();
    }

    /// <summary>
    /// 设置所有技能是否有效
    /// </summary>
    /// <param name="enable">是否有效</param>
    private void SetAllSkillEnable(bool enable)
    {
        if (allSkillEnable == enable)
            return;
        allSkillEnable = enable;
        for (int i = 0; i < skillList.Count; i++)
            skillList[i].SetSkillEnable(enable);
    }

    /// <summary>
    /// 更新所有技能冷却时间
    /// </summary>
    private void UpdateAllSkillCoolDown()
    {
        for (int i = 0; i < skillList.Count; i++)
            skillList[i].UpdateCoolDown();
    }

    /// <summary>
    /// 获取点击的技能的索引值，结果返回到currentSkillIndex，无效为-1
    /// </summary>
    /// <param name="position">点击的位置</param>
    /// <returns></returns>
    private void UpdateCurrentSkillIndex(Vector3 position)
    {
        for (int i = 0; i < skillRectList.Count; i++)
            if (skillRectList[i].Contains(position))
            {
                currentSkillIndex = i;
                return;
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
                aim.SetAimMode(skillList[currentSkillIndex].aimMode);
                aim.SetActive(true);
            }
        }
        // 第二次点击任意技能，都会取消上一次技能的准备状态
        else
        {
            skillList[readySkillIndex].Cancel();
            aim.SetActive(false);
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
            StartCoroutine(skillList[readySkillIndex].SkillEffect());
            aim.SetActive(false);
            readySkillIndex = -1;
        }
    }

}
