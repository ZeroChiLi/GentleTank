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

    /// <summary>
    /// 打开或关闭菜单列表，参数为列表执行顺序
    /// </summary>
    /// <param name="positive">是否正序</param>
    public void OpenCloseAll(bool positive)
    {
        if (easyTweenList == null || easyTweenList.Count == 0)
            return;
        StartCoroutine(OpenCloseAllCoroutine(positive));
    }

    /// <summary>
    /// 菜单开关协程
    /// </summary>
    /// <param name="positive">是否正序</param>
    /// <returns></returns>
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
