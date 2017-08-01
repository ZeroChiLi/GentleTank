using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineShellPool : MonoBehaviour
{
    public ObjectPool onlineShellsPool;             // 炮弹池对象池

    private bool IsCreate;                          // 是否已经初始化创建了炮弹对象池

    /// <summary>
    /// 仅房主创建对象池共享
    /// </summary>
    private void Start()
    {
        
        onlineShellsPool.poolParent = gameObject;
        if (PhotonNetwork.isMasterClient)
        {
            IsCreate = true;
            onlineShellsPool.CreateObjectPool(gameObject);
        }
        Debug.Log(PhotonNetwork.isMasterClient + "   ShellsPool.Count  " + onlineShellsPool.objectCount + "   Find " + FindObjectsOfType<OnlineShell>().Length);
    }

    /// <summary>
    /// 查找所有子弹，存到列表里
    /// </summary>
    public void FindAllOnlineShells()
    {
        IsCreate = true;
        var componentArray = transform.root.GetComponentsInChildren(typeof(Transform), true);
        Debug.Log("find "+ componentArray.Length +"  "+ onlineShellsPool.objectCount);
        onlineShellsPool.PoolList = new List<GameObject>();
        for (int i = 0; i < componentArray.Length; i++)
            onlineShellsPool.AddOneMoreObject(componentArray[i].gameObject);
    }

    /// <summary>
    /// 获取下一个可用对象
    /// </summary>
    /// <param name="active">是否激活</param>
    /// <param name="transform">位置</param>
    /// <returns></returns>
    public GameObject GetNextObject(bool active = true, Transform transform = null)
    {
        if (!IsCreate)            //  对象池没创建（非房主）
            FindAllOnlineShells();
        return onlineShellsPool.GetNextObject(active, transform);
    }
}
