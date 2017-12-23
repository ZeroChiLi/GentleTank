using UnityEngine;

//对象池的集中初始化创建
public class ObjectPoolCreate : MonoBehaviour
{
    public ObjectPool[] objectPools;

    private GameObject allPoolParents;

    private void Awake()
    {
        allPoolParents = new GameObject("All Game Object Pools");
        for (int i = 0; i < objectPools.Length; i++)
        {
            if (objectPools[i] == null)
                continue;
            objectPools[i].CreateObjectPool();
            objectPools[i].PoolParent.transform.SetParent(allPoolParents.transform);
        }
    }
}
