using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Configure/Object Pool")]
public class ObjectPool : ScriptableObject 
{
    [Header("Before Using It.")]
    [Header("Please Call 'CrateObjectPool()' Function.")]
    [Space(20)]
    public GameObject objectPerfab;             //预设
    public int objectCount = 10;                //数量
    public bool autoIncrease = true;            //如果需要自动增加
    public bool isPhotonView = false;           // 是否需要Photon同步

    [HideInInspector]
    public GameObject poolParent;               //对象池存放的父对象
    private List<GameObject> objectPool;        //对象池
    private int currentIndex = -1;              //当前索引

    public int Count { get { return objectPool.Count; } }
    public List<GameObject> PoolList { get { return objectPool; } set { objectPool = value; } }

    public GameObject this[int index]
    {
        get { return objectPool[index]; }
        set { objectPool[index] = value; }
    }

    /// <summary>
    /// 创建对象池
    /// </summary>
    public void CreateObjectPool(GameObject parent = null)
    {
        if (parent == null)     //如果没有设置父对象。创建一个空GameObject来存这些子对象
            poolParent = new GameObject(objectPerfab.name + " Pool");
        else
            poolParent = parent;
        objectPool = new List<GameObject>();
        for (int i = 0; i < objectCount; ++i)
        {
            GameObject obj;
            if (isPhotonView)
            {
                obj = PhotonNetwork.Instantiate(objectPerfab.name, Vector3.zero, Quaternion.identity, 0);
                obj.transform.parent = poolParent.transform;
            }
            else
                obj = Instantiate(objectPerfab, poolParent.transform);
            objectPool.Add(obj);
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// 设置所有对象激活状态
    /// </summary>
    /// <param name="active">是否激活</param>
    public void SetAllActive(bool active)
    {
        for (int i = 0; i < objectPool.Count; i++)
            objectPool[i].SetActive(active);
    }

    /// <summary>
    /// 获取下一个可用对象。
    /// </summary>
    /// <returns>返回有可用对象</returns>
    public GameObject GetNextObject(bool active = true, Transform transform = null)
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            int index = (currentIndex + 1 + i) % objectPool.Count;   //循环获取对象
            if (!objectPool[index].activeInHierarchy)
            {
                currentIndex = index;
                return SetupObject(objectPool[index],active,transform);
            }
        }
        if (autoIncrease)
            return SetupObject(AddOneMoreObject(), active, transform);
        return null;
    }

    /// <summary>
    /// 获取下一个可用对象同时激活，以及设置位置，并返回该对象
    /// </summary>
    /// <param name="transform">位置</param>
    /// <returns>放回这个对象</returns>
    private GameObject SetupObject(GameObject obj,bool active, Transform transform)
    {
        obj.SetActive(active);
        if (transform != null)
        {
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.transform.localScale = transform.localScale;
        }
        return obj;
    }

    /// <summary>
    /// 增加一个对象
    /// </summary>
    /// <param name="obj">新增对象</param>
    /// <returns>返回新增的对象</returns>
    public GameObject AddOneMoreObject(GameObject obj = null)
    {
        objectPool = objectPool ?? new List<GameObject>();  // 若没创建，那就先创建
        if (obj == null)
            obj = Instantiate(objectPerfab, poolParent.transform);
        else
            obj.transform.parent = poolParent.transform;
        objectPool.Add(obj);
        currentIndex = -1;
        return obj;
    }

    /// <summary>
    /// 清除所有对象
    /// </summary>
    public void CleanAll()
    {
        objectPool = null;
        currentIndex = -1;
    }

}
