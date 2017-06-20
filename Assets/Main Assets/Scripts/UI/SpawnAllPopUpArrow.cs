using UnityEngine;

public class SpawnAllPopUpArrow : MonoBehaviour 
{
    public ObjectPool popUpArrowPool;

    public void Spawn(AllTanksManager allTanksManager)
    {
        popUpArrowPool.GetPoolParent().transform.parent = gameObject.transform;
        for (int i = 0; i < allTanksManager.Length; i++)
        {
            if (allTanksManager[i].isAI)        //是AI就不显示
                continue;
            ArrowPopUp arrowPopUp = popUpArrowPool.GetNextObjectActive().GetComponent<ArrowPopUp>();
            if (arrowPopUp == null)
                continue;
            arrowPopUp.Setposition(allTanksManager[i].Instance.transform.position);
            arrowPopUp.SetColor(allTanksManager[i].playerColor);
            arrowPopUp.SetText("P" + i);
        }
    }


}
