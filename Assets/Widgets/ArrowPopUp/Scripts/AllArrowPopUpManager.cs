using UnityEngine;

namespace Widget
{
    public class AllArrowPopUpManager : MonoBehaviour
    {
        public ObjectPool arrowPopUpPool;               // 箭头弹出池
        public ObjectPool arrowBillboardPool;
        public Vector3 offset = new Vector3(0, 10, 0);  // 偏移量
        /// <summary>
        /// 产生箭头们
        /// </summary>
        public void Spawn()
        {
            //将对象池的层级拖到自己的层级
            //arrowPopUpPool.poolParent.transform.parent = gameObject.transform;
            //arrowBillboardPool.poolParent.transform.SetParent(gameObject.transform);
            if (AllPlayerManager.Instance == null)
                return;
            for (int i = 0; i < AllPlayerManager.Instance.Count; i++)
            {
                if (AllPlayerManager.Instance[i].IsAI)        //是AI就不显示
                    continue;
                //arrowPopUpPool.GetNextObject().GetComponent<ArrowPopUpManager>().Init(AllPlayerManager.Instance[i].transform.position + offset, AllPlayerManager.Instance[i].RepresentColor, "P" + i);
                arrowBillboardPool.GetNextObject().GetComponent<ArrowBillboard>().Setup(AllPlayerManager.Instance[i].transform.position + offset, AllPlayerManager.Instance[i].RepresentColor);
            }
        }
    }
}
