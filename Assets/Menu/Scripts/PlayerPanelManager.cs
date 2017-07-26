using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelManager : MonoBehaviour 
{
    public Text playerNameText;

    public void Init(string playerName)
    {
        playerNameText.text = playerName;
    }

}
