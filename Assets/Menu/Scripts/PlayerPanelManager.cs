using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerPanelManager : MonoBehaviour 
{
    public bool isEnter;
    public Text enterText;
    public string enterKey;
    public UnityEvent OnPlayerEnter;

    public void Awake()
    {
        enterText.text = string.Format("按“{0}”加入", enterKey);
    }

    public void Update()
    {
        if (!isEnter && Input.GetButtonDown(enterKey))
        {
            isEnter = true;
            OnPlayerEnter.Invoke();
        }
    }

}
