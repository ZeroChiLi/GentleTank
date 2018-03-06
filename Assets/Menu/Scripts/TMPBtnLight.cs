using UnityEngine;

public class TMPBtnLight : MonoBehaviour 
{
    public TMPButton tmpBtn;
    public new Light light;

    public void OnEnable()
    {
        tmpBtn.events.mouseEnterEvent.AddListener(EnterEvent);
        tmpBtn.events.mouseExitEvent.AddListener(ExitEvent);
        tmpBtn.events.mouseDownEvent.AddListener(DownEvent);
        tmpBtn.events.clickedEvent.AddListener(ClickedEvent);
        tmpBtn.events.disableEvent.AddListener(DisableEvent);
    }

    public void OnDisable()
    {
        tmpBtn.events.mouseEnterEvent.RemoveListener(EnterEvent);
        tmpBtn.events.mouseExitEvent.RemoveListener(ExitEvent);
        tmpBtn.events.mouseDownEvent.RemoveListener(DownEvent);
        tmpBtn.events.clickedEvent.RemoveListener(ClickedEvent);
        tmpBtn.events.disableEvent.RemoveListener(DisableEvent);
    }

    private void EnterEvent()
    {
        light.enabled = true;
        light.color = tmpBtn.apperance.highlightedColor;
    }

    private void ExitEvent()
    {
        light.enabled = false;
    }

    private void ClickedEvent()
    {
        light.color = tmpBtn.apperance.highlightedColor;
    }

    private void DownEvent()
    {
        light.color = tmpBtn.apperance.pressedColor;
    }

    private void DisableEvent()
    {
        light.enabled = false;
    }
}
