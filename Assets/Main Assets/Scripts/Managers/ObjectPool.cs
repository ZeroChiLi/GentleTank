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

    private List<GameObject> objectPool;        //对象池
    private int currentIndex = -1;              //当前索引

    public GameObject this[int index]
    {
        get { return objectPool[index]; }
        set { objectPool[index] = value; }
    }

    /// <summary>
    /// 创建对象池
    /// </summary>
    public void CreateObjectPool()
    {
        //创建一个空GameObject来存这些子对象
        GameObject parent = new GameObject(objectPerfab.name + " Pool");
        objectPool = new List<GameObject>();
        for (int i = 0; i < objectCount; ++i)
        {
            GameObject obj = Instantiate(objectPerfab,parent.transform);
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
    public GameObject GetNextObject()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            int index = (currentIndex + 1 + i) % objectPool.Count;   //循环获取对象
            if (!objectPool[index].activeInHierarchy)
            {
                currentIndex = index;
                return objectPool[index];
            }
        }
        if (autoIncrease)
            return AddOneMoreObject();
        return null;
    }

    /// <summary>
    /// 获取下一个可用对象被激活，以及设置位置，并返回该对象
    /// </summary>
    /// <param name="transform">位置</param>
    /// <returns>放回这个对象</returns>
    public GameObject GetNextObjectActive(Transform transform)
    {
        GameObject obj = GetNextObject();
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// 增加一个对象
    /// </summary>
    /// <returns>返回新增的对象</returns>
    public GameObject AddOneMoreObject()
    {
        GameObject obj = Instantiate(objectPerfab);
        objectPool.Add(obj);
        objectCount = objectPool.Count;
        currentIndex = objectPool.Count - 1;         //用-1也行
        return obj;
    }
    
}
