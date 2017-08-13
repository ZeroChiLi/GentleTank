using UnityEngine;
using UnityEngine.UI;

public class ChargeButtonMananger : MonoBehaviour
{
    public Button button;
    public Image buttonEmptyImg;
    public Image buttonFullImg;
    public float coolDownTime = 1f;
    public Image sliderEmptyImg;
    public Image sliderFullImg;
    public float minValue = 0f;
    public float maxValue = 100f;
    public float changingRate = 10f;

    private CountDownTimer timer;
    private readonly string chargeTimer = "ChargeTimer";

    private void Start()
    {
        timer = new CountDownTimer();
        timer.AddOrResetTimer(chargeTimer, 0);
        buttonFullImg.fillAmount = 0f;
        sliderFullImg.fillAmount = 1f;
    }

    private void Update()
    {
        if (!timer.IsTimeUp(chargeTimer))
        {
            timer.UpdateTimer(chargeTimer, Time.deltaTime);
            buttonFullImg.fillAmount = GameMathf.Persents(0,coolDownTime, timer[chargeTimer]);
            return;
        }
        button.enabled = true;
    }


    public void OnClick()
    {
        if (!timer.IsTimeUp(chargeTimer))
            return;
        button.enabled = false;
        timer.AddOrResetTimer(chargeTimer, coolDownTime);
    }

    public void OnPointerDown()
    {

    }

    public void OnPointerUp()
    {

    }

    public void OnPointerExit()
    {

    }

}
