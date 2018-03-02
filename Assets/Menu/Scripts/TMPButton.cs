using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(TextMeshPro))]
public class TMPButton : MonoBehaviour
{
    //public bool interactable = true;
    [System.Serializable]
    public class Appearance
    {
        public Color normalColor = Color.white;
        public Color highlightedColor = new Color(0.9f, 0.9f, 0.9f, 1f);
        public Color pressedColor = Color.gray;
        public Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }
    public Appearance apperance;
    public UnityEvent clickedEvent;

    private bool isPressed = false;
    private TextMeshPro tmp;

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
    }

    private void OnMouseEnter()
    {
        if (enabled && isPressed == false)
            tmp.color = apperance.highlightedColor;
    }

    private void OnMouseUp()
    {
        if (!enabled) return;
        if (isPressed)
        {
            tmp.color = apperance.highlightedColor;
            clickedEvent.Invoke();
        }
        isPressed = false;
    }

    private void OnMouseExit()
    {
        if (!enabled) return;
        tmp.color = apperance.normalColor;
        isPressed = false;
    }

    private void OnMouseDown()
    {
        if (!enabled) return;
        tmp.color = apperance.pressedColor;
        isPressed = true;
    }

    ///// <summary>
    ///// 可交互性检测
    ///// </summary>
    //private bool InteractableCheck()
    //{
    //    if (!interactable)
    //        tmp.color = apperance.disabledColor;
    //    return interactable;
    //}

}
