using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Configure/Object Pool")]
public class ObjectPool : ScriptableObject
{
    [Header("Before Using It.")]
    [Header("Please Call 'CrateObjectPool()' Function.")]
    [Space(20)]
    public GameObject objectPerfab;             //预设
    public int objectCount = 3;                 //数量
    public bool autoIncrease = true;            //如果需要自动增加

    private GameObject poolParent;              //对象池存放的父对象
    public GameObject PoolParent { get { CheckObjectPool(); return poolParent; } set { CheckObjectPool(); poolParent = value; } }

    private bool isCreated = false;
    private List<GameObject> objectPool;        //对象池
    private int currentIndex = -1;              //当前索引
    private GameObject allPoolParent;

    public int Count { get { CheckObjectPool(); return objectPool.Count; } }
    public GameObject this[int index]
    {
        get { CheckObjectPool(); return objectPool[index]; }
        set { CheckObjectPool(); objectPool[index] = value; }
    }

    private void OnEnable()
    {
        isCreated = false;
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
            obj = Instantiate(objectPerfab, poolParent.transform);
            objectPool.Add(obj);
            obj.SetActive(false);
        }
        isCreated = true;
    }

    /// <summary>
    /// 检测对象是否创建，如果没有，自动创建
    /// </summary>
    private void CheckObjectPool()
    {
        if (isCreated && objectPool == null)
            return;
        CreateObjectPool();
        allPoolParent = GameObject.FindWithTag("ObjectPools");
        if (allPoolParent == null)
        {
            allPoolParent = new GameObject("AllObjectPools");
            allPoolParent.tag = "ObjectPools";
        }
        poolParent.transform.SetParent(allPoolParent.transform);
    }

    /// <summary>
    /// 获取下一个可用对象。
    /// </summary>
    public GameObject GetNextObject(bool active = true)
    {
        CheckObjectPool();
        for (int i = 0; i < objectPool.Count; i++)
        {
            int index = (currentIndex + 1 + i) % objectPool.Count;   //循环获取对象
            if (!this[index].activeInHierarchy)
            {
                currentIndex = index;
                return SetupObject(objectPool[index], active);
            }
        }
        return autoIncrease ? SetupObject(AddOneMoreObject(), active) : null;
    }

    public GameObject GetNextObject(Vector3 position, Quaternion rotation, bool active = true)
    {
        GameObject obj = GetNextObject(active);
        if (obj)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
        }
        return obj;
    }

    public GameObject GetNextObject(Vector3 position, bool active = true)
    {
        GameObject obj = GetNextObject(active);
        if (obj)
            obj.transform.position = position;
        return obj;
    }

    public GameObject GetNextObject(Transform transform, bool active = true)
    {
        return GetNextObject(transform.position, transform.rotation, active);
    }

    public GameObject GetNextObject(Point point, bool active = true)
    {
        return GetNextObject(point.position, point.rotation, active);
    }

    /// <summary>
    /// 获取下一个可用对象同时激活，以及设置位置，并返回该对象
    /// </summary>
    private GameObject SetupObject(GameObject obj, bool active)
    {
        obj.SetActive(active);
        return obj;
    }

    /// <summary>
    /// 增加一个对象
    /// </summary>
    /// <param name="obj">新增对象</param>
    /// <returns>返回新增的对象</returns>
    private GameObject AddOneMoreObject(GameObject obj = null)
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
    /// 失效池所有对象
    /// </summary>
    public void InactiveAll()
    {
        CheckObjectPool();
        for (int i = 0; i < objectPool.Count; i++)
            objectPool[i].SetActive(false);
    }
}
