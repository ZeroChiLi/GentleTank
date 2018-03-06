using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(TextMeshPro))]
public class TMPButton : MonoBehaviour
{
    [System.Serializable]
    public class Appearance
    {
        public Color normalColor = Color.white;
        public Color highlightedColor = new Color(0.9f, 0.9f, 0.9f, 1f);
        public Color pressedColor = Color.gray;
        public Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }
    public Appearance apperance;        // 外观配置

    [System.Serializable]
    public class TMPBtnEvent
    {
        public UnityEvent clickedEvent;
        public UnityEvent mouseEnterEvent;
        public UnityEvent mouseExitEvent;
        public UnityEvent mouseUpEvent;
        public UnityEvent mouseDownEvent;
        public UnityEvent disableEvent;
    }
    public TMPBtnEvent events;

    private bool isPressed = false;
    private TextMeshPro tmp;            // 绑定的文本

    private void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
        if (!enabled)
            tmp.color = apperance.disabledColor;
    }

    private void OnEnable()
    {
        tmp.color = apperance.normalColor;
    }

    private void OnDisable()
    {
        tmp.color = apperance.disabledColor;
        events.disableEvent.Invoke();
    }

    private void OnMouseEnter()
    {
        if (!enabled) return;
        if (isPressed == false)
        {
            tmp.color = apperance.highlightedColor;
            events.mouseEnterEvent.Invoke();
        }
    }

    private void OnMouseUp()
    {
        if (!enabled) return;
        events.mouseUpEvent.Invoke();
        if (isPressed)
        {
            tmp.color = apperance.highlightedColor;
            events.clickedEvent.Invoke();
        }
        isPressed = false;
    }

    private void OnMouseExit()
    {
        if (!enabled) return;
        tmp.color = apperance.normalColor;
        isPressed = false;
        events.mouseExitEvent.Invoke();
    }

    private void OnMouseDown()
    {
        if (!enabled) return;
        tmp.color = apperance.pressedColor;
        isPressed = true;
        events.mouseDownEvent.Invoke();
    }

}
