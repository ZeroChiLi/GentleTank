using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MasterPanelManager : MonoBehaviour 
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI weightLimitText;

    private void Start()
    {
        SetMasterInfo(MasterManager.Instance.data.level, MasterManager.Instance.data.weightLimit);
    }

    public void SetMasterInfo(int level,float weightLimit)
    {
        levelText.text = string.Format("等级：{0}", level);
        weightLimitText.text = string.Format("承重：{0}", weightLimit);
    }


}
