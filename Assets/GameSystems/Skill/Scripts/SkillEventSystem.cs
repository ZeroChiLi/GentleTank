using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillEventSystem : MonoBehaviour
{
    static public SkillEventSystem Instance { private set; get; }

    public EventSystem eventSystem;

    public bool skillButtonClicked;
    public CoolDownButton firstClick;

    private void Awake() { Instance = this; }

    private void OnGUI()
    {
        //if ()
        //{

        //}
    }

    public void SkillButtonClicked(CoolDownButton target)
    {

    }

}
