using UnityEngine;
using UnityEngine.UI;

public class CurrentTankPanelManager : MonoBehaviour 
{
    public GameObject deleteConfirmPanel;
    public Toast toast;
    public AllCustomTankManager allCustomTank;
    public AllCustomTankPreviewManager allCustomTankPreview;
    public TankAssembleManager defaultTankAssemble;

    public void CreateNewTank()
    {
        if (allCustomTank.Count >= allCustomTank.maxSize)
        {
            toast.ShowToast("坦克库已满。");
            return;
        }
        allCustomTank.AddNewTank(defaultTankAssemble);
        allCustomTankPreview.CatchTankTexture(allCustomTank.Count - 1);
    }

    public void DeleteCurrentTank()
    {

    }

    public void SelectedCurrentTank()
    {

    }

}
