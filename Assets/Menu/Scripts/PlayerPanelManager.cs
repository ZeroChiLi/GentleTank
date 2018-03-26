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
    public RawImage selectedTankImage;
    public AllCustomTankManager tanksManager;
    public AllCustomTankPreviewManager tanksPreview;
    public UnityEvent OnPlayerEnter;

    public int CurrentTankIndex
    {
        get { return currentIndex; }
        set { currentIndex = (int)Mathf.Repeat(value, tanksManager.Count - 1); }
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
        if (isEnter && Input.GetButtonDown(inputsButton.horizontalButton))
        {
            CurrentTankIndex += (int)Input.GetAxisRaw(inputsButton.horizontalButton);
            selectedTankImage.texture = tanksPreview.textureList[CurrentTankIndex];
        }
    }

}
