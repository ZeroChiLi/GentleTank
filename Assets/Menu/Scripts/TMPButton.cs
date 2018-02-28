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
        public float fadeDuration = 0.5f;
    }
    public Appearance apperance;
    public UnityEvent clickedEvent;

    private TextMeshPro tmp;
    private Collider colliderSelf;

    private void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
        colliderSelf = GetComponent<Collider>();
    }

    private void Start()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Need Camera With 'MainCamera' Tag.");
            enabled = false;
        }
    }

    private void Update()
    {
        if (interactable == false)
        {
            tmp.color = apperance.disabledColor;
            return;
        }
        if (Input.GetMouseButton(0))
        {
            if (IsOnHit())
                tmp.color = apperance.pressedColor;
        }
        else if (IsOnHit())
            tmp.color = apperance.highlightedColor;
        else
            tmp.color = apperance.normalColor;
    }

    private bool IsOnHit()
    {
        RaycastHit info;
        return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out info, 200) 
            && info.collider == colliderSelf;
    }
}
