
using System.Collections;
using UnityEngine;

public class MedicalKitProp : PropBase
{
    public float healAmount = 150f;
    public new Collider collider;
    public MeshRenderer meshRenderer;
    public Effect healEffect;

    protected HealthManager targetHealth;

    protected void OnEnable()
    {
        collider.enabled = true;
        meshRenderer.gameObject.SetActive(true);
        healEffect.gameObject.SetActive(false);
    }

    protected override void OnPlayerTouch(PlayerManager player)
    {
        targetHealth = player.GetComponent<HealthManager>();
        if (targetHealth == null)
            return;
        targetHealth.SetHealthAmount(healAmount);
        StartCoroutine(TouchSuccessed());
    }

    private IEnumerator TouchSuccessed()
    {
        if (healEffect != null)
        {
            collider.enabled = false;
            meshRenderer.gameObject.SetActive(false);
            healEffect.gameObject.SetActive(true);
            while (healEffect.isActiveAndEnabled)
                yield return null;
        }
        gameObject.SetActive(false);
    }

}
