
public abstract class SkillButton : CoolDownButton
{
    public enum ClickedTimes { Once, Twice }

    public ClickedTimes clickedTimes;
    public bool isSecondClickSuccess;

    public override void OnClickSuccessed()
    {
        SkillEventSystem.Instance.SkillButtonClicked(this);
    }

    public abstract void OnFristClickedSuccess();

    public abstract bool OnSecondClicked();

    public abstract void OnSecondClickedSuccessed();

    public abstract void OnSecondClickedCanceled();

    public abstract void RelaseSkill(params object[] args);

}
