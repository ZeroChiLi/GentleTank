
public abstract class DoubleClickCDButton : CoolDownButton
{
    public override void OnClickSuccessed() { OnFristClickSuccessed(); }

    public abstract void OnFristClickSuccessed();

    public abstract bool OnSecondClick();

    public abstract void OnSecondClickFailed();

    public abstract void OnSecondClickSuccessed();

}
