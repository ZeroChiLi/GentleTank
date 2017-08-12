
namespace Item.Tank
{
    public class TankHealth : HealthManager
    {
        public ObjectPool tankExplosionPool;                // 坦克爆炸特效池
        public ObjectPool tankBustedPool;                   // 坦克残骸池

        private PlayerManager playerManager;                // 玩家信息

        /// <summary>
        /// 获取玩家信息
        /// </summary>
        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        /// <summary>
        /// 激活时开启滑动条，并初始化
        /// </summary>
        private void OnEnable()
        {
            healthSlider.gameObject.SetActive(true);
            Init();
        }

        /// <summary>
        /// 失活脚本同时关闭滑动条
        /// </summary>
        private void OnDisable()
        {
            healthSlider.gameObject.SetActive(false);
        }

        /// <summary>
        /// 更新受伤感受时间
        /// </summary>
        private void Update()
        {
            UpdateFeelPainByDeltaTime();
        }

        /// <summary>
        /// 血量改变时，默认操作就好了
        /// </summary>
        protected override void OnHealthChanged() { }

        /// <summary>
        /// 死亡事件（爆炸特性、产生残骸）
        /// </summary>
        protected override void OnDead()
        {
            tankExplosionPool.GetNextObject(transform: gameObject.transform);
            gameObject.SetActive(false);
            tankBustedPool.GetNextObject().GetComponent<BustedTankMananger>().SetupBustedTank(transform, playerManager.RepresentColor);
        }

    }
}