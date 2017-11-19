using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonTag : MonoBehaviour 
{
    public CoolDownButton target;

    private void OnEnable()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnSkillButtonClick);
        }
    }

    public void OnSkillButtonClick()
    {
        SkillEventSystem.Instance.SkillButtonClicked(target);
    }

}
