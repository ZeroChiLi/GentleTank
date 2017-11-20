using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillEventSystem : MonoBehaviour
{
    static public SkillEventSystem Instance { private set; get; }

    public EventSystem eventSystem;

    public bool skillButtonClicked;
    public SkillButton firstClick;

    private void Awake() { Instance = this; }

    private void Update()
    {
        if (firstClick != null)
        {
            if (firstClick.OnSecondClickedCanceled())
            {
                firstClick = null;
            }
            else if (firstClick.OnSecondClicked())
            {
                firstClick.OnSecondClickedSuccessed();
            }
            
        }
    }

    public void SkillButtonClicked(SkillButton target)
    {
        if (firstClick == null)
        {
            firstClick = target;
        }
    }

}
