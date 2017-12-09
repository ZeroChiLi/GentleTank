using UnityEngine;
using UnityEngine.EventSystems;

public class SkillEventSystem : MonoBehaviour
{
    static public SkillEventSystem Instance { private set; get; }

    private SkillButton skillButton;

    private void Awake() { Instance = this; }

    private void Update()
    {
        // 有第一次技能按钮点击标记、且第二次点击发生成功时调用
        if (skillButton != null && skillButton.OnSecondClicked())
        {
            if (skillButton.isSecondClickSuccess)
                skillButton.OnSecondClickedSuccessed();
            else
                skillButton.OnSecondClickedCanceled();
            skillButton = null;
        }
    }

    /// <summary>
    /// 技能按钮被点击时调用，第一次点击时做记录（一次就释放技能直接释放，两次点击就设置标记），第二次点击就取消技能释放准备。
    /// </summary>
    /// <param name="target">传入的点击按钮</param>
    public void SkillButtonClicked(SkillButton target)
    {
        if (skillButton == null)
        {
            skillButton.OnFristClickedSuccess();
            if (target.clickedTimes == SkillButton.ClickedTimes.Once)
            {
                skillButton.RelaseSkill();
                skillButton = null;
            }
            else
                skillButton = target;
        }
        else
        {
            skillButton = null;
            skillButton.OnSecondClickedCanceled();
        }
    }

}
