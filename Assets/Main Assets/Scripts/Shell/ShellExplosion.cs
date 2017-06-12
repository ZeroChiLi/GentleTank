using UnityEngine;

namespace Complete
{
    public class ShellExplosion : MonoBehaviour
    {
        public LayerMask layerMask;                         // 坦克遮罩（"Leve"）
        public ParticleSystem explosionParticles;           // 爆炸粒子
        public AudioSource explosionAudio;                  // 爆炸声音
        public float maxDamage = 100f;                      // 最大伤害
        public float explosionForce = 1000f;                // 爆炸中心的能量
        public float maxLifeTime = 2f;                      // 炸弹最大生存时间
        public float explosionRadius = 5f;                  // 爆炸半径

        private void Start ()
        {
            Destroy (gameObject, maxLifeTime);
        }

        // 当碰到任何物体
        private void OnTriggerEnter (Collider other)
        {
			// 获取爆炸范围内所有
            Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius, layerMask);

            for (int i = 0; i < colliders.Length; i++)
            {
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody> ();
                if (!targetRigidbody)
                    continue;

                // 给一个爆炸力
                targetRigidbody.AddExplosionForce (explosionForce, transform.position, explosionRadius);

                // 获取目标的血条，计算扣血量并给给扣血。
                TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();
                if (!targetHealth)
                    continue;
                float damage = CalculateDamage (targetRigidbody.position);
                targetHealth.TakeDamage (damage);
            }

            // 显示爆炸粒子
            explosionParticles.transform.parent = null;
            explosionParticles.Play();

            // 开启爆炸音效
            explosionAudio.Play();

            ParticleSystem.MainModule mainModule = explosionParticles.main;
            Destroy (explosionParticles.gameObject, mainModule.duration);
            Destroy (gameObject);
        }

        // 根据距离计算伤害
        private float CalculateDamage (Vector3 targetPosition)
        {
            // 计算爆炸中心距离和自己的距离
            Vector3 explosionToTarget = targetPosition - transform.position;
            float explosionDistance = explosionToTarget.magnitude;

            // 转换成比例
            float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

            // 根据比例计算伤害
            float damage = relativeDistance * maxDamage;

            damage = Mathf.Max (0f, damage);
            return damage;
        }
    }
}