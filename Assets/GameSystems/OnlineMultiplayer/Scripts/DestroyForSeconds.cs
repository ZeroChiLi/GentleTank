using UnityEngine;

namespace GameSystem.OnlineGame
{
    /// <summary>
    /// 一段时间后，自动设置为不激活状态
    /// </summary>
    public class DestroyForSeconds : Photon.MonoBehaviour
    {
        public float lifeTime = 5f;

        private float currentTime;

        private void Update()
        {
            if (currentTime > lifeTime)
                PhotonNetwork.Destroy(photonView);
            currentTime += Time.deltaTime;
        }

    }
}