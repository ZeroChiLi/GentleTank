
using System.Collections;
using UnityEngine;

public class MedicalKitProp : PropBase
{
    public float healAmount = 150f;

    protected HealthManager targetHealth;

    protected override bool OnPlayerTouch(PlayerManager player)
    {
        targetHealth = player.GetComponent<HealthManager>();
        if (targetHealth == null)
            return false;
        targetHealth.SetHealthAmount(healAmount);
        return true;
    }

}
