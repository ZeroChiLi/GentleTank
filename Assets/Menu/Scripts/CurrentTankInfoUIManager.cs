using TMPro;
using UnityEngine;

public class CurrentTankInfoUIManager : MonoBehaviour 
{
    public TextMeshProUGUI weightText;
    public string weightPrefix = "重量：";

    private void Start()
    {
        AllCustomTankPreviewManager.Instance.allTankSetupHandle += ShowWeightOnAllTankSetup;
    }

    /// <summary>
    /// 在所有坦克配置完后更新当前坦克重量
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ShowWeightOnAllTankSetup(object sender, System.EventArgs e)
    {
        UpdateCurrentTankWeight();
    }

    /// <summary>
    /// 更新当前坦克组合的总重量
    /// </summary>
    public void UpdateCurrentTankWeight()
    {
        weightText.text = weightPrefix + AllCustomTankManager.Instance.CurrentTankAssemble.GetTotalWeight();
    }

    /// <summary>
    /// 更新临时坦克组合总重量
    /// </summary>
    public void UpdateTemporaryTankWeight()
    {
        weightText.text = weightPrefix + AllCustomTankManager.Instance.TemporaryAssemble.GetTotalWeight();
    }
}
