using UnityEngine;
using UnityEngine.UI;

public class CurrentTankPanelManager : MonoBehaviour 
{
    public GameObject deleteConfirmPanel;
    public Toast toast;
    public AllCustomTankManager allCustomTank;
    public AllCustomTankPreviewManager allCustomTankPreview;
    public TankAssembleManager defaultTankAssemble;

    private TankAssembleManager newTankAssemble;

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
    }

    public void DeleteCurrentTank()
    {

    }

    public void SelectedCurrentTank()
    {

    }

}
