using UnityEngine;

public class SpawnAllPopUpArrow : MonoBehaviour
{
    public ObjectPool popUpArrowPool;
    public Camera targetCamera;
    public float distance = 10f;

    public void Spawn(AllTanksManager allTanksManager)
    {
        //将对象池的层级拖到自己的层级
        popUpArrowPool.poolParent.transform.parent = gameObject.transform;
        for (int i = 0; i < allTanksManager.Count; i++)
        {
            if (allTanksManager[i].isAI)        //是AI就不显示
                continue;
            ArrowPopUp arrowPopUp = popUpArrowPool.GetNextObject().GetComponent<ArrowPopUp>();
            if (arrowPopUp == null)
                continue;
            arrowPopUp.SetPosition(allTanksManager[i].Instance.transform.position + targetCamera.transform.up * distance);
            arrowPopUp.SetRotation(Quaternion.Euler(targetCamera.transform.rotation.eulerAngles + Vector3.right *180));
            arrowPopUp.SetColor(allTanksManager[i].playerColor);
            arrowPopUp.SetText("P" + i);
        }
    }


}
