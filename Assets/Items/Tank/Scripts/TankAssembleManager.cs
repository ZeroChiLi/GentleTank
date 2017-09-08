using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TankModule/TankAssemble")]
public class TankAssembleManager : ScriptableObject 
{
    public string tankName = "tank";
    public TankModuleHead head;
    public TankModuleBody body;
    public TankModuleWheel leftWheel;
    public List<TankModuleOther> others;

    private GameObject headObj;
    private GameObject bodyObj;
    private GameObject leftWheelObj;
    private GameObject rightWheelObj;
    private List<GameObject> othersObj;

    /// <summary>
    /// 判断是否合格（可以组装成坦克）
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(tankName) && head && body && leftWheel;
    }

    public GameObject CreateTank()
    {
        if (!IsValid())
        {
            Debug.LogErrorFormat("Cerate Tank Filed. TankName Or Head Or Body Or LeftWheel Should Not Be Null.");
            return null;
        }
        GameObject newTank = new GameObject(tankName);
        InstantiateModules(newTank.transform);
        AssembleTank();
        return newTank;
    }

    public void InstantiateModules(Transform parent)
    {
        headObj = Instantiate(head.prefab, parent);
        bodyObj = Instantiate(body.prefab, parent);
        leftWheelObj = Instantiate(leftWheel.prefab, parent);
        rightWheelObj = Instantiate(leftWheel.prefab, parent);
        rightWheelObj.transform.localScale = new Vector3(-rightWheelObj.transform.localScale.x, rightWheelObj.transform.localScale.y, rightWheelObj.transform.localScale.z);
        //othersObj = new List<GameObject>();
        //if (others != null)
        //    for (int i = 0; i < others.Count; i++)
        //        othersObj.Add(Instantiate(others[i].prefab, parent));
    }

    public void AssembleTank()
    {
        TankModule.ConnectHeadToBody(head, headObj, body, bodyObj);
        TankModule.ConnectLeftWheelToBody(leftWheel, leftWheelObj, body, bodyObj);
        TankModule.ConnectRightWheelToBody(leftWheel, rightWheelObj, body, bodyObj);
        //for (int i = 0; i < othersObj.Count; i++)
        //    TankModule.ConnectOtherModule(others[i], othersObj[i], , );
    }
}
