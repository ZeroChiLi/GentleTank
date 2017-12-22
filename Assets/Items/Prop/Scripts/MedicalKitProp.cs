
public class MedicalKitProp : PropBase
{
    public float healAmount = 150f;

    protected HealthManager targetHealth;

    protected override void OnPlayerTouch(PlayerManager player)
    {
        targetHealth = player.GetComponent<HealthManager>();
        if (targetHealth == null)
            return;
        targetHealth.SetHealthAmount(healAmount);
        gameObject.SetActive(false);
    }
}
