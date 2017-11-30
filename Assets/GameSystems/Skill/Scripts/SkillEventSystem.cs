using UnityEngine;
using UnityEngine.EventSystems;

public class SkillEventSystem : MonoBehaviour
{
    static public SkillEventSystem Instance { private set; get; }

    public EventSystem eventSystem;

    public bool skillButtonClicked;
    public SkillButton skillButton;

    private void Awake() { Instance = this; }

    private void Update()
    {
        if (skillButton != null && skillButton.OnSecondClicked())
        {
            if (skillButton.isSecondClickSuccess)
                skillButton.OnSecondClickedSuccessed();
            else
                skillButton.OnSecondClickedCanceled();
            skillButton = null;
        }
    }

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
