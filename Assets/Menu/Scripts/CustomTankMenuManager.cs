using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomTankMenuManager : MonoBehaviour 
{
    static private CustomTankMenuManager instance;
    [HideInInspector]
    static public CustomTankMenuManager Instance { get { return instance; } }

    public AllCustomTankManager allCustomTank;
    public TextMeshProUGUI menuText;
    public Button changeBtn;
    public List<TankModulesTableManager> moduleTables;

    private int currentIndex;

    private void Awake()
    {
        instance = this;
        currentIndex = 0;
        menuText.text = moduleTables[currentIndex].tableName;
        changeBtn.interactable = false;
    }

    /// <summary>
    /// 隐藏所有表
    /// </summary>
    private void SetAllTableInActive()
    {
        for (int i = 0; i < moduleTables.Count; i++)
            moduleTables[i].gameObject.SetActive(false);
    }

    /// <summary>
    /// 换页，参数为true为换下一页，否则为上一页，循环的
    /// </summary>
    /// <param name="positive">是否下一页</param>
    public void SkipTable(bool positive)
    {
        changeBtn.interactable = false;
        AllCustomTankManager.Instance.ResetCurrentTank(true);
        SetAllTableInActive();
        currentIndex += (positive ? 1 : -1) + moduleTables.Count;       // 避免小于0
        currentIndex %= moduleTables.Count;
        menuText.text = moduleTables[currentIndex].tableName;
        moduleTables[currentIndex].gameObject.SetActive(true);
    }


}
