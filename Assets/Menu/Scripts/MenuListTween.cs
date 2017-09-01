using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuListTween : MonoBehaviour 
{
    public float delay = 0.2f;
    public List<EasyTween> easyTweenList;

    private WaitForSeconds waitTime;
    private int index;

    private void Awake()
    {
        waitTime = new WaitForSeconds(delay);
    }

    public void Start()
    {
        OpenCloseAll(true);
    }

    public void OpenCloseAll(bool positive)
    {
        if (easyTweenList == null || easyTweenList.Count == 0)
            return;
        StartCoroutine(OpenCloseAllCoroutine(positive));
    }

    private IEnumerator OpenCloseAllCoroutine(bool positive)
    {
        index = positive ? 0 : (easyTweenList.Count - 1);
        while (index >= 0 && index < easyTweenList.Count)
        {
            easyTweenList[index].OpenCloseObjectAnimation();
            index += positive ? 1 : -1;
            yield return waitTime;
        }

    }

}
