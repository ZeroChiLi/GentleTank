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
    public int playerIndex;
    public bool isEnter;                // 是否已经点击参加
    public GameObject enterKey;
    public GameObject enterPanel;
    //public GameObject arrowsKey;
    //public Toggle aiToggle;
    public RawImage selectedTankImage;  // 选择的坦克预览图片
    public InputButtons inputsButton;   // 输入虚按钮名字
    //public UnityEvent OnPlayerEnter;    // 玩家进入事件

    //public int CurrentTankIndex
    //{
    //    get { return currentIndex; }
    //    //set { currentIndex = (int)Mathf.Repeat(value, AllCustomTankManager.Instance.Count); }
    //    //set { currentIndex = (value + AllCustomTankManager.Instance.Count) % AllCustomTankManager.Instance.Count; }
    //    set { currentIndex = value; }
    //}
    //private int currentIndex = 0;
    private int index;

    public PlayerInformation Player { get { return AllPlayerManager.Instance.playerInfoList[playerIndex]; } }

    private int horizontalMove;

    private void OnEnable()
    {
        isEnter = false;
        OpenCloseEnterPanel(false);
        SetAssembleTank(index);
        SetJoin(isEnter);
        SetAI(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown(inputsButton.enterButton))
        {
            isEnter = !isEnter;
            SetJoin(isEnter);
            OpenCloseEnterPanel(isEnter);
        }
        if (!isEnter)
            return;
        if (Input.GetButtonDown(inputsButton.horizontalButton))
        {
            //CurrentTankIndex += (int)Input.GetAxisRaw(inputsButton.horizontalButton);
            //Debug.Log(CurrentTankIndex + "" + (int)Input.GetAxisRaw(inputsButton.horizontalButton) + "  " + AllCustomTankManager.Instance.Count);
            //selectedTankImage.texture = AllCustomTankManager.Instance.textureList[CurrentTankIndex];
            //SetAssembleTank(CurrentTankIndex);
            index += (int)Input.GetAxisRaw(inputsButton.horizontalButton);
            index = (int)Mathf.Repeat(index, AllCustomTankManager.Instance.Count);
            selectedTankImage.texture = AllCustomTankManager.Instance.textureList[index];
            SetAssembleTank(index);
        }
    }

    private void OpenCloseEnterPanel(bool open)
    {
        enterPanel.SetActive(open);
        enterKey.SetActive(!open);
    }

    private void SetAssembleTank(int index)
    {
        if (!AllCustomTankManager.Instance.tankAssembleList[index].IsValid())
            index = 0;
        Player.assembleTank = AllCustomTankManager.Instance.tankAssembleList[index];
    }

    private void SetJoin(bool isJoin)
    {
        Player.isJoin = isJoin;
    }

    public void SetAI(bool isAI)
    {
        Player.isAI = isAI;
    }

    public void SetAI(Toggle toggle)
    {
        Player.isAI = toggle.isOn;
    }
}
