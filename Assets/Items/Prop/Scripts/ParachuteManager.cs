using System.Collections;
using UnityEngine;

public class ParachuteManager : MonoBehaviour
{
    public Animator animator;
    public AnimationCurve fallCurve;        // 下降时的距离变化曲线
    public float fallTime = 3f;             // 下降时间
    public bool playeAtOnEnable = true;     // 激活时自动播放
    public Vector3 startPos;
    public Vector3 endPos;

    private CountDownTimer timer;           // 下降计时器
    public CountDownTimer Timer { get { return timer = timer ?? new CountDownTimer(fallTime, true); } }

    private bool isClosed = false;

    private void OnEnable()
    {
        if (playeAtOnEnable)
            Play();
    }

    /// <summary>
    /// 执行降落动画
    /// </summary>
    public void Play()
    {
        StartCoroutine(AnimationCoroutine(startPos, endPos));
    }

    public void Play(Vector3 startPos,Vector3 endPos)
    {
        StartCoroutine(AnimationCoroutine(startPos,endPos));
    }

    private IEnumerator AnimationCoroutine(Vector3 startPos, Vector3 finalPos)
    {
        gameObject.SetActive(true);
        transform.position = startPos;
        Timer.Start();
        animator.SetTrigger("Open");
        isClosed = false;
        while (!Timer.IsTimeUp)
        {
            transform.position = Vector3.Lerp(startPos, finalPos, fallCurve.Evaluate(Timer.GetPercent()));
            if (!isClosed && Timer.CurrentTime <= 1f)
            {
                animator.SetTrigger("Close");
                isClosed = true;
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }

}
