using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TankAssemble : ScriptableObject 
{
    public string tankName;
    public TankModule bodyModule;
    public TankModule turretModule;
    public TankModule wheelLeftModule;
    public TankModule wheelRightModule;

    private GameObject body;
    private GameObject turret;
    private GameObject wheelLeft;
    private GameObject wheelRight;

    public GameObject Create()
    {
        if (!Check())
            return null;

        //PrefabUtility.CreatePrefab(path, obj);
        GameObject newTank = new GameObject(tankName);
        body = Instantiate(bodyModule.prefab, newTank.transform);
        turret = Instantiate(turretModule.prefab, newTank.transform);
        wheelLeft = Instantiate(wheelLeftModule.prefab, newTank.transform);
        wheelRight = Instantiate(wheelRightModule.prefab, newTank.transform);

        turret.transform.localPosition = bodyModule[ModuleType.Turret].anchor - turretModule[ModuleType.Body].anchor;
        wheelLeft.transform.localPosition = bodyModule[ModuleType.WheelLeft].anchor - wheelLeftModule[ModuleType.Body].anchor;
        wheelRight.transform.localPosition = bodyModule[ModuleType.WheelRight].anchor - wheelRightModule[ModuleType.Body].anchor;




        return newTank;
    }

    public bool Check()
    {
        return (wheelLeftModule != null && wheelLeftModule.type == ModuleType.WheelLeft
            && wheelRightModule != null && wheelRightModule.type == ModuleType.WheelRight
            && bodyModule != null && bodyModule.type == ModuleType.Body
            && turretModule != null && turretModule.type == ModuleType.Turret);
    }

}
