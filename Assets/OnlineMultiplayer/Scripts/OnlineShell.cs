using UnityEngine;

public class OnlineShell : Photon.MonoBehaviour
{
    public ObjectPool shellExplosionPool;               // 爆炸特性池

    public LayerMask layerMask;                         // 坦克遮罩（"Level"）
    public float maxDamage = 100f;                      // 最大伤害
    public float explosionForce = 100f;                 // 爆炸中心的能量
    public float maxLifeTime = 2f;                      // 炸弹最大生存时间
    public float explosionRadius = 5f;                  // 爆炸半径

    private Collider[] colliders;                       // 碰撞物体们
    private Rigidbody targetRigidbody;                  // 目标刚体
    private OnlineTankHealth targetHealth;              // 目标血量
    private bool isExplosion;                           // 爆炸过了

    /// <summary>
    /// 设置层级
    /// </summary>
    private void Start()
    {
        transform.parent = GameObject.FindGameObjectWithTag("ShellPool").transform;
    }

    /// <summary>
    /// 当碰到任何物体，"Level"遮罩层下
    /// </summary>
    /// <param name="other">碰到的物体</param>
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        // 从爆炸池中获取对象，并设置位置，显示之
        shellExplosionPool.GetNextObject(transform: transform);

        if (PhotonNetwork.isMasterClient)               // 房主作为基准
            photonView.RPC("Explosion", PhotonTargets.AllViaServer,transform.position);
    }

    /// <summary>
    /// 爆炸
    /// </summary>
    [PunRPC]
    public void Explosion(Vector3 position)
    {
        // 获取爆炸范围内所有碰撞体
        colliders = Physics.OverlapSphere(position, explosionRadius, layerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            AddForce(colliders[i]);
            TakeDamage(position,colliders[i]);
        }

    }

    /// <summary>
    ///  给一个爆炸力
    /// </summary>
    /// <param name="collider">碰撞到的物体</param>
    private void AddForce(Collider collider)
    {
        targetRigidbody = collider.GetComponent<Rigidbody>();
        if (!targetRigidbody)
            return;
        targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
    }

    /// <summary>
    /// 计算位置给予伤害
    /// </summary>
    /// <param name="collider">碰撞的物体</param>
    private void TakeDamage(Vector3 center, Collider collider)
    {
        targetHealth = collider.GetComponent<OnlineTankHealth>();
        if (!targetHealth)
            return;
        targetHealth.TakeDamage(CalculateDamage(center,targetRigidbody.position));
    }

    /// <summary>
    /// 根据距离计算伤害
    /// </summary>
    /// <param name="center">爆炸中心位置</param>
    /// <param name="targetPosition">目标的位置</param>
    /// <returns></returns>
    private float CalculateDamage(Vector3 center, Vector3 targetPosition)
    {
        // 计算爆炸中心距离和自己的距离，并转换成比例
        float relativeDistance = (explosionRadius - (targetPosition - center).magnitude) / explosionRadius;

        // 根据比例计算伤害
        return Mathf.Max(0f, relativeDistance * maxDamage);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
