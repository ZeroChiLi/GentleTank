using UnityEngine;

public class Shell : MonoBehaviour
{
    public ObjectPool shellExplosionPool;               // 爆炸特性池

    public LayerMask layerMask;                         // 坦克遮罩（"Level"）
    public float maxDamage = 100f;                      // 最大伤害
    public float explosionForce = 1000f;                // 爆炸中心的能量
    public float maxLifeTime = 2f;                      // 炸弹最大生存时间
    public float explosionRadius = 5f;                  // 爆炸半径


    // 当碰到任何物体
    private void OnTriggerEnter(Collider other)
    {
        // 从爆炸池中获取对象，并设置位置，显示之
        shellExplosionPool.SetNextObjectActive(transform);

        // 获取爆炸范围内所有碰撞体
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, layerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if (!targetRigidbody)
                continue;

            // 给一个爆炸力
            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            // 获取目标的血条，计算扣血量并给给扣血。
            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();
            if (!targetHealth)
                continue;
            targetHealth.TakeDamage(CalculateDamage(targetRigidbody.position));
        }

        gameObject.SetActive(false);
    }

    // 根据距离计算伤害
    private float CalculateDamage(Vector3 targetPosition)
    {
        // 计算爆炸中心距离和自己的距离
        Vector3 explosionToTarget = targetPosition - transform.position;
        float explosionDistance = explosionToTarget.magnitude;

        // 转换成比例
        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

        // 根据比例计算伤害
        return Mathf.Max(0f, relativeDistance * maxDamage);
    }

}
