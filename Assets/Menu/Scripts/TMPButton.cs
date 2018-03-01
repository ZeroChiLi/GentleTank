using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(TextMeshPro))]
public class TMPButton : MonoBehaviour
{
    public bool interactable = true;
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
    }


    private void OnMouseEnter()
    {
        if (InteractableCheck() && isPressed == false)
            tmp.color = apperance.highlightedColor;
    }

    private void OnMouseUp()
    {
        if (!InteractableCheck()) return;
        if (isPressed)
        {
            clickedEvent.Invoke();
            tmp.color = apperance.highlightedColor;
        }
        isPressed = false;
    }

    private void OnMouseExit()
    {
        if (!InteractableCheck()) return;
        tmp.color = apperance.normalColor;
        isPressed = false;
    }

    private void OnMouseDown()
    {
        if (!InteractableCheck()) return;
        tmp.color = apperance.pressedColor;
        isPressed = true;
    }

    /// <summary>
    /// 可交互性检测
    /// </summary>
    private bool InteractableCheck()
    {
        if (!interactable)
            tmp.color = apperance.disabledColor;
        return interactable;
    }

}
