using UnityEngine;

namespace GameSystem.OnlineGame
{
    public class OnlineShellPool : MonoBehaviour
    {
        public ObjectPool onlineShellsPool;             // 炮弹池对象池

        /// <summary>
        /// 初始化对象池
        /// </summary>
        private void Awake()
        {
            onlineShellsPool.CleanAll();
        }

        /// <summary>
        /// 仅房主创建对象池共享
        /// </summary>
        private void Start()
        {
            onlineShellsPool.poolParent = gameObject;
            if (PhotonNetwork.isMasterClient)
                onlineShellsPool.CreateObjectPool(gameObject);
        }

        /// <summary>
        /// 添加对象到对象池中
        /// </summary>
        /// <param name="obj">对象</param>
        public void AddToPool(GameObject obj)
        {
            onlineShellsPool.AddOneMoreObject(obj);
            Debug.Log("Add to pool  " + onlineShellsPool.Count);
        }

        /// <summary>
        /// 获取下一个可用对象
        /// </summary>
        /// <param name="active">是否激活</param>
        /// <param name="transform">位置</param>
        /// <returns></returns>
        public GameObject GetNextObject(bool active = true, Transform transform = null)
        {
            return onlineShellsPool.GetNextObject(active, transform);
        }
    }
}