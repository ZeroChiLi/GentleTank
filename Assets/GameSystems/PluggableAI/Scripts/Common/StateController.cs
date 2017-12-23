using Item.Tank;
using UnityEngine;
using UnityEngine.AI;

namespace GameSystem.AI
{
    public class StateController : MonoBehaviour
    {
        public State currentState;                              // 当前状态
        public State remainState;                               // 保持当前状态
        public AIState defaultStats;                            // 默认状态信息
        public Point eyesPoint;                                 // 眼睛：拿来观察状态变化
        public Rigidbody rigidbodySelf;                         // 自己的刚体
        public Collider colliderSelf;                           // 自己的Collider
        public NavMeshAgent navMeshAgent;                       // 导航组件
        public Points waypoints;

        public SafeDictionary<CommonCode, object> statePrefs;           // 用于每一次状态保存信息使用时（OnExitState时清除）
        public SafeDictionary<CommonCode, object> instancePrefs;        // 用于整个实例保存信息用（如ChaseEnemy）

        [HideInInspector]
        public PlayerManager playerManager;                     // 玩家信息
        [HideInInspector]
        public HealthManager healthManager;                     // 玩家血量管理器
        [HideInInspector]
        public AttackManager attackManager;                     // 玩家攻击管理器
        private State startState;                               // 初始状态，每次复活后重置
        private Transform target;                               // 追踪目标

        private int nextWaypointIndex;                          // 下一个巡逻点
        public Point NextWaypoint { get { return waypoints[nextWaypointIndex]; } }

        public TeamManager Team { get { return playerManager.Team; } }

        /// <summary>
        /// 获取组件
        /// </summary>
        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            healthManager = GetComponent<HealthManager>();
            attackManager = GetComponent<AttackManager>();
            rigidbodySelf = GetComponent<Rigidbody>();
            colliderSelf = GetComponent<Collider>();
            startState = currentState;
            statePrefs = new SafeDictionary<CommonCode, object>();
            instancePrefs = new SafeDictionary<CommonCode, object>();
        }

        /// <summary>
        /// 配置导航信息，重置状态
        /// </summary>
        private void OnEnable()
        {
            SetupNavigation();
            currentState = startState;                          //复活的时候重置状态
        }

        /// <summary>
        /// 失效时同时清除状态信息
        /// </summary>
        private void OnDisable()
        {
            OnExitState();
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        private void Update()
        {
            currentState.UpdateState(this);                     //更新状态
        }

        /// <summary>
        /// 设置巡逻点
        /// </summary>
        public void SetWaypoints(Points points)
        {
            waypoints = points;
        }

        /// <summary>
        /// 初始化导航的变量
        /// </summary>
        private void SetupNavigation()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.enabled = true;
            navMeshAgent.speed = defaultStats.navSpeed.GetRandomValue();
            navMeshAgent.angularSpeed = defaultStats.navAngularSpeed.GetRandomValue();
            navMeshAgent.acceleration = defaultStats.navAcceleration.GetRandomValue();
            navMeshAgent.stoppingDistance = defaultStats.navStopDistance.GetRandomValue();
        }

        /// <summary>
        /// 配置AI
        /// </summary>
        /// <param name="enable">是否可用</param>
        public void SetupAI(bool enable)
        {
            enabled = enable;
            navMeshAgent.enabled = enable;
            UpdateNextWayPoint(true);
        }

        /// <summary>
        /// 转换到下一个状态
        /// </summary>
        /// <param name="nextState">下一个状态</param>
        public void TransitionToState(State nextState)
        {
            if (nextState != remainState)
            {
                currentState = nextState;
                OnExitState();
            }
        }

        /// <summary>
        /// 改变状态后调用
        /// </summary>
        private void OnExitState()
        {
            statePrefs.Clear();
        }

        /// <summary>
        /// 更新下一个目标巡逻点
        /// </summary>
        /// <param name="isRandom">是否随机</param>
        /// <returns>返回下一个巡逻点</returns>
        public Point UpdateNextWayPoint(bool isRandom)
        {
            nextWaypointIndex = isRandom ? Random.Range(0, waypoints.Count - 1) : (nextWaypointIndex + 1) % waypoints.Count;
            return waypoints[nextWaypointIndex];
        }

        /// <summary>
        /// 攻击
        /// </summary>
        public void Attack()
        {
            attackManager.Attack(defaultStats.attackForce.GetRandomValue(), defaultStats.attackDamage.GetRandomValue(), defaultStats.attackRate.GetRandomValue());
        }

        /// <summary>
        /// 是否被攻击了
        /// </summary>
        /// <returns>被攻击状态</returns>
        public bool IsFeelPain()
        {
            return healthManager.IsFeelPain;
        }

        /// <summary>
        /// 通过碰撞体判断是否队友
        /// </summary>
        /// <param name="collider">碰撞的物体</param>
        /// <returns>是否是队友</returns>
        public bool IsTeamMate(Collider collider)
        {
            return playerManager.IsTeammate(collider.GetComponent<PlayerManager>());
        }

        /// <summary>
        /// 碰撞是否是自己
        /// </summary>
        /// <param name="collider">碰撞的物体</param>
        /// <returns>是否是自己</returns>
        public bool IsMyself(Collider collider)
        {
            return collider == colliderSelf;
        }

        /// <summary>
        /// 查找敌人，并存到instancePrefs
        /// </summary>
        /// <returns>是否查找到敌人</returns>
        public bool FindEnemy(Quaternion anger, float radius, Color debugColor, bool isFromEyes = true)
        {
            RaycastHit hit;
            if (isFromEyes)
                target = FindTarget.FindEnemy(out hit, eyesPoint.GetWorldPosition(transform), eyesPoint.GetWorldForward(transform), playerManager, anger, radius, debugColor);
            else
                target = FindTarget.FindEnemy(out hit, transform.position, transform.forward, playerManager, anger, radius, debugColor);
            if (target != null)
            {
                instancePrefs.AddOrModifyValue(CommonCode.ChaseEnemy, target);
                return true;
            }
            return false;
        }
    }
}