using UnityEngine;

public class PlayerInfoUI : MonoBehaviour 
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
        
    private void Update()
    {
        //Debug.Log("Position : " + mainCamera.transform.position + " --- eulerAngles : " + mainCamera.transform.rotation.eulerAngles);
        //将UI对着相机设备
        GetComponent<RectTransform>().rotation = Quaternion.LookRotation(-1 * mainCamera.transform.position)/* * Quaternion.Euler(mainCamera.transform.rotation.eulerAngles - new Vector3(0,40,0))*/;
    }

    // 设置显示到镜头玩家名字
    public void SetNameText(string name)
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
        GetComponent<TextMesh>().text = name;
    }
}
