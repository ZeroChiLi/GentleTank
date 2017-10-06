using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CurrentTankPanelManager : MonoBehaviour 
{
    public GameObject deleteConfirmPanel;                           // 删除确认窗口
    public Toast toast;                                             // 提示吐司
    public Button deleteButton;                                     // 删除按钮
    public Button selectButton;                                     // 选择按钮
    public AllCustomTankManager allCustomTank;                      // 所有坦克管理器
    public AllCustomTankPreviewManager allCustomTankPreview;        // 所有自定义坦克预览管理器
    public TankAssembleManager defaultTankAssemble;                 // 默认坦克组装（用来创建）
    public UnityEvent selectSuccessedEvent;                         // 选择成功事件

    private TankAssembleManager newTankAssemble;                    // 新建的坦克组装

    private void Start()
    {
        OnTankSelected();
    }

    /// <summary>
    /// 创建新的坦克
    /// </summary>
    public void CreateNewTank()
    {
        if (allCustomTank.Count >= allCustomTank.maxSize)
        {
            toast.ShowToast("坦克库已满。");
            return;
        }
        newTankAssemble = ScriptableObject.CreateInstance<TankAssembleManager>();
        newTankAssemble.CopyFrom(defaultTankAssemble);
        allCustomTank.AddNewTank(newTankAssemble);
        allCustomTankPreview.CatchTankTexture(allCustomTank.Count - 1);
        OnTankSelected();
    }

    /// <summary>
    /// 删除当前坦克
    /// </summary>
    public void DeleteCurrentTank()
    {
        if (allCustomTank.CurrentTank == null)
            return;
        allCustomTank.DeleteCurrentTank();
        StartCoroutine(allCustomTankPreview.SetupAllTankTexture());
        OnTankSelected();
    }

    /// <summary>
    /// 选择当前坦克
    /// </summary>
    public void SelectedCurrentTank()
    {
        if (!allCustomTank.SetCurrentTankToMaster())
            return;
        if (MasterManager.Instance.data.weightLimit < allCustomTank.CurrentTankAssemble.GetTotalWeight())
        {
            Toast.Instance.ShowToast("超出承重。");
            return;
        }
        selectSuccessedEvent.Invoke();

    }

    /// <summary>
    /// 选择坦克时响应
    /// </summary>
    public void OnTankSelected()
    {
        deleteButton.interactable = allCustomTank.CurrentTank == null ? false : true;
        selectButton.interactable = allCustomTank.CurrentTank == null ? false : true;
    }

}
