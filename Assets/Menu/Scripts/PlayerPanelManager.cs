using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerPanelManager : MonoBehaviour 
{
    [System.Serializable]
    public sealed class InputButtons
    {
        public string enterButton = "Fire";
        public string horizontalButton = "Horizontal";
        public string verticalButton = "Vertical";
    }
    public bool isEnter;
    public Text enterText;
    public InputButtons inputsButton;
    public UnityEvent OnPlayerEnter;

    public void Awake()
    {
        enterText.text = string.Format("按“{0}”加入\n按左右键切换坦克\n按上键切换颜色", inputsButton.enterButton);
    }

    public void Update()
    {
        if (!isEnter && Input.GetButtonDown(inputsButton.enterButton))
        {
            isEnter = true;
            OnPlayerEnter.Invoke();
        }
    }

}
