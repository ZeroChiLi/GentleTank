using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AllCustomTankPreviewManager : MonoBehaviour 
{
    public string defaultTankPath = "/Items/Tank/Prefabs/DefaultTanks";
    public string customTankPath = "/Items/Tank/Prefabs/CustomTanks";
    public Camera previewCamera;
    public Texture previewTexture;
    public Transform allTanksParent;
    public Vector3 startPos;
    public Vector3 offset = new Vector3(10,0,0);
    public Vector3 rotation = new Vector3(0, 150, 0);
    public List<Texture> textureList;

    private string fullDefaultTankPath { get { return Application.dataPath + defaultTankPath; } }
    private string fullCustomTankPath { get { return Application.dataPath + customTankPath; } }
    private List<GameObject> defaultTankList;
    private List<GameObject> customTankList;
    private int currentIndex;

    private void Awake()
    {
        defaultTankList = new List<GameObject>();
        customTankList = new List<GameObject>();
        GetAllPrefabs();
        SetupAllTanksPos();
    }

    private void GetAllPrefabs()
    {
        GetTanks(ref defaultTankList, fullDefaultTankPath, defaultTankPath);
        GetTanks(ref customTankList, fullCustomTankPath, customTankPath);
    }

    private bool GetTanks(ref List<GameObject> tanksList,string fullPath,string relativePath)
    {
        if (!Directory.Exists(fullPath))
        {
            Debug.LogError(fullPath + " Doesn't Exists");
            return false;
        }

        tanksList.Clear();
        FileInfo[] files = new DirectoryInfo(fullPath).GetFiles("*.prefab", SearchOption.AllDirectories);
        GameObject tank;

        for (int i = 0; i < files.Length; i++)
        {
            tank = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("{0}{1}{2}{3}", "Assets", relativePath, "/", files[i].Name))) as GameObject;
            if (tank != null)
                tanksList.Add(tank);
        }
        return true;
    }

    private void SetupAllTanksPos()
    {
        for (int i = 0; i < defaultTankList.Count; i++)
            SetupTankPos(defaultTankList[i].transform, i);

        for (int i = 0; i < customTankList.Count; i++)
            SetupTankPos(customTankList[i].transform, i + defaultTankList.Count);

    }

    private void SetupTankPos(Transform tankTransform,int index)
    {
        tankTransform.SetParent(allTanksParent);
        tankTransform.localPosition = startPos + (offset * index);
        tankTransform.localRotation = Quaternion.Euler(rotation);
    }
}
