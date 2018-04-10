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
    public Text enterText;              // 参加提示字符
    public InputButtons inputsButton;   // 输入虚按钮名字
    public RawImage selectedTankImage;  // 选择的坦克预览图片
    public UnityEvent OnPlayerEnter;    // 玩家进入事件

    public int CurrentTankIndex
    {
        get { return currentIndex; }
        set { currentIndex = (int)Mathf.Repeat(value, AllCustomTankManager.Instance.Count); }
    }
    private int currentIndex = 0;

    private int horizontalMove;

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
        if (!isEnter)
            return;
        if (Input.GetButtonDown(inputsButton.horizontalButton))
        {
            CurrentTankIndex += (int)Input.GetAxisRaw(inputsButton.horizontalButton);
            selectedTankImage.texture = AllCustomTankManager.Instance.textureList[CurrentTankIndex];
        }
    }

}
