using System.Collections;
using UnityEngine;

public class SignalExpand : MonoBehaviour
{
    public Vector3 startScale = Vector3.one;
    public Vector3 endScale = Vector3.one * 5;
    public float changeTime = 2f;
    public MeshRenderer mesh;
    public AnimationCurve changeCurve = new AnimationCurve(new Keyframe(0, 0, 0, 2f), new Keyframe(1f, 1, 0, 0));

    private CountDownTimer timer;
    public CountDownTimer Timer
    {
        get { return timer = timer ?? new CountDownTimer(changeTime, false, false); }
        set { timer = value; }
    }

    public float CurrentPercent { get { return changeCurve.Evaluate(timer.GetPercent()); } }

    /// <summary>
    /// 开始信号显示
    /// </summary>
    public void Play()
    {
        Play(startScale, endScale, changeTime);
    }

    /// <summary>
    /// 开始信号显示
    /// </summary>
    public void Play(Vector3 startScale, Vector3 endScale, float time)
    {
        this.startScale = startScale;
        this.endScale = endScale;
        changeTime = time;
        Timer.Reset(time);
        gameObject.SetActive(true);
        StartCoroutine(PlaySignalExpand());
    }

    /// <summary>
    /// 开始信号显示
    /// </summary>
    /// <param name="startScale">起始缩放</param>
    /// <param name="endScale">结束时缩放</param>
    /// <param name="time">显示时间</param>
    /// <param name="color">显示颜色</param>
    public void Play(Vector3 startScale, Vector3 endScale, float time, Color color)
    {
        Play(startScale, endScale, time);
        mesh.material.color = new Color(color.r,color.g,color.b,0.2f);
    }

    private IEnumerator PlaySignalExpand()
    {
        while (!Timer.IsTimeUp)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, CurrentPercent);
            yield return null;
        }
        gameObject.SetActive(false);
    }

}
