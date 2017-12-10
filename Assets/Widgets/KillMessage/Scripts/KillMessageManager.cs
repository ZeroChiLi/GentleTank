using TMPro;
using UnityEngine;

public class KillMessageManager : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public string killSprite = "<sprite=0>";
    public float duration = 3f;
    public Color startColor = Color.white;
    public Color endColor = new Color(1, 1, 1, 0);
    public AnimationCurve colorCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private CountDownTimer timer;
    private string temStr;

    private void Awake()
    {
        timer = new CountDownTimer(duration, false, false);
    }

    private void OnEnable()
    {
        timer.Start();
    }

    private void OnDisable()
    {
        textMesh.text = string.Empty;
        textMesh.color = startColor;
    }

    private void LateUpdate()
    {
        textMesh.color = Color.Lerp(startColor, endColor, colorCurve.Evaluate(timer.GetPercent()));
        if (timer.IsTimeUp)
            gameObject.SetActive(false);
    }


    /// <summary>
    /// 配置信息
    /// </summary>
    /// <param name="murderer">杀人者</param>
    /// <param name="killed">被害者</param>
    public void Setup(PlayerManager murderer, PlayerManager killed)
    {
        temStr = murderer == null ? "未知力量" : murderer.ColoredPlayerNameByTeam ;
        textMesh.text = string.Format("{0} {1} {2}", temStr, killSprite, killed.ColoredPlayerNameByTeam);
    }

}
