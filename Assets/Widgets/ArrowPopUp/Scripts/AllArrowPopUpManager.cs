using CameraRig;
using Item.Tank;
using UnityEngine;

namespace Widget.ArrowPopUp
{
    public class AllArrowPopUpManager : MonoBehaviour
    {
        public ObjectPool arrowPopUpPool;           // 箭头弹出池
        public float magnitude = 10f;               // 浮动范围

        /// <summary>
        /// 产生箭头们
        /// </summary>
        public void Spawn()
        {
            //将对象池的层级拖到自己的层级
            arrowPopUpPool.poolParent.transform.parent = gameObject.transform;
            if (AllPlayerManager.Instance == null || AllCameraRigManager.Instance == null)
                return;
            for (int i = 0; i < AllPlayerManager.Instance.Count; i++)
            {
                if (AllPlayerManager.Instance[i].IsAI)        //是AI就不显示
                    continue;
                ArrowPopUpManager arrowPopUp = arrowPopUpPool.GetNextObject().GetComponent<ArrowPopUpManager>();
                if (arrowPopUp == null)
                    continue;
                arrowPopUp.SetPosition(AllPlayerManager.Instance[i].transform.position + AllCameraRigManager.Instance.CurrentCamera.transform.up * magnitude);
                arrowPopUp.SetColor(AllPlayerManager.Instance[i].RepresentColor);
                arrowPopUp.SetText("P" + i);
            }
        }
    }
}
