using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillButton : CoolDownButton
{
    public enum ClickedTimes { Once, Twice }

    public ClickedTimes clickedTimes;

    public override void OnClickSuccessed()
    {
        OnFristClickedSuccess();
        SkillEventSystem.Instance.SkillButtonClicked(this);
    }

    public abstract void OnFristClickedSuccess();

    public abstract bool OnSecondClicked();

    public abstract void OnSecondClickedSuccessed();

    public abstract void OnSecondClickedCanceled();

}
