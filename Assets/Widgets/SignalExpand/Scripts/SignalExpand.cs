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
        get { return timer = timer == null ? new CountDownTimer(changeTime, false, false) : timer; }
        set { timer = value; }
    }

    public void Play()
    {
        Play(startScale, endScale, changeTime);
    }

    public void Play(Vector3 startScale, Vector3 endScale, float time)
    {
        this.startScale = startScale;
        this.endScale = endScale;
        changeTime = time;
        Timer.Reset(time);
        gameObject.SetActive(true);
        StartCoroutine(PlaySignalExpand());
    }

    public void Play(Vector3 startScale, Vector3 endScale, float time, Color color)
    {
        Play(startScale, endScale, time);
        mesh.material.color = new Color(color.r,color.g,color.b,0.2f);
    }

    private IEnumerator PlaySignalExpand()
    {
        while (!Timer.IsTimeUp)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, changeCurve.Evaluate(timer.GetPercent()));
            yield return null;
        }
        gameObject.SetActive(false);
    }

}
