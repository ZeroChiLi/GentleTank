using System.Collections;
using UnityEngine;

public class AttackUpProp : PropBase
{
    public float damage = 15f;                  // 增加伤害值
    public float duaration = 5f;                // 持续时间

    private AttackManager targetAttack;

    protected override bool OnPlayerTouch(PlayerManager player)
    {
        targetAttack = player.GetComponentInChildren<AttackManager>();
        if (targetAttack == null)
            return false;
        player.StartCoroutine(AttackUpForSceconds(targetAttack, damage));
        return true;
    }

    /// <summary>
    /// 提高伤害值
    /// </summary>
    private IEnumerator AttackUpForSceconds(AttackManager targetAttack, float amount)
    {
        targetAttack.additionDamage += amount;
        yield return new WaitForSeconds(duaration);
        targetAttack.additionDamage -= amount;
    }
}
