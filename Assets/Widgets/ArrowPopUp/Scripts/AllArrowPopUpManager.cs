using Item.Tank;
using UnityEngine;

namespace Widget.ArrowPopUp
{
    public class AllArrowPopUpManager : MonoBehaviour
    {
        public ObjectPool arrowPopUpPool;           // 箭头弹出池
        public Camera targetCamera;                 // 面向的镜头
        public float magnitude = 10f;               // 浮动范围

        /// <summary>
        /// 产生箭头们
        /// </summary>
        public void Spawn()
        {
            //将对象池的层级拖到自己的层级
            arrowPopUpPool.poolParent.transform.parent = gameObject.transform;
            //    if (AllTanksManager.Instance == null)
            //        return;
            //    for (int i = 0; i < AllTanksManager.Instance.Count; i++)
            //    {
            //        if (AllTanksManager.Instance[i].isAI)        //是AI就不显示
            //            continue;
            //        ArrowPopUpManager arrowPopUp = arrowPopUpPool.GetNextObject().GetComponent<ArrowPopUpManager>();
            //        if (arrowPopUp == null)
            //            continue;
            //        arrowPopUp.SetPosition(AllTanksManager.Instance[i].Instance.transform.position + targetCamera.transform.up * magnitude);
            //        arrowPopUp.SetRotation(Quaternion.Euler(targetCamera.transform.rotation.eulerAngles + Vector3.right * 180));
            //        arrowPopUp.SetColor(AllTanksManager.Instance[i].playerColor);
            //        arrowPopUp.SetText("P" + i);
            //    }
        }
    }
}
