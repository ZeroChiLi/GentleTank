using System;
using UnityEngine;

namespace GameSystem.OnlineGame
{
    [CreateAssetMenu(menuName = "Online/Online Object Pool")]
    public class OnlineObjectPool : ObjectPool, IPunPrefabPool
    {
        public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
        {
            throw new NotImplementedException();
        }

        public void Destroy(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
    }
}