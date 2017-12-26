using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpeedUpProp : PropBase
{
    [Range(-1f, 1f)]
    public float accelerateRatio = 0.5f;        // 加速比例
    public float duaration = 5f;                // 持续时间

    private MoveManager targetMove;
    private NavMeshAgent targetNav;

    protected override bool OnPlayerTouch(PlayerManager player)
    {
        targetMove = player.GetComponent<MoveManager>();
        if (targetMove)
            player.StartCoroutine(AccelerateForSceconds(targetMove, targetMove.speed * accelerateRatio));
        targetNav = player.GetComponent<NavMeshAgent>();
        if (targetNav)
            player.StartCoroutine(AccelerateForSceconds(targetNav, targetNav.speed * accelerateRatio));

        return targetMove || targetNav; // 只有存在一个就是道具使用成功
    }

    /// <summary>
    /// 为人工操作速度加速
    /// </summary>
    private IEnumerator AccelerateForSceconds(MoveManager targetMove, float amount)
    {
        targetMove.speed += amount;
        yield return new WaitForSeconds(duaration);
        targetMove.speed -= amount;
    }

    /// <summary>
    /// 为AI操作速度加速
    /// </summary>
    private IEnumerator AccelerateForSceconds(NavMeshAgent targetNav, float amount)
    {
        targetNav.speed += amount;
        yield return new WaitForSeconds(duaration);
        targetNav.speed -= amount;
    }
}
