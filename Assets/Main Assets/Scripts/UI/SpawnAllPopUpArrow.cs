using UnityEngine;

public class SpawnAllPopUpArrow : MonoBehaviour
{
    public ObjectPool popUpArrowPool;
    public Camera targetCamera;
    public float distance = 10f;

    public void Spawn()
    {
        //将对象池的层级拖到自己的层级
        popUpArrowPool.poolParent.transform.parent = gameObject.transform;
        for (int i = 0; i < AllTanksManager.Instance.Count; i++)
        {
            if (AllTanksManager.Instance[i].isAI)        //是AI就不显示
                continue;
            ArrowPopUp arrowPopUp = popUpArrowPool.GetNextObject().GetComponent<ArrowPopUp>();
            if (arrowPopUp == null)
                continue;
            arrowPopUp.SetPosition(AllTanksManager.Instance[i].Instance.transform.position + targetCamera.transform.up * distance);
            arrowPopUp.SetRotation(Quaternion.Euler(targetCamera.transform.rotation.eulerAngles + Vector3.right *180));
            arrowPopUp.SetColor(AllTanksManager.Instance[i].playerColor);
            arrowPopUp.SetText("P" + i);
        }
    }


}
