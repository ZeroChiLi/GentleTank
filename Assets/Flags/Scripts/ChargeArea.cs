using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChargeArea : MonoBehaviour 
{
    public Slider slider;

    private int currentTeamID;
    private List<int> playerIDList;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        //other.gameObject.GetComponent<Tank>
    }
}



