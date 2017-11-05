using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SignImageManager : MonoBehaviour
{
    public enum SignType { Exclamation, Question }

    public Image image;
    public Sprite exclamationSprite;
    public Sprite questionSprite;

    private void OnDisable()
    {
        image.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示符号一段时间
    /// </summary>
    /// <param name="type">符号类型</param>
    /// <param name="time">持续时间</param>
    public void ShowForSecond(SignType type,float time)
    {
        switch (type)
        {
            case SignType.Exclamation:
                image.sprite = exclamationSprite;
                break;
            case SignType.Question:
                image.sprite = questionSprite;
                break;
        }
        image.gameObject.SetActive(true);
        StartCoroutine(WaitToInactive(time));
    }

    /// <summary>
    /// 一段时间后隐藏
    /// </summary>
    public IEnumerator WaitToInactive(float time)
    {
        yield return new WaitForSeconds(time);
        image.gameObject.SetActive(false);
    }

}
