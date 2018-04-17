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
    public bool isEnter;                // 是否已经点击参加
    public GameObject enterKey;
    public GameObject arrowsKey;
    public Toggle aiToggle;
    public RawImage selectedTankImage;  // 选择的坦克预览图片
    public InputButtons inputsButton;   // 输入虚按钮名字
    public UnityEvent OnPlayerEnter;    // 玩家进入事件

    public int CurrentTankIndex
    {
        get { return currentIndex; }
        set { currentIndex = (int)Mathf.Repeat(value, AllCustomTankManager.Instance.Count); }
    }
    private int currentIndex = 0;

    private int horizontalMove;

    public void OnEnable()
    {
        isEnter = false;
        OpenCloseEnterPanel(true);
    }

    public void Update()
    {
        if (!isEnter && Input.GetButtonDown(inputsButton.enterButton))
        {
            isEnter = true;
            OnPlayerEnter.Invoke();
        }
        if (!isEnter)
            return;
        if (Input.GetButtonDown(inputsButton.horizontalButton))
        {
            CurrentTankIndex += (int)Input.GetAxisRaw(inputsButton.horizontalButton);
            selectedTankImage.texture = AllCustomTankManager.Instance.textureList[CurrentTankIndex];
        }
    }

    public void OpenCloseEnterPanel(bool open)
    {
        enterKey.SetActive(open);
        arrowsKey.SetActive(!open);
        aiToggle.gameObject.SetActive(!open);
        selectedTankImage.gameObject.SetActive(!open);
    }

}
