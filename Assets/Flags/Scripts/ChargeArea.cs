using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChargeArea : MonoBehaviour 
{
    public Canvas areaCanvas;               // 区域画布
    public Slider slider;                   // 滑动条
    public float radius = 5;                // 区域半径

    private List<GameObject> playerList;    // 在区域内的所有玩家

    private void Awake()
    {
        areaCanvas.GetComponent<RectTransform>().sizeDelta = Vector2.one * radius * 2;
        GetComponent<SphereCollider>().radius = radius;
        playerList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        Debug.Log(other.GetComponent<TankInformation>().playerName + " Get In ");
        playerList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        Debug.Log(other.GetComponent<TankInformation>().playerName + " Get Out ");

        playerList.Remove(other.gameObject);
    }
}



